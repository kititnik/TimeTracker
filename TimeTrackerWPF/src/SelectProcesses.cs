using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TimeTrackerWPF.MainWindow;
using System.Windows.Documents;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Reflection;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows;
using TimeTracker.src;
using System.Management;
using System.Drawing.Design;
using System.ComponentModel;

namespace TimeTrackerWPF.src
{
    class SelectProcesses
    {
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool IsIconic(IntPtr hWnd);

        private List<MainProcess> _lastCurrentProcesses = new List<MainProcess>();

        public List<MainProcess> Action()
        {
            var correctProcesses = SelectCorrectProcesses();
            var result = _lastCurrentProcesses.Concat(IncreaseTime(correctProcesses)).Distinct().OrderByDescending(t => t.Time).ToList();
            _lastCurrentProcesses = result;
            return result;
        }


        public List<Process> SelectCorrectProcesses()
        {
            var all = Process.GetProcesses();
            var correctProcesses = new List<Process>();

            foreach (var item in all)
            {
                if (!IsCorrect(item)) continue;
                correctProcesses.Add(item);
            }
            return correctProcesses;
        }

        private List<MainProcess> IncreaseTime(List<Process> correctProcesses)
        {
            var currentProcesses = new List<MainProcess>();
            foreach (var item in correctProcesses)
            {
                var oldProcess = currentProcesses.FirstOrDefault(p => p.ProcessName == item.MainModule?.FileVersionInfo.FileDescription);
                if (oldProcess != null) continue;
                else
                {
                    var last = _lastCurrentProcesses.FirstOrDefault(p => p.ProcessName == item.MainModule?.FileVersionInfo.FileDescription);
                    if (last != null)
                    {
                        last.IncreaseTime(1);
                        UpdateSubProcesses(item, last);
                        currentProcesses.Add(last);
                    }
                    else
                    {
                        //Set icon and name
                        ImageSource appIcon = ProcessIcon.GetIcon(item.MainModule!.FileName!, false, false)!;
                        string name = item.MainModule.FileVersionInfo.FileDescription!;
                        //Find sub processes
                        var subProcesses = new List<SubProcess>();
                        var currentProcess = new MainProcess(name, appIcon, subProcesses);
                        UpdateSubProcesses(item, currentProcess);
                        currentProcesses.Add(currentProcess);
                    }
                }
            }
            return currentProcesses;
        }

        public void UpdateSubProcesses(Process process, MainProcess currentProcess) 
        {
            var newSubProcesses = Process.GetProcessesByName(process.ProcessName);
            foreach (var item in newSubProcesses)
            {
                if (!IsCorrect(item)) continue;
                var oldSubProcess = currentProcess.SubProcesses.FirstOrDefault(p => p.ProcessName == item.MainWindowTitle);
                if (oldSubProcess != null)
                {
                    oldSubProcess.IncreaseTime(1);
                }
                else
                {
                    var currentSubProcess = new SubProcess(item.MainWindowTitle, currentProcess.AppIcon);
                    currentProcess.SubProcesses.Add(currentSubProcess);
                }
            }
            currentProcess.SubProcesses = currentProcess.SubProcesses.OrderByDescending(p => p.Time).ToList();
        }

        private bool IsCorrect(Process item)
        {
            try
            {
                return item.MainWindowHandle.ToInt32() != 0 && !String.IsNullOrEmpty(item.MainModule?.FileVersionInfo.FileDescription) &&
                   !IsIconic(item.MainWindowHandle) && !Constants.Exceptions.Contains(item.ProcessName);
            }
            catch (Win32Exception) 
            {
                return false;
            }
        }
    }
}
