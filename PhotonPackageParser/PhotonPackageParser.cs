using System;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using PcapDotNet.Packets.Transport;
using System.IO;
using System.Linq;

namespace PhotonPackageParser
{
    public class PhotonPackageParser
    {
        private readonly IPhotonPackageHandler _handler;

        private class SegmentedPackage
        {
            public int TotalLength;
            public int BytesWritten;
            public byte[] TotalPayload;
        }

        private static readonly Protocol16 Protocol16 = new Protocol16();

        private const int CommandHeaderLength = 12;
        private const int PhotonHeaderLength = 12;
        
        private readonly Dictionary<int, SegmentedPackage> _pendingSegments = new Dictionary<int, SegmentedPackage>();

        public PhotonPackageParser(IPhotonPackageHandler handler)
        {
            _handler = handler;
        }

        public void DeserializeMessageAndCallback(TransportDatagram datagram)
        {
            if (datagram.Payload.Length < PhotonHeaderLength)
                return;

            byte[] source = datagram.Payload.ToArray();
            int offset = 0;
            
            Protocol.Deserialize(out short peerId, source, ref offset);
            ReadByte(out byte crcEnabled, source, ref offset);
            ReadByte(out byte commandCount, source, ref offset);
            Protocol.Deserialize(out int timestamp, source, ref offset);
            Protocol.Deserialize(out int challenge, source, ref offset);

            for (var commandIdx = 0; commandIdx < commandCount; commandIdx++)
            {
                HandleCommand(source, ref offset);
            }
        }

        private void HandleCommand(byte[] source, ref int offset)
        {
            ReadByte(out byte commandType, source, ref offset);
            ReadByte(out byte channelId, source, ref offset);
            ReadByte(out byte commandFlags, source, ref offset);
            offset++; // Skip 1 byte
            Protocol.Deserialize(out int commandLength, source, ref offset);
            Protocol.Deserialize(out int sequenceNumber, source, ref offset);
            commandLength -= CommandHeaderLength;

            switch (commandType)
            {
                case 4:// Disconnect
                    return;
                case 7:// Send unreliable
                    offset += 4;
                    commandLength -= 4;
                    goto case 6;
                case 6:// Send reliable
                    HandleSendReliable(source, ref offset, ref commandLength);
                    break;
                case 8:// Send fragment
                    HandleSendFragment(source, ref offset, ref commandLength);
                    break;
                default:
                    offset += commandLength;
                    break;
            }
        }

        private void HandleSendReliable(byte[] source, ref int offset, ref int commandLength)
        {
            offset++;// Skip 1 byte
            commandLength--;
            ReadByte(out byte messageType, source, ref offset);
            commandLength--;

            int operationLength = commandLength;
            var payload = new StreamBuffer(operationLength);
            payload.Write(source, offset, operationLength);
            payload.Seek(0L, SeekOrigin.Begin);

            offset += operationLength;
            switch (messageType)
            {
                case 2:// Operation Request
                    var requestData = Protocol16.DeserializeOperationRequest(payload);
                    _handler.OnRequest(requestData.OperationCode, requestData.Parameters);
                    break;
                case 3:// Operation Response
                    var responseData = Protocol16.DeserializeOperationResponse(payload);
                    _handler.OnResponse(responseData.OperationCode, responseData.ReturnCode, responseData.Parameters);
                    break;
                case 4:// Event
                    var eventData = Protocol16.DeserializeEventData(payload);
                    _handler.OnEvent(eventData.Code, eventData.Parameters);
                    break;
            }
        }

        private void HandleSendFragment(byte[] source, ref int offset, ref int commandLength)
        {
            Protocol.Deserialize(out int startSequenceNumber, source, ref offset);
            commandLength -= 4;
            Protocol.Deserialize(out int fragmentCount, source, ref offset);
            commandLength -= 4;
            Protocol.Deserialize(out int fragmentNumber, source, ref offset);
            commandLength -= 4;
            Protocol.Deserialize(out int totalLength, source, ref offset);
            commandLength -= 4;
            Protocol.Deserialize(out int fragmentOffset, source, ref offset);
            commandLength -= 4;

            var fragmentLength = commandLength;
            HandleSegementedPayload(startSequenceNumber, totalLength, fragmentLength, fragmentOffset, source, ref offset);
        }

        private void HandleFinishedSegmentedPackage(byte[] totalPayload)
        {
            int offset = 0;
            int commandLength = totalPayload.Length;
            HandleSendReliable(totalPayload, ref offset, ref commandLength);
        }

        private void HandleSegementedPayload(int startSequenceNumber, int totalLength, int fragmentLength, int fragmentOffset, byte[] source, ref int offset)
        {
            var segmentedPackage = GetSegmentedPackage(startSequenceNumber, totalLength);

            Buffer.BlockCopy(source, offset, segmentedPackage.TotalPayload, fragmentOffset, fragmentLength);
            offset += fragmentLength;
            segmentedPackage.BytesWritten += fragmentLength;

            if (segmentedPackage.BytesWritten >= segmentedPackage.TotalLength)
            {
                _pendingSegments.Remove(startSequenceNumber);
                HandleFinishedSegmentedPackage(segmentedPackage.TotalPayload);
            }
        }

        private SegmentedPackage GetSegmentedPackage(int startSequenceNumber, int totalLength)
        {
            if (_pendingSegments.TryGetValue(startSequenceNumber, out var segmentedPackage))
                return segmentedPackage;

            segmentedPackage = new SegmentedPackage
            {
                TotalLength = totalLength,
                TotalPayload = new byte[totalLength],
            };
            _pendingSegments.Add(startSequenceNumber, segmentedPackage);

            return segmentedPackage;
        }

        private static void ReadByte(out byte value, byte[] source, ref int offset)
        {
            value = source[offset++];
        }
    }
}
