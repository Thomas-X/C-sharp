using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading;

namespace _03_periode3_opdracht1_consoleapp
{
    // App class because the application's state needs to be able to be resetted, this is only able if there's a separate class for the 
    // entire app and the Program class just manages an instance of App.
    public class App
    {
        private const string Divider = "<==================>";
        protected List<Option> Names = new List<Option>();


        // Prints shortcuts that can be used.
        public void Info()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(App.Divider);
            Console.Write("Press CTRL+R at any time to reset the whole application, removing any stored data.\r\n");
            Console.Write("Press CTRL+W to exit the program when you\'ve exhausted all options.\r\n");
            Console.Write("Press CTRL+Q to go back to the selection menu when you\'ve exhausted all options.\r\n");
            Console.WriteLine(App.Divider + "\r\n");
            Console.ResetColor();
        }

        // Enter name option delegate.
        public void EnterName (int idx)
        {
            Console.Write("Enter a name you want to enter: ");
            string input = Console.ReadLine();
            this.Names.Add(new Option(input, this.ShowAndFormatName));

            // After adding, display the current names.
            this.SwitchPage();
            this.ShowNames(idx);
        }

        // Enter specific name option delegate
        public void ShowAndFormatName(int idx)
        {
            Console.WriteLine("Your name is: {0}", this.Names[idx].Value);
        }

        // Simple calculator that can parse mathematical expressions. Uses an instance of DataTable to figure out the answer
        // to a given expression.
        public void SimpleCalculator(int idx)
        {
            Console.Write("Simple calculator. These mathematical operators are tested: +, /, %, * and -\r\nSome examples are:\r\n");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("1.1.1 + 1\r\n");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("1 + 1\r\n");
            Console.ResetColor();
            Console.Write("Enter a calculation: ");
            string input = Console.ReadLine();
            // Make an empty DataTable because we can use the Compute method of DataTable to figure out 
            // math expressions like 1+1 and (2 + 2 * 5) / 2. Not very memory efficient.
            DataTable table = new DataTable();
            try
            {
                object computedValue = table.Compute(input, "");
                Console.Write("The answer to that calculation is: ");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write(computedValue);
                Console.ResetColor();
            }
            catch (Exception)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.BackgroundColor = ConsoleColor.Red;
                Console.WriteLine("\r\nInvalid calculation input! Returning to selection menu in 3 seconds..");

                Thread.Sleep(3000);
                Console.ResetColor();
                this.SwitchPage();
                this.SelectionMenu();
            }
            
        }

        // Shows all currently stored names.
        public void ShowNames(int idx)
        {
            this.PrintOutList(this.Names, "Available names:");
            this.SelectFromList(this.Names);
        }

        // Switches pages, clearing the console and printing the info, fooling the user that the info were ever gone.
        public void SwitchPage()
        {
            Console.Clear();
            this.Info();
        }

        // Prints out a list of Option enums.
        // Uses StringBuilder to add formatting to each item.
        public void PrintOutList(List<Option> list, string info)
        {
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < list.Count; i++)
            {
                stringBuilder.AppendFormat("{0, 2}. {1}\r\n", i + 1, list[i].Value);
            }
            Console.WriteLine(info);
            Console.Write(stringBuilder.ToString());
        }

        // "Home" page, we return to this page if something goes wrong in the selections.
        public void SelectionMenu()
        {
            List<Option> opts = new List<Option>()
            {
                new Option("Add a name", this.EnterName),
                new Option("Simple Calculator", this.SimpleCalculator),
                new Option("Show all names and pick one", this.ShowNames)
            };
            this.PrintOutList(opts, "Available options:");
            this.SelectFromList(opts);
        }

        // Selects an item from a List<Option> and invokes the delegate given for that instance Option.
        public void SelectFromList(List<Option> list)
        {
            Console.Write("\r\nEnter your selection from {0} to {1}: ", 1, list.Count);
            string input = Console.ReadLine();
            // Out meaning we pass a reference to the piece of memory used for storing the value, but unlike ref
            // we don't have to initialize it :).
            if (int.TryParse(input, out var val))
            {
                // Since we added 1 in the opts displaying but the index is still 0-based.
                val -= 1;
                // Check if index is valid and in the opts.
                if (val >= 0 && val < list.Count)
                {
                    this.SwitchPage();
                    list[val].InvokeMethod(val);
                }
            }
            else
            {
                this.SwitchPage();
                this.SelectionMenu();
            }
        }
    }
}