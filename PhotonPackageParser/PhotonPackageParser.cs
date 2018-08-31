using ExitGames.Client.Photon;
using PcapDotNet.Packets.Transport;
using System.IO;
using System.Linq;

namespace PhotonPackageParser
{
    public static class PhotonPackageParser
    {
        private static readonly Protocol16 Protocol16 = new Protocol16();

        private const int CommandHeaderLength = 12;

        public static void ParsePhotonPackage(TransportDatagram datagram, IPhotonPackageHandler handler)
        {
            if (datagram.Payload.Length < CommandHeaderLength)
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
                HandleCommand(source, ref offset, handler);
            }
        }

        private static void HandleCommand(byte[] source, ref int offset, IPhotonPackageHandler handler)
        {
            ReadByte(out byte commandType, source, ref offset);
            ReadByte(out byte channelId, source, ref offset);
            ReadByte(out byte commandFlags, source, ref offset);
            offset++; // Skip 1 byte
            Protocol.Deserialize(out int commandLength, source, ref offset);
            Protocol.Deserialize(out int sequenceNumber, source, ref offset);

            switch (commandType)
            {
                case 4:// Disconnect
                    break;
                case 7:// Send unreliable
                    offset += 4;
                    commandLength -= 4;
                    goto case 6;
                case 6:// Send reliable
                    offset++;// Skip 1 byte
                    ReadByte(out byte messageType, source, ref offset);

                    int operationLength = commandLength - CommandHeaderLength - 2;
                    var payload = new StreamBuffer(operationLength);
                    payload.Write(source, offset, operationLength);
                    payload.Seek(0L, SeekOrigin.Begin);

                    offset += operationLength;
                    switch (messageType)
                    {
                        case 2:// Operation Request
                            var requestData = Protocol16.DeserializeOperationRequest(payload);
                            handler.OnRequest(requestData.OperationCode, requestData.Parameters);
                            break;
                        case 3:// Operation Response
                            var responseData = Protocol16.DeserializeOperationResponse(payload);
                            handler.OnResponse(responseData.OperationCode, responseData.ReturnCode, responseData.Parameters);
                            break;
                        case 4:// Event
                            var eventData = Protocol16.DeserializeEventData(payload);
                            handler.OnEvent(eventData.Code, eventData.Parameters);
                            break;
                    }
                    break;
                default:
                    offset += commandLength - CommandHeaderLength;
                    break;
            }
        }

        private static void ReadByte(out byte value, byte[] source, ref int offset)
        {
            value = source[offset++];
        }
    }
}
