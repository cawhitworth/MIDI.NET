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
            MockWin32MIDI win32Midi = new MockWin32MIDI();
            uint deviceID = 0;
            OutDevice dev = OutDevice.FromCaps(caps, win32Midi, deviceID);
            Assert.AreEqual(dev.DeviceName, deviceName);
        }

        #region Open Device Tests
        [Test]
        public void OpenOneDevice()
        {
            // Set up a framework with one device
            MockWin32MIDI win32Midi = new MockWin32MIDI();
            win32Midi.NumOutDevs = 1;

            // Construct it manually
            OutDevice dev = OutDevice.FromCaps(caps, win32Midi, 0);

            // And open it
            dev.Open();

            Assert.AreEqual(win32Midi.callsTo("midiOutOpen"), 1);
            Assert.AreEqual(dev.IsOpen, true);
        }

        [Test]
        public void OpenDeviceThatIsAlreadyOpen()
        {
            // Set up a framework with one device that is already open (perhaps by another application?)
            MockWin32MIDI win32Midi = new MockWin32MIDI();
            win32Midi.NumOutDevs = 1;
            win32Midi.OutDeviceOpen[0] = true;

            // Construct an OutDevice with the appropriate device ID
            OutDevice dev = OutDevice.FromCaps(caps, win32Midi, 0);

            // And try and open it
            MIDIException e = Assert.Throws<MIDIException>(delegate { dev.Open(); });

            Assert.AreEqual(e.ErrorCode, InvokeLayer.ErrorCode.MMSYSERR_ALLOCATED);
            Assert.AreEqual(win32Midi.callsTo("midiOutOpen"), 1);
        }

        [Test]
        public void OpenNonExistentDevice()
        {
            // Set up a framework with no devices
            MockWin32MIDI win32Midi = new MockWin32MIDI();
            win32Midi.NumOutDevs = 0;

            // Construct a device that doesn't exist
            // (this is pretty pathological, but it's possible someone could do it)
            OutDevice dev = OutDevice.FromCaps(caps, win32Midi, 0);

            // And try and open it
            MIDIException e = Assert.Throws<MIDIException>(delegate { dev.Open(); });

            Assert.AreEqual(e.ErrorCode, InvokeLayer.ErrorCode.MMSYSERR_BADDEVICEID);
            Assert.AreEqual(win32Midi.callsTo("midiOutOpen"), 1);
        }

        #endregion

        #region Close device tests
        [Test]
        public void CloseDeviceThatWeDidNotOpen()
        {
            MockWin32MIDI win32Midi = new MockWin32MIDI();
            win32Midi.NumOutDevs = 1;
            win32Midi.OutDeviceOpen[0] = true;

            OutDevice dev = OutDevice.FromCaps(caps, win32Midi, 0);

            MIDIException e = Assert.Throws<MIDIException>( delegate { dev.Close(); } );

            Assert.AreEqual(win32Midi.callsTo("midiOutClose"), 0);
            Assert.AreEqual(e.ErrorCode, ErrorCode.MDNERR_INVALIDDEVICE);
        }

        [Test]
        public void CloseDeviceThatWeOpened()
        {
            MockWin32MIDI win32Midi = new MockWin32MIDI();
            win32Midi.NumOutDevs = 1;
            OutDevice dev = OutDevice.FromCaps(caps, win32Midi, 0);
            dev.Open();

            Assert.DoesNotThrow(delegate { dev.Close(); });

            Assert.AreEqual(win32Midi.callsTo("midiOutClose"), 1);

            Assert.AreEqual(dev.IsOpen, false);
        }
        #endregion

        #region Short message tests
        [Test]
        public void SendMessage_Simple()
        {
            MockWin32MIDI win32Midi = new MockWin32MIDI();
            win32Midi.NumOutDevs = 1;
            OutDevice dev = OutDevice.FromCaps(caps, win32Midi, 0);
            dev.Open();

            uint shortMsg = (uint)0x00000080;

            dev.SendShortMsg(shortMsg);

            Assert.AreEqual(win32Midi.callsTo("midiOutShortMsg"), 1);
            Assert.AreEqual(win32Midi.LastSentShortMsg, shortMsg);
        }

        [Test]
        public void SendMessage_Invalid()
        {
            MockWin32MIDI win32Midi = new MockWin32MIDI();
            win32Midi.NumOutDevs = 1;
            OutDevice dev = OutDevice.FromCaps(caps, win32Midi, 0);
            dev.Open();

            uint shortMsg = (uint)0x00000000; // Status bytes have to have MSB set
            MIDIException e = Assert.Throws<MIDIException>(delegate { dev.SendShortMsg(shortMsg); });
            Assert.AreEqual(e.ErrorCode, ErrorCode.MDNERR_MSG_INVALIDSTATUS);
        
            shortMsg = (uint)0x00008080; // Data bytes must have MSB clear
            e = Assert.Throws<MIDIException>(delegate { dev.SendShortMsg(shortMsg); });
            Assert.AreEqual(e.ErrorCode, ErrorCode.MDNERR_MSG_INVALIDDATA);

            shortMsg = (uint)0x00800080; // Data bytes must have MSB clear
            e = Assert.Throws<MIDIException>(delegate { dev.SendShortMsg(shortMsg); });
            Assert.AreEqual(e.ErrorCode, ErrorCode.MDNERR_MSG_INVALIDDATA);
        }

        [Test]
        public void SendMessage_Errors()
        {
            MockWin32MIDI win32Midi = new MockWin32MIDI();
            win32Midi.NumOutDevs = 1;
            OutDevice dev = OutDevice.FromCaps(caps, win32Midi, 0);

            uint shortMsg = (uint)0x00000080; // Status bytes have to have MSB set
            MIDIException e = Assert.Throws<MIDIException>(delegate { dev.SendShortMsg(shortMsg); });
            Assert.AreEqual(e.ErrorCode, ErrorCode.MDNERR_INVALIDDEVICE);
        }

        #endregion
    }
}
