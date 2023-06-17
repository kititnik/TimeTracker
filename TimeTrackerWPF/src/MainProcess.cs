using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Media;

namespace TimeTracker.src
{
    public class MainProcess : CurrentProcess
    {
        public List<SubProcess> SubProcesses { get; set; }

        public MainProcess(string processName, ImageSource appIcon, List<SubProcess> subProcesses) : base(processName, appIcon)
        {
            SubProcesses = subProcesses;
        }

        public void Update(Process originalProcess)
        {
            IncreaseTime(Constants.DelayInMilliseconds / 1000);

            var originalSubProcesses = Process.GetProcessesByName(originalProcess.ProcessName);
            foreach (var originalSubProcess in originalSubProcesses)
            {
                var existedSubProcess = SubProcesses.Find(t => t.ProcessName == originalProcess.MainWindowTitle);
                if(existedSubProcess != null) 
                {
                    existedSubProcess.IncreaseTime(Constants.DelayInMilliseconds / 1000);
                }
                else 
                {
                    if (!originalSubProcess.IsCorrect()) continue;
                    SubProcesses.Add(new SubProcess(originalSubProcess.MainWindowTitle, AppIcon));
                }
            }
        }
    }
}
