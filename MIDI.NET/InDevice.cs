using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MIDIDotNet
{
    public class InDevice
    {
        IWin32MIDI win32MIDI;
        string deviceName;
        uint deviceID;
        bool isOpen = false;
        bool isStarted = false;
        IntPtr hMidiIn = (IntPtr)0;
        InvokeLayer.MidiInProc midiInProc;
        HandleData handleDataDelegate = null;

        private InDevice(IWin32MIDI win32MIDI, uint deviceID)
        {
            this.win32MIDI = win32MIDI;
            this.deviceID = deviceID;
            midiInProc = new InvokeLayer.MidiInProc(MidiInProc);
        }

        public static InDevice FromCaps(InvokeLayer.MIDIINCAPS caps, IWin32MIDI win32MIDI, uint deviceID)
        {
            InDevice device = new InDevice(win32MIDI, deviceID);
            device.deviceName = caps.szPName;
            return device;
        }


        private void MidiInProc(IntPtr hMidiIn, uint uMsg, IntPtr dwInstance, IntPtr dwParam1, IntPtr dwParam2)
        {
            switch (uMsg)
            {
                case InvokeLayer.MidiInMessages.MM_MIM_DATA:
                    if (handleDataDelegate != null)
                    {
                        handleDataDelegate(dwParam1, dwParam2);
                    }
                    break;
                default:
                    break;
            }
        }

        public string DeviceName
        {
            get { return deviceName; }
        }

        public bool IsOpen
        {
            get { return isOpen; }
        }

        public bool IsStarted
        {
            get { return isStarted; }
        }

        public HandleData HandleDataDelegate
        {
            set
            {
                handleDataDelegate = value;
            }
        }

        public void Open()
        {
            uint err = win32MIDI.midiInOpen(ref hMidiIn, deviceID, midiInProc, (IntPtr)0, InvokeLayer.MidiOpenFlags.CALLBACK_FUNCTION);
            if (err != InvokeLayer.ErrorCode.MMSYSERR_NOERROR)
            {
                throw new MIDIException("Error opening in device", err);
            }
            isOpen = true;
        }

        public void Close()
        {
            if (hMidiIn == (IntPtr)0)
            {
                throw new MIDIException("Cannot close a device that we did not open", ErrorCode.MDNERR_INVALIDDEVICE);
            }
            uint err = win32MIDI.midiInClose(hMidiIn);

            if (err != InvokeLayer.ErrorCode.MMSYSERR_NOERROR)
            {
                throw new MIDIException("Error closing in device", err);
            }
            isOpen = false;

        }

        public void Start()
        {
            if (hMidiIn == (IntPtr)0)
            {
                throw new MIDIException("Cannot start a device we did not open", ErrorCode.MDNERR_INVALIDDEVICE);
            }
            uint err = win32MIDI.midiInStart(hMidiIn);

            if (err != InvokeLayer.ErrorCode.MMSYSERR_NOERROR)
            {
                throw new MIDIException("Error starting in device", err);
            }

            isStarted = true;
        }

        public void Stop()
        {

        }
    }
}
