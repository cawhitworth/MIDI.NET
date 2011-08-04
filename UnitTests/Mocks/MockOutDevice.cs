using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MIDIDotNet;

namespace UnitTests
{
    internal class MockOutDevice : MockBase, IOutDevice
    {
        bool open = false;
        uint lastShortMsg;
        bool[] channels = new bool[16];

        public MockOutDevice()
        {
            for (int i = 0; i < 16; i++)
            {
                channels[i] = true;
            }
        }

        public uint LastShortMsg
        {
            get
            {
                return lastShortMsg;
            }
        }

        public string DeviceName
        {
            get
            {
                noteCall("DeviceName.get");
                return "MockOutDevice";
            }
        }

        public bool IsOpen
        {
            get
            {
                noteCall("IsOpen.get");
                return open;
            }
        }

        public uint Voices
        {
            get { return 128; }
        }

        public uint Polyphony
        {
            get { return 16; }
        }

        public bool[] Channel
        {
            get { return channels; }
        }

        public uint DeviceType
        {
            get { return 2; }
        }

        public void Open()
        {
            noteCall("Open");
            open = true;
        }

        public void Close()
        {
            noteCall("Close");
            open = false;
        }

        public void SendShortMsg(uint msg)
        {
            noteCall("SendShortMsg");
            lastShortMsg = msg;
        }
    }
}
