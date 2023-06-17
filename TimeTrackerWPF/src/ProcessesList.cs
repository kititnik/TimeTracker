using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Windows.Media;
using TimeTracker.src;

namespace TimeTracker.src
{
    class ProcessesList : IEnumerable
    {
        private List<MainProcess> Processes = new List<MainProcess>();
        public IEnumerator GetEnumerator() => Processes.GetEnumerator();

        public void Update(List<Process> originalProcesses)
        {
            foreach (var originalProcess in originalProcesses)
            {
                MainProcess? existedProcess = Processes.Find(t => t.ProcessName == originalProcess.MainModule!.FileVersionInfo.FileDescription);
                if (existedProcess != null)
                {
                    existedProcess.Update(originalProcess);
                }
                else
                {
                    Processes.Add(CreateProcess(originalProcess));
                }
            }
        }

        private MainProcess CreateProcess(Process originalProcess)
        {
            ImageSource processIcon = ProcessIcon.GetIcon(originalProcess.MainModule!.FileName!, false, false)!;
            string processName = originalProcess.MainModule.FileVersionInfo.FileDescription!;
            var originalSubProcesses = Process.GetProcessesByName(originalProcess.ProcessName);
            List<SubProcess> subProcesses = new List<SubProcess>();
            var proccess = new MainProcess(processName, processIcon, subProcesses);
            foreach (var originalSubProcess in originalSubProcesses)
            {
                if (!originalSubProcess.IsCorrect()) continue;
                subProcesses.Add(new SubProcess(originalSubProcess.MainWindowTitle, proccess.AppIcon));
            }
            return proccess;
        }
    }
}
