using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MIDIDotNet
{
    public interface IWin32MIDI
    {
        uint midiOutGetNumDevs();
        uint midiOutGetDevCaps(IntPtr uDeviceID, out InvokeLayer.MIDIOUTCAPS lpMidiOutCaps);
        
        uint midiOutOpen(ref IntPtr lphmo, uint uDeviceID, InvokeLayer.MidiOutProc dwCallback, IntPtr dwCallbackInstance, uint dwFlags);
        uint midiOutClose(IntPtr hmo);
        
        uint midiOutMessage( IntPtr deviceID, uint msg, IntPtr dw1, IntPtr dw2); 
        uint midiOutShortMsg( IntPtr deviceID, uint dwMsg); 

        uint midiInGetNumDevs();
        uint midiInGetDevCaps(IntPtr uDeviceID, out InvokeLayer.MIDIINCAPS lpMidiInCaps);

        uint midiInOpen( ref IntPtr lphMidiIn, uint uDeviceID, InvokeLayer.MidiInProc dwCallback, IntPtr dwCallbackInstance, uint dwFlags);

        uint midiInClose(IntPtr hMidiIn);
        uint midiInStart(IntPtr hMidiIn);
        uint midiInStop(IntPtr hMidiIn);
    }
}
