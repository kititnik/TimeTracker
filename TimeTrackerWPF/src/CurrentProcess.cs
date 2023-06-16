using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace TimeTracker.src
{
    public class CurrentProcess
    {
        public string ProcessName { get; set; }
        public string ShownProcessName { get; set; }
        public string? StringTime { get; set; }
        public DateTime Time { get; set; }
        public ImageSource AppIcon { get; set; }
        public bool IsExpanded { get; set; }

        public CurrentProcess(string processName, ImageSource appIcon)
        {
            ProcessName = processName;
            if (processName.Length > 70)
            {
                ShownProcessName = processName.Substring(0, 70) + "...";
            }
            else
            {
                ShownProcessName = processName;
            }
            if (String.IsNullOrEmpty(StringTime))
            {
                IncreaseTime(1);
            }
            AppIcon = appIcon;
        }

        public void IncreaseTime(int seconds)
        {
            Time = Time.AddSeconds(1);

            if (Time.Minute < 1 && Time.Hour < 1) StringTime = "< 1 мин.";
            else if (Time.Hour < 1) StringTime = Time.ToString("mмин.");
            else if(Time.Hour >= 1 || Time.Day >= 1) StringTime = Time.ToString("Hч. mмин.");
        }

        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            var process = (CurrentProcess)obj;
            return process.ProcessName == ProcessName;
        }

        public override int GetHashCode()
        {
            return ProcessName.GetHashCode();
        }
    }
}
