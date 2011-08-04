using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MIDIDotNet
{
    public class MIDIException : Exception
    {
        string details;
        uint errorCode;

        public MIDIException(string details, uint errorCode)
        {
            this.details = details;
            this.errorCode = errorCode;
        }

        public string Details
        {
            get { return details; }
        }

        public uint ErrorCode
        {
            get { return errorCode; }
        }

        public override string ToString()
        {
            return String.Format("MIDIException: {0}, code {1}", Details, ErrorCode.ToString("X8"));
        }
    }

    public static class ErrorCode
    {
        // General errors
        public static uint MDNERR_INVALIDDEVICE  = 0xf0000001;
        public static uint MDNERR_DEVICENOTOPEN  = 0xf0000002;
        // Send message
        public static uint MDNERR_MSG_INVALIDSTATUS  = 0xf0000101;
        public static uint MDNERR_MSG_INVALIDDATA    = 0xf0000102;

        // GM errors
        public static uint MDNERR_GM_INVALIDCHANNEL  = 0xf0010001;
        public static uint MDNERR_GM_INVALIDNOTE     = 0xf0010002;
        public static uint MDNERR_GM_INVALIDVELOCITY = 0xf0010003;

    }
}
