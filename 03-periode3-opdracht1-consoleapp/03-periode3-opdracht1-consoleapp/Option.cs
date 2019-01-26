using System;

namespace _03_periode3_opdracht1_consoleapp
{
    public struct Option
    {
        public string Value;
        public Action<int> InvokeMethod;

        public Option(string optionValue, Action<int> optionInvokeMethod)
        {
            Value = optionValue;
            InvokeMethod = optionInvokeMethod;
        }
    }
}