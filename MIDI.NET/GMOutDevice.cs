using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MIDIDotNet
{
    class GMOutDevice : IOutDevice
    {
        IOutDevice outDevice;

        public GMOutDevice(IOutDevice outDevice)
        {
            this.outDevice = outDevice;
        }

        #region IOutDevice
        public string DeviceName { get { return outDevice.DeviceName; } }
        public bool IsOpen { get { return outDevice.IsOpen; } }
        public void Open() { outDevice.Open(); }
        public void Close() { outDevice.Close(); }
        public void SendShortMsg(uint msg) { outDevice.SendShortMsg(msg); }
        #endregion

        public void SendNoteOn(byte channel, byte note, byte velocity)
        {
            if (channel > 0x0f)
            {
                throw new MIDIException("Invalid channel", ErrorCode.MDNERR_INVALIDCHANNEL);
            }
            uint msg = ((uint)0x90 | channel) | ((uint)note << 8) | ((uint)velocity << 16);

            SendShortMsg(msg);
        }

    }
}
