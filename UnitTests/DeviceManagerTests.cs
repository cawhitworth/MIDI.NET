using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using MIDIDotNet;

namespace UnitTests
{
    class DeviceManagerTests
    {
        public DeviceManagerTests()
        {

        }

        [Test]
        public void DefaultConstruction()
        {
            MockWin32MIDI win32Midi = new MockWin32MIDI();
            DeviceManager manager = new DeviceManager(win32Midi);

            Assert.AreEqual(manager.InDevices.Count, win32Midi.NumInDevs);
            Assert.AreEqual(manager.OutDevices.Count, win32Midi.NumOutDevs);

            Assert.AreEqual(win32Midi.callsTo("midiOutGetDevCaps"), win32Midi.NumOutDevs);
            Assert.AreEqual(win32Midi.callsTo("midiInGetDevCaps"), win32Midi.NumInDevs);
        }

        [Test]
        public void ConstructionWithDevices()
        {
            MockWin32MIDI win32Midi = new MockWin32MIDI();
            win32Midi.NumInDevs = 1;
            win32Midi.NumOutDevs = 1;

            DeviceManager manager = new DeviceManager(win32Midi);

            Assert.AreEqual(manager.InDevices.Count, win32Midi.NumInDevs);
            Assert.AreEqual(manager.OutDevices.Count, win32Midi.NumOutDevs);

            Assert.AreEqual(win32Midi.callsTo("midiOutGetDevCaps"), win32Midi.NumOutDevs);
            Assert.AreEqual(win32Midi.callsTo("midiInGetDevCaps"), win32Midi.NumInDevs);


        }
    }
}
