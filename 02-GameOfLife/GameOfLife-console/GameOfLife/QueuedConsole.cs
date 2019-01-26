using System;
using System.Text;

namespace GameOfLife
{
    public static class QueuedConsole
    {
        private static readonly StringBuilder stringBuilder = new StringBuilder();
        private static int _lineCount = 0;

        public static void Write(string message)
        {
            stringBuilder.Append(message);
        }

        public static void NewLine()
        {
            stringBuilder.Append("\r\n  ");
        }

        public static void WriteAll()
        {
            Console.CursorLeft = 2;
            Console.WriteLine(stringBuilder.ToString());
            stringBuilder.Clear();
        }
    }
}