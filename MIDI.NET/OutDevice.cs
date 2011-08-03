using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MIDIDotNet
{
    public class OutDevice
    {
        private IWin32MIDI win32MIDI;
        private string deviceName;
        private uint deviceID;
        private InvokeLayer.MidiOutProc midiOutProc;
        private IntPtr hMidiOut;
        private bool open = false;

        #region Constructors
        private OutDevice(IWin32MIDI win32MIDI, uint deviceID)
        {
            this.win32MIDI = win32MIDI;
            this.deviceID = deviceID;
            midiOutProc = new InvokeLayer.MidiOutProc(MidiOutProc);
        }

        public static OutDevice FromCaps(InvokeLayer.MIDIOUTCAPS caps, IWin32MIDI win32MIDI, uint deviceID)
        {
            OutDevice device = new OutDevice(win32MIDI, deviceID);

            device.deviceName = caps.szPName;

            return device;
        }
        #endregion

        private void MidiOutProc(IntPtr lphmo, uint uMsg, IntPtr dwInstance, IntPtr dwParam1, IntPtr dwParam2)
        {

        }

        #region Properties
        public string DeviceName
        {
            get { return deviceName; }
        }

        public bool IsOpen
        {
            get { return open; }
        }
        #endregion

        #region Public methods

        public void Open()
        {
            uint err = win32MIDI.midiOutOpen(ref hMidiOut, deviceID, midiOutProc, (IntPtr)0, InvokeLayer.MidiOpenFlags.CALLBACK_FUNCTION);
            if (err != InvokeLayer.ErrorCodes.MMSYSERR_NOERROR)
            {
                throw new MIDIException("Error on out device open", err);
            }
            open = true;
        }

        #endregion
    }
}
