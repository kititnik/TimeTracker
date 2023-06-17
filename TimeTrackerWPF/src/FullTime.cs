using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeTracker.src
{
    class FullTime
    {
        public DateTime FullTimeValue { get; set; }
        public string? FullTimeStringValue { get; set; }
        ProcessesList c = new ProcessesList();

        public FullTime()
        {
            FullTimeStringValue = "00:00:00";
        }

        public void IncreaseTime(int seconds)
        {
            FullTimeValue = FullTimeValue.AddSeconds(seconds);
            FullTimeStringValue = FullTimeValue.ToString("HH:mm:ss");
        }
    }
}
