using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Launchpad;

namespace SimpleLaunchpadDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            using (Controller controller = new Controller())
            {
                try
                {
                    Console.WriteLine(controller);
                }
                catch
                {
                    Console.WriteLine("No Launchpad found");
                    return;
                }

                Random r = new Random();
                for (int repeat = 0; repeat < 10; repeat++ )
                    for (int x = 0; x < 8; x++)
                        for (int y = 0; y < 8; y++)
                            controller.Set(x, y, new Color(r.Next(4), r.Next(4)));

                controller.Reset();

            }
        }
    }
}
