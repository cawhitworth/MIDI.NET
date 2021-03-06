﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MIDIDotNet
{
    public class OutDevice : IOutDevice
    {
        private IWin32MIDI win32MIDI;
        private string deviceName;
        private uint deviceID;
        private InvokeLayer.MidiOutProc midiOutProc;
        private IntPtr hMidiOut = (IntPtr)0;
        private bool isOpen = false;
        private uint polyphony;
        private uint voices;
        private uint deviceType;
        private bool[] channels = new bool[16];

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
            device.polyphony = caps.wNotes;
            device.voices = caps.wVoices;
            device.deviceType = caps.wTechnology;
            for (int channel = 0; channel < 16; channel++)
            {
                device.channels[channel] = (caps.wChannelMask & (1 << channel)) != 0;
            }

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
            get { return isOpen; }
        }
        #endregion

        public uint Polyphony
        {
            get { return polyphony; }
        }

        public uint Voices
        {

            get { return voices; }
        }

        public bool[] Channel
        {
            get { return channels; }
        }

        public uint DeviceType
        {
            get { return deviceType; }
        }

        #region Public methods

        public void Open()
        {
            if (hMidiOut != (IntPtr)0)
            {
                throw new MIDIException("Cannot open a device that is already open", ErrorCode.MDNERR_DEVICEOPEN);
            }
            uint err = win32MIDI.midiOutOpen(ref hMidiOut, deviceID, midiOutProc, (IntPtr)0, InvokeLayer.MidiOpenFlags.CALLBACK_FUNCTION);
            if (err != InvokeLayer.ErrorCode.MMSYSERR_NOERROR)
            {
                throw new MIDIException("Error opening out device", err);
            }
            isOpen = true;
        }

        public void Close()
        {
            if (hMidiOut == (IntPtr)0)
            {
                throw new MIDIException("Cannot close a device that we did not open", ErrorCode.MDNERR_DEVICENOTOPEN);
            }
            uint err = win32MIDI.midiOutClose(hMidiOut);
            if (err != InvokeLayer.ErrorCode.MMSYSERR_NOERROR)
            {
                throw new MIDIException("Error closing out device", err);
            }
            isOpen = false;
        }

        public void SendShortMsg(uint msg)
        {
            if (hMidiOut == (IntPtr)0)
            {
                throw new MIDIException("Cannot send a message to an unopened device", ErrorCode.MDNERR_DEVICENOTOPEN);
            }
            if ((msg & 0x80) != 0x80)
            {
                throw new MIDIException("Invalid status byte in short message", ErrorCode.MDNERR_MSG_INVALIDSTATUS);
            }
            if ((msg & 0x00808000) != 0)
            {
                throw new MIDIException("Invalid data byte in short message", ErrorCode.MDNERR_MSG_INVALIDDATA);
            }
            uint err = win32MIDI.midiOutShortMsg(hMidiOut, msg);
        }

        #endregion
    }
}
