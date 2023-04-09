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

namespace TimeTrackerWPF.src
{
    class Class
    {
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool IsIconic(IntPtr hWnd);

        //List that stores processes from last iteration
        private List<CurrentProcess> _lastCorrectProcesses = new List<CurrentProcess>();

        public List<CurrentProcess> Action()
        {
            var correctProcesses = SelectCorrectProcesses();
            var result = _lastCorrectProcesses.Concat(correctProcesses).Distinct().OrderByDescending(t => t.Time).ToList();
            IncreaseTime(correctProcesses);
            _lastCorrectProcesses = result;
            return result;
        }


        public List<CurrentProcess> SelectCorrectProcesses()
        {
            var all = Process.GetProcesses();
            var correctProcesses = new List<CurrentProcess>();

            foreach (var item in all)
            {
                if (IsCorrect(item))
                {
                    try
                    {
                        ImageSource? appIcon = ProcessIcon.GetIcon(item.MainModule?.FileName, false, false);
                        correctProcesses.Add(new CurrentProcess(item.MainModule?.FileVersionInfo.FileDescription, item.Id, appIcon));
                    }
                    catch
                    {
                        continue;
                    }
                }
            }
            return correctProcesses;
        }

        private void IncreaseTime(List<CurrentProcess> correctProcesses)
        {
            //if element was earlier and now in list - increase its time
            var oldAndNewProcesses = _lastCorrectProcesses.Concat(correctProcesses).ToList();
            var dontRepeat = oldAndNewProcesses.Where(item => oldAndNewProcesses.IndexOf(item) == oldAndNewProcesses.LastIndexOf(item)).ToList();
            var repeats = oldAndNewProcesses.Except(dontRepeat).ToList();
            foreach (var item in repeats)
            {
                item.IncreaseTime(1);
            }
        }

        private bool IsCorrect(Process item)
        {
            return item.MainWindowHandle.ToInt32() != 0 && !String.IsNullOrEmpty(item.ProcessName) &&
                   !IsIconic(item.MainWindowHandle) && !Constants.Exceptions.Contains(item.ProcessName);
        }
    }
}
