using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MIDIDotNet
{
    public delegate void NoteOnDelegate(byte note, byte velocity);
    public delegate void NoteOffDelegate(byte note, byte velocity);

    public class GMInDevice : IInDevice, IDisposable
    {
        IInDevice inDevice;
        NoteOnDelegate noteOnDelegate = null;
        NoteOffDelegate noteOffDelegate = null;
        HandleDataDelegate otherHandleDataDelegate = null;

        public GMInDevice(IInDevice inDevice)
        {
            this.inDevice = inDevice;
            inDevice.HandleDataDelegate = new HandleDataDelegate(this.HandleData);
        }

        #region IDisposable
        public void Dispose()
        {

        }
        #endregion

        #region IInDevice

        public string DeviceName { get { return inDevice.DeviceName;} }
        public bool IsOpen { get { return inDevice.IsOpen; } }
        public bool IsStarted { get { return inDevice.IsStarted; } }
        public HandleDataDelegate HandleDataDelegate { set { otherHandleDataDelegate = value; } }

        public void Open() { inDevice.Open(); }
        public void Close() { inDevice.Close(); }

        public void Start() { inDevice.Start(); }
        public void Stop() { inDevice.Stop(); }
        #endregion

        public NoteOnDelegate NoteOnDelegate
        {
            set { noteOnDelegate = value; }
        }

        public NoteOffDelegate NoteOffDelegate
        {
            set { noteOffDelegate = value; }
        }

        public void HandleData(IntPtr dwParam1, IntPtr dwParam2)
        {
            uint param1 = (uint)dwParam1;
            uint param2 = (uint)dwParam2;
            byte status = (byte)(param1 & 0xff);
            byte data1 = (byte)((param1 >> 8) & 0xff);
            byte data2 = (byte)((param1 >> 16) & 0xff);

            if (status == 0x90 && noteOnDelegate != null)
            {
                noteOnDelegate(data1, data2);
                return;
            }
            if (status == 0x80 && noteOffDelegate != null)
            {
                noteOffDelegate(data1, data2);
                return;
            }
            if (otherHandleDataDelegate != null)
            {
                otherHandleDataDelegate(dwParam1, dwParam2);
            }

        }
    }
}
