using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MIDIDotNet;
using NUnit.Framework;

namespace UnitTests
{
    class GMOutDeviceTests
    {
        [Test]
        public void SendNoteOn_Simple()
        {
            MockOutDevice mockOutDevice = new MockOutDevice();
            GMOutDevice gmOutDevice = new GMOutDevice(mockOutDevice);

            gmOutDevice.SendNoteOn(0, 0, 0);

            Assert.AreEqual(mockOutDevice.callsTo("SendShortMsg"), 1);
            Assert.AreEqual(mockOutDevice.LastShortMsg, 0x00000090);

            mockOutDevice.clearCalls();

            gmOutDevice.SendNoteOn(10, 50, 127);

            Assert.AreEqual(mockOutDevice.callsTo("SendShortMsg"), 1);
            Assert.AreEqual(mockOutDevice.LastShortMsg, 0x007f329a);
        }

        [Test]
        public void SendNoteOn_Invalid()
        {
            MockOutDevice mockOutDevice = new MockOutDevice();
            GMOutDevice gmOutDevice = new GMOutDevice(mockOutDevice);

            MIDIException e = Assert.Throws<MIDIException>(delegate { gmOutDevice.SendNoteOn(17, 0, 0); });
            Assert.AreEqual(e.ErrorCode, ErrorCode.MDNERR_GM_INVALIDCHANNEL);

            e = Assert.Throws<MIDIException>(delegate { gmOutDevice.SendNoteOn(0, 200, 0); });
            Assert.AreEqual(e.ErrorCode, ErrorCode.MDNERR_GM_INVALIDNOTE);

            e = Assert.Throws<MIDIException>(delegate { gmOutDevice.SendNoteOn(0, 0, 128); });
            Assert.AreEqual(e.ErrorCode, ErrorCode.MDNERR_GM_INVALIDVELOCITY);
        }
    
        [Test]
        public void SendNoteOff_Simple()
        {
            MockOutDevice mockOutDevice = new MockOutDevice();
            GMOutDevice gmOutDevice = new GMOutDevice(mockOutDevice);

            gmOutDevice.SendNoteOff(0, 0, 0);

            Assert.AreEqual(mockOutDevice.callsTo("SendShortMsg"), 1);
            Assert.AreEqual(mockOutDevice.LastShortMsg, 0x00000080);

            mockOutDevice.clearCalls();

            gmOutDevice.SendNoteOff(10, 50, 127);

            Assert.AreEqual(mockOutDevice.callsTo("SendShortMsg"), 1);
            Assert.AreEqual(mockOutDevice.LastShortMsg, 0x007f328a);
        }
    }
}
