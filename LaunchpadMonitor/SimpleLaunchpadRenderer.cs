using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LaunchpadMonitor
{
    class SimpleLaunchpadRenderer : ILaunchpad
    {
        Launchpad.Color[,] grid = new Launchpad.Color[8, 8];

        Launchpad.Controller controller = new Launchpad.Controller();

        public void Clear()
        {
            for (int x = 0; x < 8; x++)
                for (int y = 0; y < 8; y++)
                    grid[x, y] = Launchpad.Color.off;
        }

        public void Set(int x, int y, Launchpad.Color c)
        {
            grid[x, y] = c;
        }

        public Launchpad.Color Get(int x, int y)
        {
            return grid[x, y];
        }

        public void Present()
        {
            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    controller.Set(x, y, grid[x, y]);
                }
            }
        }
    }
}
