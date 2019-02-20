using PcapDotNet.Core;
using PcapDotNet.Packets;
using PcapDotNet.Packets.IpV4;
using PcapDotNet.Packets.Transport;
using System.Linq;

namespace PhotonPackageParser.Example
{
    class Program
    {
        public static void Main(string[] args)
        {
            new Program().Start();
        }

        private readonly PhotonParser photonParser;

        public Program()
        {
            photonParser = new ExampleParser();
        }

        private void Start()
        {
            var device = PacketDeviceSelector.AskForPacketDevice();
            // var device = new OfflinePacketDevice("dump.pcap"); // Your wireshark dump (IT MUST BE *.pcap)

            using (PacketCommunicator communicator = device.Open(65536, PacketDeviceOpenAttributes.Promiscuous, 1000))
            {
                communicator.ReceivePackets(0, PacketHandler);
            }
        }

        private void PacketHandler(Packet packet)
        {
            IpV4Datagram ip = packet.Ethernet.IpV4;
            UdpDatagram udp = ip.Udp;

            if (udp == null || (udp.SourcePort != 5056 && udp.DestinationPort != 5056))
            {
                return;
            }

            photonParser.ReceivePacket(udp.Payload.ToArray());
        }
    }
}
