using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MIDIDotNet
{
    public delegate void HandleData(IntPtr dwParam1, IntPtr dwParam2);

    interface IInDevice
    {
        string DeviceName { get; }
        bool IsOpen { get; }
        HandleData HandleDataDelegate { set; }
        void Open();
        void Close();
    }
}
