using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MIDIDotNet
{
    public class InDevice
    {
        private string deviceName;

        private InDevice()
        {

        }

        public static InDevice FromCaps(InvokeLayer.MIDIINCAPS caps)
        {
            InDevice device = new InDevice();
            device.deviceName = caps.szPName;
            return device;
        }

        public string DeviceName
        {
            get { return deviceName; }
        }
    }
}
