using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeTracker.src
{
    static class Constants
    {
        //Array of exceptions
        public static readonly string[] Exceptions = new string[]
        {
            "ApplicationFrameHost",
            "TextInputHost",
            "SystemSettings",
            "svchost",
            "TimeTracker",
            "explorer",
            "CalculatorApp"
        };
        public static readonly int DelayInMilliseconds = 1000;
    }
}
