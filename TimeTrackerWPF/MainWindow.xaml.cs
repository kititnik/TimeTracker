using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using System.Windows.Forms;
using TimeTracker.src;

namespace TimeTrackerWPF
{
    public partial class MainWindow
    {
        ProcessesList processesList = new ProcessesList();
        FullTime fullTime = new FullTime();
        private bool close = false;

        [RequiresAssemblyFiles()]
        public MainWindow()
        {
            InitializeComponent();
            SetUpContextMenu();
            Hide();
            ShowProcesses();
        }

        private void SetUpContextMenu()
        {
            NotifyIcon ni = new NotifyIcon();
            ni.Icon = System.Drawing.Icon.ExtractAssociatedIcon(AppDomain.CurrentDomain.BaseDirectory + "/TimeTracker.exe");
            ni.Visible = true;
            ni.Text = "TimeTracker";
            ni.DoubleClick += new EventHandler(ShowApp!);
            ToolStripMenuItem exitMenuItem = new ToolStripMenuItem("Выйти");
            var contextMenuStrip = new ContextMenuStrip();
            contextMenuStrip.Items.Add(exitMenuItem);
            ni.ContextMenuStrip = contextMenuStrip;
            exitMenuItem.Click += CloseApp!;
        }

        private async void ShowProcesses()
        {
            while (true)
            {
                var originalProcesses = FindCorrectProcesses();
                if (originalProcesses.Count > 0)
                {
                    processesList.Update(originalProcesses);
                    ProcessesTree.Items.Clear();
                    foreach (var item in processesList)
                    {
                        ProcessesTree.Items.Add(item);
                    }
                    IncreaseFullTime();
                }
                await Task.Delay(Constants.DelayInMilliseconds);
            }
        }

        private void IncreaseFullTime()
        {
            fullTime.IncreaseTime(Constants.DelayInMilliseconds / 1000);
            FullTimeText.Content = fullTime.FullTimeStringValue;
        }

        public List<Process> FindCorrectProcesses()
        {
            var allProcesses = Process.GetProcesses();
            var correctProcesses = new List<Process>();

            foreach (var process in allProcesses)
            {
                if (!process.IsCorrect()) continue;
                correctProcesses.Add(process);
            }
            return correctProcesses;
        }

        private void HideApp()
        {
            Hide();
        }

        private void ShowApp(object Sender, EventArgs e)
        {
            Show();
        }

        void CloseApp(object sender, EventArgs e)
        {
            close = true;
            Close();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if(close) { base.OnClosing(e); }
            else 
            {
                e.Cancel = true;
                HideApp();
            }
            
        }
    }
}