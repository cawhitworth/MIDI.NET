using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MIDIDotNet
{
    public delegate void HandleDataDelegate(IntPtr dwParam1, IntPtr dwParam2);

    public interface IInDevice
    {
        string DeviceName { get; }
        bool IsOpen { get; }
        bool IsStarted { get; }
        HandleDataDelegate HandleDataDelegate { set; }

        void Open();
        void Close();

        void Start();
        void Stop();
    }
}
