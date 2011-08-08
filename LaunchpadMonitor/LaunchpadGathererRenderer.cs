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

        int scale;

        public Gatherer Gatherer { set { gatherer = value; } }

        Launchpad.Color orange = new Launchpad.Color(3, 3);
        Launchpad.Color red = new Launchpad.Color(3, 0);
        Launchpad.Color green = new Launchpad.Color(0, 3);
        Launchpad.Color off = new Launchpad.Color(0, 0);

        public LaunchpadGathererRenderer(ILaunchpad launchpad)
        {
            this.launchpad = launchpad;
            this.scale = 1;

        }

        public void ZoomOut()
        {
            scale++;
        }

        public void ZoomIn()
        {
            if (scale > 1) scale--;
        }

        public int Scale
        {
            get { return scale; }
        }

        public void Present()
        {
            if (gatherer != null)
            {
                int[] lastN = gatherer.LatestN(8 * scale).ToArray<int>();

                launchpad.Clear();
                
                Launchpad.Color c;

                for (int column = 0; column < 8; column++)
                {
                    int height = 0;
                    try
                    {
                        for (int index = 0; index < scale; index++)
                        {
                            height += lastN[column * scale + index];
                        }
                        height = (height * 8) / (25 * scale); // 0 - 31
                    }
                    catch (System.IndexOutOfRangeException ex)
                    {
                        height = 0;
                    }
                    int y = 0;
                    while (height > 0)
                    {
                        int intensity = height;
                        if (height > 3)
                        {
                            intensity = 3;
                        }

                        if (y < 3)
                        {
                            c = new Launchpad.Color(0, intensity) ;
                        }
                        else if (y >= 3 && y < 5)
                        {
                            c = new Launchpad.Color(intensity, intensity);
                        }
                        else 
                        {
                            c = new Launchpad.Color(intensity, 0) ;
                        }
                        launchpad.Set(column, 7-y, c);
                        height -= 4;
                        y++;
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
