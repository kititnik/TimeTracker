using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using TimeTrackerWPF.src;

namespace TimeTracker.src
{
    public class SubProcess : CurrentProcess
    { 
        public SubProcess(string processName, ImageSource appIcon) : base(processName, appIcon) { }
    }
}
