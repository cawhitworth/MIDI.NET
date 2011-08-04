using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MIDIDotNet;

namespace Launchpad
{
    public class Controller : IDisposable
    {
        Win32MIDI win32Midi = null;
        DeviceManager deviceManager = null;
        InDevice inDevice = null;
        GMOutDevice outDevice = null;

        public Controller()
        {
            win32Midi = new Win32MIDI();
            deviceManager = new DeviceManager(win32Midi);

            foreach(InDevice inDevice in deviceManager.InDevices)
            {
                if (inDevice.DeviceName.Contains("Launchpad"))
                {
                    this.inDevice = inDevice;
                    break;
                }
            }

            foreach (OutDevice outDevice in deviceManager.OutDevices)
            {
                if (outDevice.DeviceName.Contains("Launchpad"))
                {
                    this.outDevice = new GMOutDevice(outDevice);
                    break;
                }
            }

            this.inDevice.Open();
            this.inDevice.Start();
            this.outDevice.Open();
            Reset();
        }

        public void Dispose()
        {
            inDevice.Stop();
            inDevice.Close();
            outDevice.Close();
        }

        public override String ToString()
        {
            StringBuilder s = new StringBuilder();
            s.Append(String.Format("InDevice: {0}\n", inDevice.DeviceName));
            s.Append(String.Format("OutDevice: {0}\n", outDevice.DeviceName));
            return s.ToString();
        }

        public void Reset()
        {
            outDevice.SendShortMsg(0x000000b0);
        }

        public byte Colour(int Green, int Red)
        {
            return Colour(Green, Red, false, false);
        }

        public byte Colour(int Green, int Red, bool Copy, bool Clear)
        {
            if (Green < 0) Green = 0;
            if (Green > 3) Green = 3;
            if (Red < 0) Red =0;
            if (Red > 3) Red = 3;
            return (byte)(Red | (Copy ? 1 << 2 : 0) | (Clear ? 1 << 3 : 0) | (Green << 4));
        }

        public void Set(int x, int y, byte colour)
        {
            x = x & 0xf;
            y = y & 0x7;
            outDevice.SendNoteOn( 0, (byte)(x + (y * 16)), colour);
        }

        public void Clear(int x, int y)
        {
            x = x & 0xf;
            y = y & 0x7;
            outDevice.SendNoteOff( 0, (byte)(x + (y*16)), 0);
        }
    }
}
