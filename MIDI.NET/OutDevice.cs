using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MIDIDotNet
{
    public class OutDevice
    {
        private string deviceName;

        private OutDevice()
        {

        }

        public static OutDevice FromCaps(InvokeLayer.MIDIOUTCAPS caps)
        {
            OutDevice device = new OutDevice();

            device.deviceName = caps.szPName;

            return device;
        }

        public string DeviceName
        {
            get { return deviceName; }
        }
    }
}
