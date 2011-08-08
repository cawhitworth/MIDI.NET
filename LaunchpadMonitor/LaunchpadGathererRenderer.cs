using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LaunchpadMonitor
{
    class LaunchpadGathererRenderer : IGathererRenderer
    {
        Gatherer gatherer = null;
        ILaunchpad launchpad = null;

        public Gatherer Gatherer { set { gatherer = value; } }

        Launchpad.Color orange = new Launchpad.Color(3, 3);
        Launchpad.Color red = new Launchpad.Color(3, 0);
        Launchpad.Color green = new Launchpad.Color(0, 3);
        Launchpad.Color off = new Launchpad.Color(0, 0);

        public LaunchpadGathererRenderer(ILaunchpad launchpad)
        {
            this.launchpad = launchpad;

        }

        public void Present()
        {
            if (gatherer != null)
            {
                int[] lastEight = gatherer.LatestN(8).ToArray<int>();
                launchpad.Clear();
                for (int column = 0; column < 8; column++)
                {
                    lastEight[column] = (lastEight[column] * 2) / 25;
                    for (int height = 0; height <= lastEight[column]; height++)
                    {
                        if (height <= 3)
                        {
                            launchpad.Set(column, 7-height, green);
                        }
                        else if (height > 3 && height < 5)
                        {
                            launchpad.Set(column, 7-height, orange);
                        }
                        else 
                        {
                            launchpad.Set(column, 7-height, red);
                        }
                    }
                }

                launchpad.Present();
            }
            else
            {
                launchpad.Clear();
                launchpad.Present();
            }
        }
    }
}
