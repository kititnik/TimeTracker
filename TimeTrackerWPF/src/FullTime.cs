using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeTrackerWPF.src
{
    class FullTime
    {
        public DateTime FullTimeValue { get; set; }
        public string? FullTimeStringValue { get; set; }
        Class c = new Class();

        public FullTime()
        {
            FullTimeStringValue = "00:00:00";
        }

        public void IncreaseTime(int seconds)
        {
            if(c.SelectCorrectProcesses().Count > 0)
            {
                FullTimeValue = FullTimeValue.AddSeconds(seconds);
                FullTimeStringValue = FullTimeValue.ToString("HH:mm:ss");
            }
        }
    }
}
