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
            InDevice dev = InDevice.FromCaps(caps, null, 0);
            Assert.AreEqual(dev.DeviceName, deviceName);
        }
    
        #region Open Device Tests
        [Test]
        public void OpenOneDevice()
        {
            // Set up a framework with one device
            MockWin32MIDI win32Midi = new MockWin32MIDI();
            win32Midi.NumInDevs = 1;

            // Construct it manually
            InDevice dev = InDevice.FromCaps(caps, win32Midi, 0);

            Assert.AreEqual(dev.IsOpen, false);

            // And open it
            Assert.DoesNotThrow(delegate { dev.Open(); });

            Assert.AreEqual(win32Midi.callsTo("midiInOpen"), 1);
            Assert.AreEqual(dev.IsOpen, true);
        }

        [Test]
        public void OpenNonExistentDevice()
        {
            // Set up a framework with one device
            MockWin32MIDI win32Midi = new MockWin32MIDI();
            win32Midi.NumInDevs = 0;

            // Construct it manually
            InDevice dev = InDevice.FromCaps(caps, win32Midi, 0);

            // And open it
            MIDIException e = Assert.Throws<MIDIException>(delegate { dev.Open(); });

            Assert.AreEqual(e.ErrorCode, InvokeLayer.ErrorCode.MMSYSERR_BADDEVICEID);
            Assert.AreEqual(win32Midi.callsTo("midiInOpen"), 1);
            Assert.AreEqual(dev.IsOpen, false);
        }

        [Test]
        public void OpenAlreadyOpenDevice()
        {
            // Set up a framework with one device
            MockWin32MIDI win32Midi = new MockWin32MIDI();
            win32Midi.NumInDevs = 1;
            win32Midi.InDeviceOpen[0] = true;

            // Construct it manually
            InDevice dev = InDevice.FromCaps(caps, win32Midi, 0);

            // And open it
            MIDIException e = Assert.Throws<MIDIException>(delegate { dev.Open(); });

            Assert.AreEqual(e.ErrorCode, InvokeLayer.ErrorCode.MMSYSERR_ALLOCATED);
            Assert.AreEqual(win32Midi.callsTo("midiInOpen"), 1);
            Assert.AreEqual(dev.IsOpen, false);
        }
        #endregion

        #region Close device tests
        [Test]
        public void CloseDeviceWeOpened()
        {

            // Set up a framework with one device
            MockWin32MIDI win32Midi = new MockWin32MIDI();
            win32Midi.NumInDevs = 1;

            InDevice dev = InDevice.FromCaps(caps, win32Midi, 0);
            dev.Open();

            Assert.DoesNotThrow(delegate { dev.Close(); });

            Assert.AreEqual(win32Midi.callsTo("midiInClose"), 1);
            Assert.AreEqual(dev.IsOpen, false);
        }

        [Test]
        public void CloseDeviceWeDidNotOpen()
        {
            // Set up a framework with one device
            MockWin32MIDI win32Midi = new MockWin32MIDI();
            win32Midi.NumInDevs = 1;

            InDevice dev = InDevice.FromCaps(caps, win32Midi, 0);
            
            MIDIException e = Assert.Throws<MIDIException>(delegate { dev.Close(); });

            Assert.AreEqual(e.ErrorCode, ErrorCode.MDNERR_INVALIDDEVICE);
            Assert.AreEqual(dev.IsOpen, false);
        }
        #endregion

        #region Start tests
        [Test]
        public void StartAnOpenDevice()
        {
            MockWin32MIDI win32MIDI = new MockWin32MIDI();
            win32MIDI.NumInDevs = 1;
            InDevice dev = InDevice.FromCaps(caps, win32MIDI, 0);
            dev.Open();

            Assert.AreEqual(dev.IsStarted, false);

            Assert.DoesNotThrow(delegate { dev.Start(); });

            Assert.AreEqual(win32MIDI.callsTo("midiInStart"), 1);

            Assert.AreEqual(dev.IsStarted, true);
        }

        [Test]
        public void StartAnUnopenedDevice()
        {
            MockWin32MIDI win32MIDI = new MockWin32MIDI();
            win32MIDI.NumInDevs = 1;
            InDevice dev = InDevice.FromCaps(caps, win32MIDI, 0);

            Assert.AreEqual(dev.IsStarted, false);

            MIDIException e = Assert.Throws<MIDIException>(delegate { dev.Start(); });

            Assert.AreEqual(e.ErrorCode, ErrorCode.MDNERR_INVALIDDEVICE);
        }

        [Test]
        public void StartADeviceNotOpenedByUs()
        {
            MockWin32MIDI win32MIDI = new MockWin32MIDI();
            win32MIDI.NumInDevs = 1;
            win32MIDI.InDeviceOpen[0] = true;
            InDevice dev = InDevice.FromCaps(caps, win32MIDI, 0);

            Assert.AreEqual(dev.IsStarted, false);

            MIDIException e = Assert.Throws<MIDIException>(delegate { dev.Start(); });

            Assert.AreEqual(e.ErrorCode, ErrorCode.MDNERR_INVALIDDEVICE);
        }

        #endregion
    }
}
