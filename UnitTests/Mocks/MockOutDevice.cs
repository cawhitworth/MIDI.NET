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
