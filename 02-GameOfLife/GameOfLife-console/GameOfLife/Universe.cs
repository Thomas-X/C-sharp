using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace GameOfLife
{
    public class Universe
    {
        public static Cell[,] Generate(Cell[,] grid)
        {
            for (int i = 0; i < Program.Width; i++)
            {
                for (int o = 0; o < Program.Height; o++)
                {

                    if ((i + o) % 6 == 0 || (i + o) % 9 == 0)
                    {
                        grid[i, o] = Cell.Alive;
                    }
                    else
                    {
                        grid[i, o] = Cell.Dead;
                    }

//                    Random rand = new Random();
//                    int val = rand.Next(0, 100);
//
//                    // 33% of being alive
//                    if (val < 8)
//                    {
//                        grid[i, o] = Cell.Alive;
//                    }
//                    else
//                    {
//                        grid[i, o] = Cell.Dead;
//                    }
//                    
                    // grid[i, o] = (Cell) rand.Next(2);
                }
            }

            return grid;
        }

        public static Cell[,] Calculate(Cell[,] grid)
        {
            for (int x = 0; x < Program.Width; x++)
            {
                for (int y = 0; y < Program.Height; y++)
                {
                    int aliveNeighbors = Universe.GetAliveNeighbors(grid, x, y);
                    bool isAlive = grid[x, y] == Cell.Alive;

                    if (isAlive && aliveNeighbors < 2)
                    {
                        grid[x, y] = Cell.Dead;
                    } else if (isAlive && (aliveNeighbors == 2 || aliveNeighbors == 3))
                    {
                        grid[x, y] = Cell.Alive;
                    }
                    else if (isAlive && aliveNeighbors > 3)
                    {
                        grid[x, y] = Cell.Dead;
                    }
                    else if (!isAlive && aliveNeighbors == 3)
                    {
                        grid[x, y] = Cell.Alive;
                    }
                   
                }
            }

            return grid;
        }


        public static int GetAliveNeighbors(Cell[,] grid, int x, int y)
        {

            int count = 0;
            int[] columns = {Program.Width - 1, 0, 1};
            int[] rows = {Program.Height - 1, 0, 1};
            foreach (var column in columns)
            {
                foreach (var row in rows)
                {
                    // Our own position
                    if (column == 0 && row == 0)
                    {
                        continue;
                    }

                    int neighborX = (x + column) % Program.Width;
                    int neighborY = (y + row) % Program.Height;
                    if (grid[neighborX, neighborY] == Cell.Alive)
                    {
                        count++;
                    }
                }
            }

            return count;
        }
    }
}