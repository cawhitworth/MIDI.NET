using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LaunchpadMonitor
{
    interface IGathererRenderer
    {
        Gatherer Gatherer { set; }
        void Present();
        void ZoomIn();
        void ZoomOut();
        int Scale { get; }
    }
}
