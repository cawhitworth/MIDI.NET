using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MIDIDotNet
{
    public interface IOutDevice
    {
        string DeviceName { get; }
        bool IsOpen { get; }
        void Open();
        void Close();
        void SendShortMsg(uint msg);
    }
}
