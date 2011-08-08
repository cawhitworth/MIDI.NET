using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MIDIDotNet;

namespace Launchpad
{

    public delegate void ButtonPressDelegate(bool pressed);
    public delegate void GridButtonPressDelegate(int x, int y, bool pressed);

    public class Controller : IDisposable
    {
        Win32MIDI win32Midi = null;
        DeviceManager deviceManager = null;
        GMInDevice inDevice = null;
        GMOutDevice outDevice = null;

        public Controller()
        {
            win32Midi = new Win32MIDI();
            deviceManager = new DeviceManager(win32Midi);

            foreach(InDevice inDevice in deviceManager.InDevices)
            {
                if (inDevice.DeviceName.Contains("Launchpad"))
                {
                    this.inDevice = new GMInDevice(inDevice);
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
            this.inDevice.HandleDataDelegate = new HandleDataDelegate(this.HandleData);
            this.inDevice.NoteOnDelegate = new NoteOnDelegate(this.NoteOn);
            this.inDevice.NoteOffDelegate = new NoteOffDelegate(this.NoteOff);
            this.inDevice.Start();
            this.outDevice.Open();
            Reset();
        }

        public GridButtonPressDelegate GridButtonPressDelegate { set; private get; }
        
        public ButtonPressDelegate UpButtonPressDelegate { set; private get; }
        public ButtonPressDelegate DownButtonPressDelegate { set; private get; }
        public ButtonPressDelegate LeftButtonPressDelegate { set; private get; }
        public ButtonPressDelegate RightButtonPressDelegate { set; private get; }

        public ButtonPressDelegate SessionButtonPressDelegate { set; private get; }
        public ButtonPressDelegate User1ButtonPressDelegate { set; private get; }
        public ButtonPressDelegate User2ButtonPressDelegate { set; private get; }
        public ButtonPressDelegate MixerButtonPressDelegate { set; private get; }

        public void Dispose()
        {
            inDevice.Stop();
            inDevice.Close();
            outDevice.Close();
        }

        public void HandleData(IntPtr dwParam1, IntPtr dwParam2)
        {
            uint param1 = (uint)dwParam1;
            uint param2 = (uint)dwParam2;
            byte status = (byte)(param1 & 0xff);
            byte data1 = (byte)((param1 >> 8) & 0xff);
            byte data2 = (byte)((param1 >> 16) & 0xff);

            if (status == 0xb0)
            {
                if (data1 == 0x68 && UpButtonPressDelegate != null)
                {
                    UpButtonPressDelegate(data2 == 0x7f);
                }
                if (data1 == 0x69 && DownButtonPressDelegate != null)
                {
                    DownButtonPressDelegate(data2 == 0x7f);
                }
                if (data1 == 0x6a && LeftButtonPressDelegate != null)
                {
                    LeftButtonPressDelegate(data2 == 0x7f);
                }
                if (data1 == 0x6b && RightButtonPressDelegate != null)
                {
                    RightButtonPressDelegate(data2 == 0x7f);
                }

                if (data1 == 0x6c && SessionButtonPressDelegate != null)
                {
                    SessionButtonPressDelegate(data2 == 0x7f);
                }
                if (data1 == 0x6d && User1ButtonPressDelegate != null)
                {
                    User1ButtonPressDelegate(data2 == 0x7f);
                }
                if (data1 == 0x6e && User2ButtonPressDelegate != null)
                {
                    User2ButtonPressDelegate(data2 == 0x7f);
                }
                if (data1 == 0x6f && MixerButtonPressDelegate != null)
                {
                    MixerButtonPressDelegate(data2 == 0x7f);
                }
            }
        }

        public void NoteOn(byte note, byte velocity)
        {
            if (GridButtonPressDelegate != null)
            {
                int x = note & 0x0f;
                if (x > 8) x = 8;
                int y = (note & 0xf0) >> 4;
                if (y > 8) y = 8;
                GridButtonPressDelegate(x, y, velocity == 0x7f);
            }
        }

        public void NoteOff(byte note, byte velocity)
        {

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

        public void Set(int x, int y, Color color)
        {
            Set(x, y, color.Byte);
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
