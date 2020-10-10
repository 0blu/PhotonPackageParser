using PacketDotNet;
using SharpPcap;

namespace PhotonPackageParser.Example
{
    class Program
    {
        private readonly PhotonParser photonParser;

        public Program()
        {
            photonParser = new ExampleParser();
        }

        public static void Main(string[] args)
        {
            new Program().Start();
        }

        private void Start()
        {
            ICaptureDevice device = PacketDeviceSelector.AskForPacketDevice();

            device.OnPacketArrival += new PacketArrivalEventHandler(PacketHandler);
            device.Open(DeviceMode.Promiscuous, 1000);
            device.StartCapture();
        }

        private void PacketHandler(object sender, CaptureEventArgs e)
        {
            UdpPacket packet = Packet.ParsePacket(e.Packet.LinkLayerType, e.Packet.Data).Extract<UdpPacket>();
            if (packet != null && (packet.SourcePort == 5056 || packet.DestinationPort == 5056))
            {
                photonParser.ReceivePacket(packet.PayloadData);
            }
        }
    }
}
