using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MIDIDotNet
{
    public class GMOutDevice : IOutDevice, IDisposable
    {
        IOutDevice outDevice;

        public GMOutDevice(IOutDevice outDevice)
        {
            this.outDevice = outDevice;
        }

        #region IDisposable
        public void Dispose()
        {

        }
        #endregion

        #region IOutDevice
        public string DeviceName { get { return outDevice.DeviceName; } }
        public bool IsOpen { get { return outDevice.IsOpen; } }
        public uint Polyphony { get { return outDevice.Polyphony; } }
        public uint Voices { get { return outDevice.Voices; } }
        public bool[] Channel { get { return outDevice.Channel; } }
        public uint DeviceType { get { return outDevice.DeviceType; } }
        public void Open() { outDevice.Open(); }
        public void Close() { outDevice.Close(); }
        public void SendShortMsg(uint msg) { outDevice.SendShortMsg(msg); }
        #endregion

        public void SendNoteOn(byte channel, byte note, byte velocity)
        {
            if (channel > 0x0f)
            {
                throw new MIDIException("Invalid channel", ErrorCode.MDNERR_GM_INVALIDCHANNEL);
            }
            
            if (note > 0x7f)
            {
                throw new MIDIException("Invalid note", ErrorCode.MDNERR_GM_INVALIDNOTE);
            }

            if (velocity > 0x7f)
            {
                throw new MIDIException("Invalid velocity", ErrorCode.MDNERR_GM_INVALIDVELOCITY);
            }

            uint msg = ((uint)0x90 | channel) | ((uint)note << 8) | ((uint)velocity << 16);

            SendShortMsg(msg);
        }

        public void SendNoteOff(byte channel, byte note, byte velocity)
        {
            if (channel > 0x0f)
            {
                throw new MIDIException("Invalid channel", ErrorCode.MDNERR_GM_INVALIDCHANNEL);
            }
            
            if (note > 0x7f)
            {
                throw new MIDIException("Invalid note", ErrorCode.MDNERR_GM_INVALIDNOTE);
            }

            if (velocity > 0x7f)
            {
                throw new MIDIException("Invalid velocity", ErrorCode.MDNERR_GM_INVALIDVELOCITY);
            }

            uint msg = ((uint)0x80 | channel) | ((uint)note << 8) | ((uint)velocity << 16);

            SendShortMsg(msg);
        }

    }
}
