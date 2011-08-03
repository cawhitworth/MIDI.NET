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
        public void OpenNonExistentDevice()
        {
            // Set up a framework with no devices
            MockWin32MIDI win32Midi = new MockWin32MIDI();
            win32Midi.NumOutDevs = 0;

            // Construct a device that doesn't exist
            // (this is pretty pathological, but it's possible someone could do it)
            OutDevice dev = OutDevice.FromCaps(caps, win32Midi, 0);

            // And try and open it
            MIDIException e = Assert.Throws<MIDIException>( delegate { dev.Open(); } );

            Assert.AreEqual(e.ErrorCode, InvokeLayer.ErrorCodes.MMSYSERR_BADDEVICEID);
            Assert.AreEqual(win32Midi.callsTo("midiOutOpen"), 1);
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

            Assert.AreEqual(e.ErrorCode, InvokeLayer.ErrorCodes.MMSYSERR_ALLOCATED);
            Assert.AreEqual(win32Midi.callsTo("midiOutOpen"), 1);
        }
    }
}
