using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.IO;
using System.Reflection;

namespace Terminal
{
    /// <summary>
    /// Логика взаимодействия для Info.xaml
    /// </summary>
    public partial class Info : Window
    {
        private int time = 0;

        public Info()
        {
            InitializeComponent();
            DispatcherTimer dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Start();
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            if (this.IsActive)
            {
                time++;
                if (time == 60)
                {
                    time = 0;
                    MainWindow mainwindow = new MainWindow();
                    mainwindow.Show();
                    mainwindow.openVideo();
                    this.Close();
                }
            }
        }

        private void btBack_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainwindow = new MainWindow();
            mainwindow.Show();
            this.Close();
        }

        private void openWindow(int content)
        {
            InfoChooser infochooser = new InfoChooser(content, 0);
            infochooser.Background = this.Background;
            infochooser.Show();
            this.Close();
        }

        private void btTradeUnions_Click(object sender, RoutedEventArgs e)
        {
            if (File.Exists(System.IO.Path.GetTempPath() + "forTerminal.pdf"))
                File.Delete(System.IO.Path.GetTempPath() + "forTerminal.pdf");
            Prof prof = new Prof();
            prof.Background = this.Background;
            prof.Show();
            this.Close();
        }

        private void btCorruption_Click(object sender, RoutedEventArgs e)
        {
            openWindow(2);
        }

        private void btOrders_Click(object sender, RoutedEventArgs e)
        {
            openWindow(3);
        }

        private void btHallOfFame_Click(object sender, RoutedEventArgs e)
        {
            Doska doska = new Doska();
            doska.Background = this.Background;
            doska.Show();
            this.Close();
        }

        private void btSafety_Click(object sender, RoutedEventArgs e)
        {
            openWindow(5);
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            time = 0;
        }

        private void bt_Click(object sender, RoutedEventArgs e)
        {
            openWindow(6);
        }
    }
}
