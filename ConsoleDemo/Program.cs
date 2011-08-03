using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MIDIDotNet;

namespace ConsoleDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            IWin32MIDI win32MIDI = new Win32MIDI();
            DeviceManager devManager = new DeviceManager(win32MIDI);

            Console.WriteLine("In Devices:");

            foreach (InDevice dev in devManager.InDevices)
            {
                Console.WriteLine(" - {0}", dev.DeviceName);
            }

            Console.WriteLine();

            Console.WriteLine("Out Devices:");

            foreach (OutDevice dev in devManager.OutDevices)
            {
                Console.WriteLine(" - {0}", dev.DeviceName);
            }

            devManager.OutDevices[0].Open();
            try
            {
                devManager.OutDevices[0].Open();
            }
            catch (MIDIException e)
            {
                Console.WriteLine("Failed to open device twice, as expected: {0} (code {1}", e.Details, e.ErrorCode);
            }

        }
    }
}
