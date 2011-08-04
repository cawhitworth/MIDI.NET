using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

            foreach (OutDevice outDevice in devManager.OutDevices)
            {
                using (GMOutDevice gmOutDevice = new GMOutDevice(outDevice))
                {
                    Console.WriteLine("Device: {0}", gmOutDevice.DeviceName);
                    Console.WriteLine("{0} voices, {1} polyphony", gmOutDevice.Voices, gmOutDevice.Polyphony);
                    StringBuilder s = new StringBuilder("Channels: ");
                    for (int channel = 0; channel < 16; channel++)
                    {
                        s.Append(gmOutDevice.Channel[channel] ? "X" : "-");
                    }
                    Console.WriteLine(s.ToString());

                }
                Console.WriteLine();

            }

            /*
            gmOutDevice.Open();
            for (int rep = 0; rep < 10; rep++)
            {
                gmOutDevice.SendNoteOn(0, 50, 127);
                Thread.Sleep(500);
                gmOutDevice.SendNoteOff(0, 50, 0);
                Thread.Sleep(500);
            }
            gmOutDevice.Close();*/
        }
    }
}
