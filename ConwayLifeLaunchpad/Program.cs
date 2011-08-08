using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Launchpad;
using System.Threading;

namespace ConwayLifeLaunchpad
{
    class Program
    {
        static void Main(string[] args)
        {
            Random r = new Random();
            Life life = new Life(8, 8);

            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    life[x, y] = ((r.Next() & 1) == 1);
                }
            }

            //life[0, 2] = life[1, 2] = life[2, 2] = life[2, 1] = life[1, 0] = true;

            Color on = new Color(3, 3);
            Color off = new Color(0, 0);

            using (Controller c = new Controller())
            {
                c.Reset();

                for (int generation = 0; generation < 100; generation++)
                {
                    Console.WriteLine("{0}", generation);
                    for (int x = 0; x < 8; x++)
                    {
                        for (int y = 0; y < 8; y++)
                        {
                            if (life[x, y] != life.LastGeneration[x, y])
                            {
                                c.Set(x, y, life[x, y] ? on : off);
                            }
                        }
                    }

                    life.Tick();
                    Thread.Sleep(100);
                }
                c.Reset();

            }

        }
    }
}
