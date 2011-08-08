using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LaunchpadMonitor
{
    interface ILaunchpad : IDisposable
    {
        void Clear();

        void Set(int x, int y, Launchpad.Color c);
        Launchpad.Color Get(int x, int y);

        void Present();

        Launchpad.ButtonPressDelegate UpButtonPressDelegate { set; }
        Launchpad.ButtonPressDelegate DownButtonPressDelegate { set; }
        Launchpad.ButtonPressDelegate LeftButtonPressDelegate { set; }
        Launchpad.ButtonPressDelegate RightButtonPressDelegate { set; }
        Launchpad.GridButtonPressDelegate GridButtonPressDelegate { set; }
    }
}
