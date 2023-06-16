using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using TimeTrackerWPF.src;
using System.Windows.Forms;
using System.IO;
using System.Reflection;
using System.Drawing;
using System.Diagnostics.CodeAnalysis;
using System.ComponentModel;
using System.Windows.Controls;
using TimeTracker.src;

namespace TimeTrackerWPF
{
    public partial class MainWindow
    {
        SelectProcesses selectProcesses = new SelectProcesses();
        FullTime fullTime = new FullTime();
        private bool close = false;

        [RequiresAssemblyFiles()]
        public MainWindow()
        {
            InitializeComponent();
            Hide();
            ShowProcesses();
            ShowFullTime();

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
                var list = selectProcesses.Action();
                ProcessesTree.Items.Clear();
                foreach (var item in list)
                {
                    ProcessesTree.Items.Add(item);
                }
                await Task.Delay(1000);
            }
        }

        private async void ShowFullTime()
        {
            while (true) 
            {
                fullTime.IncreaseTime(1);
                FullTimeText.Content = fullTime.FullTimeStringValue;
                await Task.Delay(1000);
            }
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