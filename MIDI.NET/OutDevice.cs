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
        private IntPtr hMidiOut = (IntPtr)0;
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
            if (err != InvokeLayer.ErrorCode.MMSYSERR_NOERROR)
            {
                throw new MIDIException("Error on out device open", err);
            }
            open = true;
        }

        public void Close()
        {
            if (hMidiOut == (IntPtr)0)
            {
                throw new MIDIException("Cannot close a device that we did not open", ErrorCode.MDNERR_INVALIDDEVICE);
            }

            uint err = win32MIDI.midiOutClose(hMidiOut);
        }

        public void SendShortMsg(uint msg)
        {
            if (hMidiOut == (IntPtr)0)
            {
                throw new MIDIException("Cannot send a message to an unopened device", ErrorCode.MDNERR_DEVICENOTOPEN);
            }
            uint err = win32MIDI.midiOutShortMsg(hMidiOut, msg);
        }

        #endregion
    }
}
