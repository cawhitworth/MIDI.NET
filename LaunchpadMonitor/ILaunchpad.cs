using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LaunchpadMonitor
{
    interface ILaunchpad
    {
        void Clear();

        void Set(int x, int y, Launchpad.Color c);
        Launchpad.Color Get(int x, int y);

        void Present();
    }
}
