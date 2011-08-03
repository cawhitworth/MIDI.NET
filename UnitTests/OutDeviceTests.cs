using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using MIDIDotNet;

namespace UnitTests
{
    class OutDeviceTests
    {
        string deviceName = "OutDeviceTests";

        InvokeLayer.MIDIOUTCAPS caps = new InvokeLayer.MIDIOUTCAPS();

        public OutDeviceTests()
        {
            caps.szPName = deviceName;
        }

        [Test]
        public void Name()
        {
            OutDevice dev = OutDevice.FromCaps(caps);
            Assert.AreEqual(dev.DeviceName, deviceName);
        }
    }
}
