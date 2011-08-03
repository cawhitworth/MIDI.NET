using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MIDIDotNet;

namespace UnitTests
{
    internal class MockWin32MIDI : MockBase, MIDIDotNet.IWin32MIDI
    {
        uint lastSentMsg;
        IntPtr lastSentData1;
        IntPtr lastSentData2;
        public uint LastSentMsg { get { return lastSentMsg; } }
        public IntPtr LastSentData1 { get { return lastSentData1; } }
        public IntPtr LastSentData2 { get { return lastSentData2; } }

        uint lastSentShortMsg;
        public uint LastSentShortMsg { get { return lastSentShortMsg; } }
        public uint LastSentShortMsg_Status { get { return lastSentShortMsg & 0xff; } }
        public uint LastSentShortMsg_Data1 { get { return (lastSentShortMsg >> 8) & 0xff; } }
        public uint LastSentShortMsg_Data2 { get { return (lastSentShortMsg >> 16) & 0xff; } }

        public uint NumOutDevs { get; set; }
        public uint NumInDevs { get; set; }

        public MockWin32MIDI()
        {
            NumInDevs = 0;
            NumOutDevs = 0;
        }

        public uint midiOutGetNumDevs()
        {
            noteCall("midiOutGetNumDevs");
            return NumOutDevs;
        }

        public uint midiOutGetDevCaps(IntPtr uDeviceID, out InvokeLayer.MIDIOUTCAPS lpMidiOutCaps)
        {
            noteCall("midiOutGetDevCaps");
            lpMidiOutCaps = new InvokeLayer.MIDIOUTCAPS();
            lpMidiOutCaps.szPName = "MockWin32MIDI";

            return 0;
        }

        public uint midiOutOpen(ref IntPtr lphmo, uint uDeviceID, InvokeLayer.MidiOutProc dwCallback, IntPtr dwCallbackInstance, uint dwFlags)
        {
            noteCall("midiOutOpen");
            return 0;
        }

        public uint midiOutClose(IntPtr hmo)
        {
            noteCall("midiOutClose");
            return 0;
        }

        public uint midiOutMessage(IntPtr deviceID, uint msg, IntPtr dw1, IntPtr dw2)
        {
            noteCall("midiOutMessage");
            lastSentMsg = msg;
            lastSentData1 = dw1;
            lastSentData2 = dw2;
            return 0;
        }

        public uint midiOutShortMsg(IntPtr deviceID, uint dwMsg)
        {
            noteCall("midiOutShortMsg");
            lastSentShortMsg = dwMsg;
            return 0;
        }

        public uint midiInGetNumDevs()
        {
            noteCall("midiInGetNumDevs");
            return NumInDevs;
        }

        public uint midiInGetDevCaps(IntPtr uDeviceID, out InvokeLayer.MIDIINCAPS lpMidiInCaps)
        {
            noteCall("midiInGetDevCaps");
            lpMidiInCaps = new InvokeLayer.MIDIINCAPS();
            lpMidiInCaps.szPName = "MockWin32MIDI";
            return 0;
        }

        public uint midiInOpen(ref IntPtr lphMidiIn, uint uDeviceID, InvokeLayer.MidiInProc dwCallback, IntPtr dwCallbackInstance, uint dwFlags)
        {
            noteCall("midiInOpen");
            return 0;
        }

        public uint midiInClose(IntPtr hMidiIn)
        {
            noteCall("midiInClose");
            return 0;
        }

        public uint midiInStart(IntPtr hMidiIn)
        {
            noteCall("midiInStart");
            return 0;
        }

        public uint midiInStop(IntPtr hMidiIn)
        {
            noteCall("midiInStop");
            return 0;
        }
    }
}
