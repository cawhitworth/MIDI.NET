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
    }
}
