using Hardcodet.Wpf.TaskbarNotification;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;

namespace TimeTrackerWPF
{
    public partial class MainWindow : Window
    {
        //TaskbarIcon tbi = new TaskbarIcon();

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool IsIconic(IntPtr hWnd);

        //Лист, в котором хранятся все нужные процессы
        List<CurrentProcess> _old = new List<CurrentProcess>();

        //Лист исключений
        public List<string> _exceptions = new List<string>
        {
            "ApplicationFrameHost",
            "TextInputHost",
            "SystemSettings"
        };

        public MainWindow()
        {
            //Инициализируем
            InitializeComponent();
            UpdateProcess();
        }

        //Обновляем список процессов
        public async void UpdateProcess()
        {
            while (true)
            {
                //Получаем все запущенные процессы
                Process[] all = Process.GetProcesses();
                List<CurrentProcess> _new = new List<CurrentProcess>();

                foreach (Process item in all)
                {
                    //Провряем, подходит ли процесс: имеется ли интерфейс, есть ли название окна, не явлеятся ли процесс исключением
                    if (item.MainWindowHandle.ToInt32() != 0 && item.MainWindowTitle != "" && !IsIconic(item.MainWindowHandle) && !_exceptions.Contains(item.ProcessName))
                    {
                        //Создаём экземпляр класса CurrentProcess с параметрами как у процесса
                        CurrentProcess p = new CurrentProcess(item.MainWindowTitle, item.Id);
                        _new.Add(p);
                    }
                }

                var everything = _old.Concat(_new).ToList<CurrentProcess>();
                var _current = everything.Distinct().ToList<CurrentProcess>();
                _old = _current;

                var doesntRepeat = new List<CurrentProcess>();

                foreach (var item in everything)
                {
                    if(everything.IndexOf(item) == everything.LastIndexOf(item))
                    {
                        doesntRepeat.Add(item);
                    }
                }

                var repeats = everything.Except(doesntRepeat).ToList<CurrentProcess>();

                foreach (var item in repeats)
                {
                    item.Time += 1;
                }

                //Обновляем список
                listViewProcess.ItemsSource = _current;
                await Task.Delay(1000);
            }
            
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
        public int Id { get; set; }
        public int Time { get; set; }

        public CurrentProcess(string? processName, int id)
        {
            this.ProcessName = processName;
            this.Id = id;
        }

        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            var Process = (CurrentProcess)obj;
            return Process.Id == Id;
        }

        public override int GetHashCode()
        {
            return Id;
        }
    }

}