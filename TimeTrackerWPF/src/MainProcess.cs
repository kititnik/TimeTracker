using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using TimeTracker.src;

namespace TimeTracker.src
{
    public class MainProcess : CurrentProcess
    {
        public List<SubProcess> SubProcesses { get; set; }

        public MainProcess(string processName, ImageSource appIcon, List<SubProcess> subProcesses) : base(processName, appIcon)
        {
            SubProcesses = subProcesses;
        }
    }
}
