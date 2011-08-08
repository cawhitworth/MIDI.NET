using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Launchpad
{
    public class Color
    {
        readonly int red;
        readonly int green;
        readonly bool clear;
        readonly bool copyto;

        public Color(int red, int green)
        {
            this.red = Clamp(0, 3, red);
            this.green = Clamp(0,3,green);
            this.copyto = false;
            this.clear = false;
        }

        public Color(int red, int Green, bool CopyTo)
        {
            this.red = Clamp(0, 3, red);
            this.green = Clamp(0,3, green);

            this.copyto = CopyTo;
            this.clear = false;
        }

        public Color()
        {
            red = 0; green = 0; copyto = false;
            clear = true;
        }

        public int Red
        {
            get { return red; }
        }

        public int Green
        {
            get { return green; }
        }

        public bool Clear
        {
            get { return clear; }
        }

        public bool CopyTo
        {
            get { return copyto; }
        }

        public byte Byte
        {
            get
            {
                return (byte)(Red | (CopyTo ? 1 << 2 : 0) | (Clear ? 1 << 3 : 0) | (Green << 4));
            }
        }

        public override string ToString()
        {
            return "R" + red + "G" + green + (copyto?" CopyTo":"") + (clear ?" Clear":"");
        }

        private static int Clamp(int min, int max, int value)
        {
            if (value < min) return min;
            if (value > max) return max;
            return value;
        }

        public static Color off = new Color();
    }
}
