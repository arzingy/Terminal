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
using System.IO;
using Microsoft.Win32;
using System.Windows.Threading;
using System.Reflection;
using Spire.Doc;
using System.Diagnostics;
using Syncfusion.PdfViewer;
using Syncfusion.DocToPDFConverter;
using Syncfusion.DocIO.DLS;
using Syncfusion.DocIO;
using Syncfusion.Pdf;
using System.Globalization;

namespace Terminal
{
    /// <summary>
    /// Interaction logic for img.xaml
    /// </summary>
    public partial class img : Window
    {

        private int time = 0;
        public double st = 0;

        public img(int a)
        {
            InitializeComponent();

            string[] lines = File.ReadAllLines(@"..\..\Resources\Об организации\Документы\Объекты.txt");
            int i = 0, j = 0;
            for (; i < a; j++)
                if (lines[j] == "")
                    i++;
            string str1 = Assembly.GetExecutingAssembly().Location, str = "pack://";
            str1 = str1.Remove(str1.Length - 22, 22);
            int c = 0;
            for (int l = 0; l < str1.Length; l++)
                if (str1[l] != '\\')
                {
                    str += str1[l];
                }
                else
                {
                    c = 1 + l;
                    break;
                }
            str1 = str1.Substring(c);
            str += ",,,";
            str += str1;
            imge.Source = new BitmapImage(new Uri(@str + "Resources\\Об организации\\Изображения\\Объекты\\" + lines[j]));
            j += 2;
            while (lines[j] != "")
            {
                description.Text += "\r\n" + lines[j++];
                if (j >= lines.Length)
                    break;
            }
            DispatcherTimer dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Start();
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            if (quitter.IsVisible)
            {
                time++;
                if (time >= 180 && time <= 240)
                {
                    if (time == 180)
                    {
                        btBack.IsEnabled = false;
                        quitter.Visibility = Visibility.Visible;
                        tblQuitter.Visibility = Visibility.Visible;
                        tblQuitterTime.Visibility = Visibility.Visible;
                        btQuitter.Visibility = Visibility.Visible;
                    }
                    int timeForShow = 60 + 180 - time;
                    tblQuitterTime.Text = timeForShow.ToString() + " секунд";
                    if (timeForShow % 10 == 1 && timeForShow != 11)
                        tblQuitterTime.Text += "у";
                    else if ((timeForShow % 10 == 2 && timeForShow != 12) || (timeForShow % 10 == 3 && timeForShow != 13) || (timeForShow % 10 == 4 && timeForShow != 14))
                        tblQuitterTime.Text += "ы";
                }
                else if (time > 240)
                {
                    time = 0;
                    MainWindow mainwindow = new MainWindow();
                    mainwindow.Show();
                    mainwindow.openVideo();
                    this.Close();
                }
            }
        }

        private void tbBorder_MouseDown(object sender, MouseButtonEventArgs e)
        {
            eraseTime();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            eraseTime();
        }

        private void eraseTime()
        {
            if (time < 180)
                time = 0;
        }

        private void btBack_Click(object sender, RoutedEventArgs e)
        {
            About mainwindow = new About(true, st)
            {
                Background = this.Background
            };
            mainwindow.Show();
            this.Close();
        }

        private void btQuitter_Click(object sender, RoutedEventArgs e)
        {
            btBack.IsEnabled = true;
            quitter.Visibility = Visibility.Hidden;
            tblQuitter.Visibility = Visibility.Hidden;
            tblQuitterTime.Visibility = Visibility.Hidden;
            btQuitter.Visibility = Visibility.Hidden;
            time = 0;
        }

        private void description_MouseDown(object sender, MouseButtonEventArgs e)
        {
            eraseTime();
        }

        private void scroller_MouseDown(object sender, MouseButtonEventArgs e)
        {
            eraseTime();
        }

        private void image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            eraseTime();
        }

        private void btHome_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainwindow = new MainWindow();
            mainwindow.Show();
            this.Close();
        }
    }
}
