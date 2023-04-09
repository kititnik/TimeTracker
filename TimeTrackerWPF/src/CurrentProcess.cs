using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace TimeTrackerWPF.src
{
    class CurrentProcess
    {
        public string? ProcessName { get; set; }
        public int Id { get; }
        public string? StringTime { get; set; }
        public DateTime Time { get; set; }
        public ImageSource? AppIcon { get; set; }

        public CurrentProcess(string? processName, int id, ImageSource? appIcon)
        {
            ProcessName = processName;
            Id = id;
            if (String.IsNullOrEmpty(StringTime))
            {
                IncreaseTime(1);
            }
            AppIcon = appIcon;
        }

        public void IncreaseTime(int seconds)
        {
            Time = Time.AddSeconds(seconds);
            if (Time.Minute < 1) StringTime = "< 1 мин";
            else if (Time.Hour < 1) StringTime = Time.ToString("m мин");
            else StringTime = Time.ToString("H ч m мин");
        }

        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            var process = (CurrentProcess)obj;
            return process.Id == Id;
        }

        public override int GetHashCode()
        {
            return Id;
        }
    }
}
