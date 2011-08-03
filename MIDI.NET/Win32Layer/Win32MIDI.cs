using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MIDIDotNet
{
    /// <summary>
    /// This is the thinnest of thin layers around the InvokeLayer
    /// 
    /// It is provided as an instanceable class based off an interface so it can be mocked out for testing purposes
    /// 
    /// You probably don't really want to use this directly.
    /// </summary>
    public class Win32MIDI : IWin32MIDI
    {
        public Win32MIDI()
        {

        }

        public uint midiOutGetNumDevs()
        {
            return InvokeLayer.midiOutGetNumDevs();
        }

        public uint midiOutGetDevCaps(IntPtr uDeviceID, out InvokeLayer.MIDIOUTCAPS lpMidiOutCaps)
        {
            lpMidiOutCaps = new InvokeLayer.MIDIOUTCAPS();
            return InvokeLayer.midiOutGetDevCaps(uDeviceID, lpMidiOutCaps, InvokeLayer.MIDIOUTCAPS.CBMIDIOUTCAPS);
        }

        public uint midiOutOpen(ref IntPtr lphmo, uint uDeviceID, InvokeLayer.MidiOutProc dwCallback, IntPtr dwCallbackInstance, uint dwFlags)
        {
            return InvokeLayer.midiOutOpen(ref lphmo, uDeviceID, dwCallback, dwCallbackInstance, dwFlags);
        }

        public uint midiOutClose(IntPtr hmo)
        {
            return InvokeLayer.midiOutClose(hmo);
        }

        public uint midiOutMessage(IntPtr deviceID, uint msg, IntPtr dw1, IntPtr dw2)
        {
            return InvokeLayer.midiOutMessage(deviceID, msg, dw1, dw2);
        }

        public uint midiOutShortMsg(IntPtr deviceID, uint dwMsg)
        {
            return InvokeLayer.midiOutShortMsg(deviceID, dwMsg);
        }

        public uint midiInGetNumDevs()
        {
            return InvokeLayer.midiInGetNumDevs();
        }

        public uint midiInGetDevCaps(IntPtr uDeviceID, out InvokeLayer.MIDIINCAPS lpMidiInCaps)
        {
            lpMidiInCaps = new InvokeLayer.MIDIINCAPS();
            return InvokeLayer.midiInGetDevCaps(uDeviceID, lpMidiInCaps, InvokeLayer.MIDIINCAPS.CBMIDIINCAPS);
        }

        public uint midiInOpen(ref IntPtr lphMidiIn, uint uDeviceID, InvokeLayer.MidiInProc dwCallback, IntPtr dwCallbackInstance, uint dwFlags)
        {
            return InvokeLayer.midiInOpen(ref lphMidiIn, uDeviceID, dwCallback, dwCallbackInstance, dwFlags);
        }

        public uint midiInClose(IntPtr hMidiIn)
        {
            return InvokeLayer.midiInClose(hMidiIn);
        }

        public uint midiInStart(IntPtr hMidiIn)
        {
            return InvokeLayer.midiInStart(hMidiIn);
        }

        public uint midiInStop(IntPtr hMidiIn)
        {
            return InvokeLayer.midiInStop(hMidiIn);
        }
    }
}
