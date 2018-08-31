using PcapDotNet.Core;
using PcapDotNet.Packets;
using PcapDotNet.Packets.IpV4;
using PcapDotNet.Packets.Transport;

namespace PhotonPackageParser.Example
{
    class Program
    {
        private IPhotonPackageHandler _photonHandler;

        public static void Main(string[] args)
        {
            new Program().Start();
        }

        private void Start()
        {
            var device = PacketDeviceSelector.AskForPacketDevice();
            _photonHandler = new ExamplePackageHandler();

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

            PhotonPackageParser.ParsePhotonPackage(udp, _photonHandler);
        }
    }
}
