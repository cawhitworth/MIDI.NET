using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConwayLifeLaunchpad
{
    class Life
    {
        bool[,] currentGeneration;

        public Life(int width, int height)
        {
            currentGeneration = new bool[width, height];
        }

        public int Width
        {
            get { return currentGeneration.GetLength(0); }
        }
        
        public int Height 
        {
            get { return currentGeneration.GetLength(1); }
        }

        public bool this[int x, int y]
        {
            get { return currentGeneration[x, y]; }
            set { currentGeneration[x, y] = value; }
        }

        public void Tick()
        {
            bool[,] lastGeneration = new bool[Width, Height];
            for (int x = 0; x < Width; x++)
                for (int y = 0; y < Height; y++)
                    lastGeneration[x, y] = currentGeneration[x, y];

            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    int neighbours = 0;
                    for (int xx = x - 1; xx <= x + 1; xx++)
                    {
                        for (int yy = y - 1; yy <= y + 1; yy++)
                        {
                            if (xx >= 0 && xx < Width && yy >= 0 && yy < Height && !(xx == x && yy == y))
                            {
                                neighbours += lastGeneration[xx, yy] ? 1 : 0;
                            }
                        }
                    }
                    if (neighbours == 3 || (lastGeneration[x, y] && neighbours == 2))
                    {
                        currentGeneration[x, y] = true;
                    }
                    else
                    {
                        currentGeneration[x,y] = false;
                    }
                }
            }
        }
    }
}
