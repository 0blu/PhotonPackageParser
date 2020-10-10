using SharpPcap;
using System;

namespace PhotonPackageParser.Example
{
    internal static class PacketDeviceSelector
    {
        public static ICaptureDevice AskForPacketDevice()
        {
            // Retrieve the device list from the local machine
            CaptureDeviceList devices = CaptureDeviceList.Instance;

            if (devices.Count == 0)
            {
                throw new Exception("No interfaces found! Make sure WinPcap is installed.");
            }

            // Print the list
            for (int i = 0; i != devices.Count; ++i)
            {
                ICaptureDevice device = devices[i];
                Console.Write((i + 1) + ". ");
                if (device.Description != null)
                    Console.WriteLine(" (" + device.Description + ")");
                else
                    Console.WriteLine(" (No description available)");
            }

            int deviceIndex;
            do
            {
                Console.WriteLine("Enter the interface number (1-" + devices.Count + "):");
                string deviceIndexString = Console.ReadLine();
                if (!int.TryParse(deviceIndexString, out deviceIndex) ||
                    deviceIndex < 1 || deviceIndex > devices.Count)
                {
                    deviceIndex = 0;
                }
            } while (deviceIndex == 0);

            return devices[deviceIndex - 1];
        }
    }
}
