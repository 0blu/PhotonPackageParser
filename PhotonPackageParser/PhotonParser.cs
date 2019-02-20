using Protocol16;
using System;
using System.Collections.Generic;
using System.IO;

namespace PhotonPackageParser
{
    public class PhotonParser
    {
        private const int CommandHeaderLength = 12;
        private const int PhotonHeaderLength = 12;

        private readonly IPhotonPackageHandler handler;
        private readonly Dictionary<int, SegmentedPackage> pendingSegments;

        public PhotonParser(IPhotonPackageHandler handler)
        {
            this.handler = handler;
            pendingSegments = new Dictionary<int, SegmentedPackage>();
        }

        public void DeserializeMessageAndCallback(byte[] payload)
        {
            if (payload.Length < PhotonHeaderLength)
            {
                return;
            }

            int offset = 0;
            Deserializer.Deserialize(out short peerId, payload, ref offset);
            ReadByte(out byte flags, payload, ref offset);
            ReadByte(out byte commandCount, payload, ref offset);
            Deserializer.Deserialize(out int timestamp, payload, ref offset);
            Deserializer.Deserialize(out int challenge, payload, ref offset);

            bool isEncrypted = flags == 1;
            bool isCrcEnabled = flags == 0xCC;

            if (isEncrypted)
            {
                // Encrypted packages are not supported
                return;
            }

            if (isCrcEnabled)
            {
                int ignoredOffset = 0;
                Deserializer.Deserialize(out int crc, payload, ref ignoredOffset);
                Serializer.Serialize(0, payload, ref offset);

                if (crc != CrcCalculator.Calculate(payload, payload.Length))
                {
                    // Invalid crc
                    return;
                }
            }

            for (int commandIdx = 0; commandIdx < commandCount; commandIdx++)
            {
                HandleCommand(payload, ref offset);
            }
        }

        private void HandleCommand(byte[] source, ref int offset)
        {
            ReadByte(out byte commandType, source, ref offset);
            ReadByte(out byte channelId, source, ref offset);
            ReadByte(out byte commandFlags, source, ref offset);
            // Skip 1 byte
            offset++;
            Deserializer.Deserialize(out int commandLength, source, ref offset);
            Deserializer.Deserialize(out int sequenceNumber, source, ref offset);
            commandLength -= CommandHeaderLength;

            switch ((CommandType)commandType)
            {
                case CommandType.Disconnect:
                    {
                        return;
                    }
                case CommandType.SendUnreliable:
                    {
                        offset += 4;
                        commandLength -= 4;
                        goto case CommandType.SendReliable;
                    }
                case CommandType.SendReliable:
                    {
                        HandleSendReliable(source, ref offset, ref commandLength);
                        break;
                    }
                case CommandType.SendFragment:
                    {
                        HandleSendFragment(source, ref offset, ref commandLength);
                        break;
                    }
                default:
                    {
                        offset += commandLength;
                        break;
                    }
            }
        }

        private void HandleSendReliable(byte[] source, ref int offset, ref int commandLength)
        {
            // Skip 1 byte
            offset++;
            commandLength--;
            ReadByte(out byte messageType, source, ref offset);
            commandLength--;

            int operationLength = commandLength;
            var payload = new Protocol16Stream(operationLength);
            payload.Write(source, offset, operationLength);
            payload.Seek(0L, SeekOrigin.Begin);

            offset += operationLength;
            switch ((MessageType)messageType)
            {
                case MessageType.OperationRequest:
                    {
                        OperationRequest requestData = Protocol16Deserializer.DeserializeOperationRequest(payload);
                        handler.OnRequest(requestData.OperationCode, requestData.Parameters);
                        break;
                    }
                case MessageType.OperationResponse:
                    {
                        OperationResponse responseData = Protocol16Deserializer.DeserializeOperationResponse(payload);
                        handler.OnResponse(responseData.OperationCode, responseData.ReturnCode, responseData.Parameters);
                        break;
                    }
                case MessageType.Event:
                    {
                        EventData eventData = Protocol16Deserializer.DeserializeEventData(payload);
                        handler.OnEvent(eventData.Code, eventData.Parameters);
                        break;
                    }
            }
        }

        private void HandleSendFragment(byte[] source, ref int offset, ref int commandLength)
        {
            Deserializer.Deserialize(out int startSequenceNumber, source, ref offset);
            commandLength -= 4;
            Deserializer.Deserialize(out int fragmentCount, source, ref offset);
            commandLength -= 4;
            Deserializer.Deserialize(out int fragmentNumber, source, ref offset);
            commandLength -= 4;
            Deserializer.Deserialize(out int totalLength, source, ref offset);
            commandLength -= 4;
            Deserializer.Deserialize(out int fragmentOffset, source, ref offset);
            commandLength -= 4;

            int fragmentLength = commandLength;
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
            SegmentedPackage segmentedPackage = GetSegmentedPackage(startSequenceNumber, totalLength);

            Buffer.BlockCopy(source, offset, segmentedPackage.TotalPayload, fragmentOffset, fragmentLength);
            offset += fragmentLength;
            segmentedPackage.BytesWritten += fragmentLength;

            if (segmentedPackage.BytesWritten >= segmentedPackage.TotalLength)
            {
                pendingSegments.Remove(startSequenceNumber);
                HandleFinishedSegmentedPackage(segmentedPackage.TotalPayload);
            }
        }

        private SegmentedPackage GetSegmentedPackage(int startSequenceNumber, int totalLength)
        {
            if (pendingSegments.TryGetValue(startSequenceNumber, out SegmentedPackage segmentedPackage))
            {
                return segmentedPackage;
            }

            segmentedPackage = new SegmentedPackage
            {
                TotalLength = totalLength,
                TotalPayload = new byte[totalLength],
            };
            pendingSegments.Add(startSequenceNumber, segmentedPackage);

            return segmentedPackage;
        }

        private static void ReadByte(out byte value, byte[] source, ref int offset)
        {
            value = source[offset++];
        }
    }
}
