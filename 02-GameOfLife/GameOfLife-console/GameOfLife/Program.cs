using System;
using System.Collections;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Timers;

namespace GameOfLife
{
    class Program
    {
        public static int Width = 64;
        // Must be a multiple of 8 (because of the way rendering to console is handled)
        public static int Height = 48;
        protected static Cell[,] Grid = new Cell[Width, Height];
        static void Main(string[] args)
        {
            Program.Intro();
            Program.Grid = Universe.Generate(Program.Grid);
            // Render generated Universe
            Program.Tick();
            // Start simulation
            while (true)
            {
                Console.CursorVisible = false;
                
                // Calculate new values
//                Stopwatch stopwatch1 = Stopwatch.StartNew();
                Program.Grid = Universe.Calculate(Program.Grid);
//                Console.SetCursorPosition(60, 1);
                //Console.Write("CALCULATION TIME (ms): ");
//                Console.WriteLine(stopwatch1.ElapsedMilliseconds);

                // Render the calculated and updated grid
//                Stopwatch stopwatch2 = Stopwatch.StartNew();
                Program.Tick();
//                Console.SetCursorPosition(60, 2);
                //Console.Write("RENDER TIME (ms): ");
//                Console.WriteLine(stopwatch2.ElapsedMilliseconds);
                
                // Thread.Sleep(1000);
            }
        }

        static void Intro()
        {
            Console.WriteLine("\r\n  ___   __   _  _ ____     __ ____    __   __ ____ ____ \r\n / __) / _\\ ( \\/ |  __)   /  (  __)  (  ) (  |  __|  __)\r\n( (_ \\/    \\/ \\/ \\) _)   (  O ) _)   / (_/\\)( ) _) ) _) \r\n \\___/\\_/\\_/\\_)(_(____)   \\__(__)    \\____(__|__) (____)\r\n");
            Console.ForegroundColor = ConsoleColor.Green;

            Console.WriteLine("\r\nCreated by: Thomas Zwarts");
            Console.WriteLine("Click window to pause, press enter to continue");
            Console.ResetColor();
        }
        static void Tick()
        {
            // 2 and 8 because Intro takes up space and depending on the current x,y change the position.
            // This way there's always a predefined offset the "grid" is rendered off.
            Console.SetCursorPosition(2, 11);

            // Only render every 16 lines for performance
            for (int i = 0; i < Program.Height; i++)
            {
                for (int o = 0; o < Program.Width; o++)
                {
                    if (Program.Grid[o, i] == Cell.Dead)
                    {
                        QueuedConsole.Write(" ");
                    } else if (Program.Grid[o, i] == Cell.Alive)
                    {
                        QueuedConsole.Write("#");
                    }
                }
                // If remainder is higher than 0 means we don't have a value that's divisible by 16, so skip it 
                if (i % 16 == 0)
                {
                    QueuedConsole.WriteAll();
                    // To see the 16 lines rendering at a time working uncomment this
                    // Thread.Sleep(1000);
                }
                else
                {
                    QueuedConsole.NewLine();
                }
            }
        }
    }
}
