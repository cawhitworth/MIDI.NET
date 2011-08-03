using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace MIDIDotNet
{
    public static class InvokeLayer
    {
        public static class Messages
        {
            public const uint MM_MOM_OPEN  = 0x3C7;
            public const uint MM_MOM_CLOSE = 0x3C8;
            public const uint MM_MOM_DONE  = 0x3C9;
        }

        public static class MidiOpenFlags
        {
            public static uint CALLBACK_NULL = 0x00000000;
            public static uint CALLBACK_WINDOW = 0x00010000;
            public static uint CALLBACK_THREAD = 0x00020000;
            public static uint CALLBACK_FUNCTION = 0x00030000;
            public static uint CALLBACK_EVENT = 0x00050000;
        }

        [DllImport("winmm.dll")]
        public static extern uint midiInGetNumDevs();

        #region midiInGetDevCaps
        [StructLayout(LayoutKind.Sequential, CharSet=CharSet.Auto)]
        public class MIDIINCAPS
        {
            public const int MAXPNAMELEN = 32;
            public static readonly uint CBMIDIINCAPS = (uint)Marshal.SizeOf(typeof(MIDIINCAPS));
            public ushort wMid;
            public ushort wPid;
            public uint vDriverVersion;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MAXPNAMELEN)]
            public string szPname;
            public uint dwSupport;
        };

        [DllImport("winmm.dll", CharSet = CharSet.Auto)]
        public static extern uint midiInGetDevCaps(
            IntPtr uDeviceID,
            [Out] MIDIINCAPS lpMidiInCaps,
            uint cbMidiInCaps);

        #endregion

        [DllImport("winmm.dll")]
        public static extern uint midiOutGetNumDevs();

        #region midiOutGetDevCaps

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public class MIDIOUTCAPS
        {
            public const int MAXPNAMELEN = 32;
            public static readonly uint CBMIDIOUTCAPS = (uint)Marshal.SizeOf(typeof(MIDIOUTCAPS));
            public ushort wMid; // 2
            public ushort wPid; // 4
            public uint vDriverVersion; // 8
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MAXPNAMELEN)]
            public String szPName;
            public ushort wTechnology;  // 10
            public ushort wVoices; // 12
            public ushort wNotes; // 14
            public ushort wChannelMask;  //16
            public uint dwSpport; // 20
        }

        [DllImport("winmm.dll", CharSet=CharSet.Auto)]
        public static extern uint midiOutGetDevCaps(
            IntPtr uDeviceID,
            [Out] MIDIOUTCAPS lpMidiOutCaps,
            uint cbMidiOutCaps);
        #endregion

        #region midiOutOpen

        public delegate void MidiOutProc(IntPtr lphmo, uint uMsg, IntPtr dwInstance, IntPtr dwParam1, IntPtr dwParam2);

        [DllImport("winmm.dll")]
        public static extern uint midiOutOpen(
            ref IntPtr lphmo,
            uint uDeviceID,
            MidiOutProc dwCallback,
            IntPtr dwCallbackInstance,
            uint dwFlags);

        #endregion

        #region midiOutClose
        [DllImport("winmm.dll")]
        public static extern uint midiOutClose(IntPtr hmo);
        #endregion

        #region midiOutMessage
        [DllImport("winmm.dll")]
        public static extern uint midiOutMessage(
            IntPtr deviceID,
            uint msg,
            IntPtr dw1,
            IntPtr dw2);

        [DllImport("winmm.dll")]
        public static extern uint midiOutShortMsg(
            IntPtr deviceID,
            uint dwMsg);
        
        #endregion

        #region midiInOpen

        public delegate void MidiInProc(IntPtr hMidiIn, uint uMsg, IntPtr dwInstance, IntPtr dwParam1, IntPtr dwParam2);

        [DllImport("winmm.dll")]
        public static extern uint midiInOpen(
            ref IntPtr lphMidiIn,
            uint uDeviceID,
            MidiInProc dwCallback,
            IntPtr dwCallbackInstance,
            uint dwFlags);

        #endregion

        #region midiInClose
        [DllImport("winmm.dll")]
        public static extern uint midiInClose(IntPtr hMidiIn);
        #endregion

        #region midiInStart
        [DllImport("winmm.dll")]
        public static extern uint midiInStart(IntPtr hMidiIn);
        #endregion
        
        #region midiInStop
        [DllImport("winmm.dll")]
        public static extern uint midiInStop(IntPtr hMidiIn);
        #endregion
    }
}
