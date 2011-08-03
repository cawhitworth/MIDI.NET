using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using MIDIDotNet;

namespace UnitTests
{
    class InDeviceTests
    {
        string deviceName = "InDeviceTests";

        InvokeLayer.MIDIINCAPS caps = new InvokeLayer.MIDIINCAPS();

        public InDeviceTests()
        {
            caps.szPName = deviceName;
        }

        [Test]
        public void Name()
        {
            InDevice dev = InDevice.FromCaps(caps);
            Assert.AreEqual(dev.DeviceName, deviceName);
        }
    }
}
