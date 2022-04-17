using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace TimeTrackerWPF
{
    public partial class MainWindow
    {
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool IsIconic(IntPtr hWnd);

        //List that stores all the necessary processes
        private List<CurrentProcess> _old = new List<CurrentProcess>();

        //List of exceptions
        private readonly List<string> _exceptions = new List<string>
        {
            "ApplicationFrameHost",
            "TextInputHost",
            "SystemSettings"
        };

        public MainWindow()
        {
            InitializeComponent();
            UpdateProcess();
        }

        //Update list of process
        private async void UpdateProcess()
        {
            try
            {
                while (true)
                {
                    var all = Process.GetProcesses();
                    var recent = (from item in all where IsCorrect(item) select new CurrentProcess(item.MainWindowTitle, item.ProcessName)).ToList();

                    var allCorrect = _old.Concat(recent).ToList();
                    var current = allCorrect.Distinct().ToList();
                    _old = current;

                    var doesntRepeat = allCorrect.Where(item => allCorrect.IndexOf(item) == allCorrect.LastIndexOf(item)).ToList();

                    var repeats = allCorrect.Except(doesntRepeat).ToList();

                    foreach (var item in repeats)
                    {
                        var time = new DateTime();
                        if (item.StringTime != null)
                        {
                            time = DateTime.Parse(item.StringTime);
                        }
                        time = time.AddSeconds(1);
                        item.StringTime = time.ToString("HH:mm:ss");
                    }

                    //Update listview
                    ListViewProcess.ItemsSource = current;
                    await Task.Delay(1000);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show($"Something went wrong. App will be closed in 10 seconds. If the problem persists, write to me on github, attaching the error code: \n{e}");
                Thread.Sleep(10000);
                Close();
            }
            
        }

        private bool IsCorrect(Process item)
        {
            return item.MainWindowHandle.ToInt32() != 0 && item.MainWindowTitle != "" &&
                   !IsIconic(item.MainWindowHandle) && !_exceptions.Contains(item.ProcessName);
        }

        private void Close(object sender, RoutedEventArgs e)
        {
            Visibility = Visibility.Collapsed;
        }

        private void Open(object sender, RoutedEventArgs e)
        {
            Visibility = Visibility.Visible;
        }

        private void Exit(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
    
    public class CurrentProcess
    {
        public string? ProcessName { get; set; }
        public int Id { get; }
        public string? StringTime { get; set; }

        public CurrentProcess(string? processName, string id)
        {
            ProcessName = processName;
            Id = id.GetHashCode();
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