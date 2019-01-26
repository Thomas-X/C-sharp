using System;
using System.Runtime.CompilerServices;

namespace _03_periode3_opdracht1_consoleapp
{
    class Program
    {
        // App instance used for resetting application state
        public static App AppInstance = new App();

        // This method is called on start and the program ends when this 
        // method is done, in this case when the while loop is broken the process
        // ends.
        static void Main(string[] args)
        {
            Program.Init();
            while (Program.ShouldStayOpen());
        }

        // Method used for initializing a new app, also used for resetting the app.
        static void Init()
        {
            Program.AppInstance = new App();
            Program.AppInstance.Info();
            Program.AppInstance.SelectionMenu();
        }

        // Resets the app.
        static void Reset()
        {
            Console.Clear();
            Program.Init();
        }

        // Checks whether or not the process should keep running and 
        // watches for a combination of CTRL+<SomeKeyLikeW> shortcuts
        static bool ShouldStayOpen()
        {
            // Intercept incoming key combination
            ConsoleKeyInfo input = Console.ReadKey(true);
            bool shouldStayOpen = true;
            // Check if the current modifier value matches the value of Control (input.Modifiers=ConsoleModifiers)
            // ConsoleModifiers = Enum
            if (input.Modifiers == ConsoleModifiers.Control)
            {
                switch (input.Key)
                {
                    // CTRL+W = exit
                    case ConsoleKey.W:
                        // If the key DOES match W, return false and end the while loop that's blocking the process
                        shouldStayOpen = input.Key != ConsoleKey.W;
                        break;
                    // CTRL+R = reset
                    case ConsoleKey.R:
                        Program.Reset();
                        break;
                    // CTRL+Q = go back to menu
                    case ConsoleKey.Q:
                        Program.AppInstance.SwitchPage();
                        Program.AppInstance.SelectionMenu();
                        break;
                }
            }
            return shouldStayOpen;
        }
    }
}
