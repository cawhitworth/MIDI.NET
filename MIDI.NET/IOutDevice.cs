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
        uint Polyphony { get; }
        uint Voices { get; }
        bool[] Channel { get; }
        uint DeviceType { get; }
        void Open();
        void Close();
        void SendShortMsg(uint msg);
    }
}
