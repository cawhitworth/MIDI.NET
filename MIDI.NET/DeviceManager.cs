using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MIDIDotNet
{
    public class DeviceManager
    {
        List<InDevice> inDevices = new List<InDevice>();
        List<OutDevice> outDevices = new List<OutDevice>();

        IWin32MIDI win32MIDI;

        public DeviceManager(IWin32MIDI win32MIDI)
        {
            this.win32MIDI = win32MIDI;
            UpdateDevices();
        }

        public List<InDevice> InDevices
        {
            get { return inDevices; }
        }

        public List<OutDevice> OutDevices
        {
            get { return outDevices; }
        }

        private void UpdateDevices()
        {
            UpdateInDevices();
            UpdateOutDevices();
        }

        private void UpdateInDevices()
        {
            inDevices.Clear();
            uint numDevs = win32MIDI.midiInGetNumDevs();
            for (int dev = 0; dev < numDevs; dev++)
            {
                InvokeLayer.MIDIINCAPS midiInCaps;
                win32MIDI.midiInGetDevCaps((IntPtr)dev, out midiInCaps);
                inDevices.Add(InDevice.FromCaps(midiInCaps));
            }
        }

        private void UpdateOutDevices()
        {
            outDevices.Clear();
            uint numDevs = win32MIDI.midiOutGetNumDevs();
            for (int dev = 0; dev < numDevs; dev++)
            {
                InvokeLayer.MIDIOUTCAPS midiOutCaps;
                win32MIDI.midiOutGetDevCaps((IntPtr)dev, out midiOutCaps);
                outDevices.Add(OutDevice.FromCaps(midiOutCaps));
            }
        }

    }
}
