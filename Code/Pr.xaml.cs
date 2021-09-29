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
    /// Interaction logic for Pr.xaml
    /// </summary>
    public partial class Pr : Window
    {
        private int time = 0;

        public Pr()
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

        private void openWindow(string content, string name)
        {
            if (File.Exists(System.IO.Path.GetTempPath() + "forTerminal.pdf"))
                File.Delete(System.IO.Path.GetTempPath() + "forTerminal.pdf");
            InfoDoc infochooser = new InfoDoc(content, name);
            infochooser.Background = this.Background;
            infochooser.Show();
            this.Close();
        }

        private void btTradeUnions_Click(object sender, RoutedEventArgs e)
        {
            string content = @"..\..\Resources\Продукты и товары\Документы\Осиповичи.docx";
            if (!File.Exists(content))
                content = content.Replace("docx", "pdf");
            openWindow(content, "Осиповичский завод железобетонных конструкций");
        }

        private void btOrders_Click(object sender, RoutedEventArgs e)
        {
            string content = @"..\..\Resources\Продукты и товары\Документы\Барановичи.docx";
            if (!File.Exists(content))
                content = content.Replace("docx", "pdf");
            openWindow(content, "Барановичский завод строительных деталей и конструкций");
        }

        private void btHallOfFame_Click(object sender, RoutedEventArgs e)
        {
            string content = @"..\..\Resources\Продукты и товары\Документы\Брест.docx";
            if (!File.Exists(content))
                content = content.Replace("docx", "pdf");
            openWindow(content, "Брестский завод железобетонных конструкций и строительных деталей");
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            time = 0;
        }
    }
}
