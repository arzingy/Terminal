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

namespace Terminal
{
    /// <summary>
    /// Логика взаимодействия для News.xaml
    /// </summary>
    public partial class News : Window
    {
        private int time = 0;
        private string[] files;
        private readonly bool m;

        public News(double scroller_thing, bool mw)
        {
            InitializeComponent();
            m = mw;
            if (m)
                btHome.Visibility = Visibility.Hidden;
            loadInfo();
            scroller.ScrollToVerticalOffset(scroller_thing);
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
                if (time >= 180 && time <= 240)
                {
                    if (time == 180)
                    {
                        btBack.IsEnabled = false;
                        scroller.IsEnabled = false;
                        dataGrid.IsEnabled = false;
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

        public class Piece
        {
            public ImageSource photo { get; set; }
            public string news { get; set; }
        }

        private void btBack_Click(object sender, RoutedEventArgs e)
        {
            if (m)
            {
                MainWindow mainwindow = new MainWindow();
                mainwindow.Show();
                this.Close();
            }
            else
            {
                if (File.Exists(System.IO.Path.GetTempPath() + "forTerminal.pdf"))
                    File.Delete(System.IO.Path.GetTempPath() + "forTerminal.pdf");
                Prof prof = new Prof
                {
                    Background = this.Background
                };
                prof.Show();
                this.Close();
            }
        }

        private void loadInfo()
        {
            dataGrid.Items.Clear();
            dataGrid.Items.Refresh();
            scroller.ScrollToVerticalOffset(0);
            if (m)
                files = Directory.GetFiles(@"..\..\Resources\Новости\Документы");
            else
                files = Directory.GetFiles(@"..\..\Resources\Информация\Профсоюзы\Новости\Документы");
            string str = Assembly.GetExecutingAssembly().Location, str1 = "pack://";
            str = str.Remove(str.Length - 22, 22);
            int c = 0;
            for (int i = 0; i < str.Length; i++)
                if (str[i] != '\\')
                {
                    str1 += str[i];
                }
                else
                {
                    c = i + 1;
                    break;
                }
            str = str.Substring(c);
            str1 += ",,,";
            str1 += str;
            if (m)
                str1 += "Resources\\Новости\\Изображения\\";
            else
                str1 += "Resources\\Информация\\Профсоюзы\\Новости\\Изображения\\";
            for (int i = files.Length - 1; i >= 0; i--)
            {
                string[] lines = File.ReadAllLines(files[i]);
                ImageSource aphoto;
                string anews = lines[4] + "\r\n" + lines[6] + "\r\n\r\n";
                if ((m && File.Exists(@"..\..\Resources\Новости\Изображения\" + lines[0]))
                    || (!m && File.Exists(@"..\..\Resources\Информация\Профсоюзы\Новости\Изображения\" + lines[0])))
                    aphoto = new BitmapImage(new Uri(@str1 + lines[0]));
                else
                    aphoto = new BitmapImage(new Uri(@str1 + "default.jpg"));
                anews += lines[8] + "..";
                dataGrid.Items.Add(new Piece { photo = aphoto, news = anews });
            }
        }

        private void eraseTime()
        {
            if (time < 180)
                time = 0;
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            eraseTime();
        }

        private void dataGrid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            eraseTime();
        }

        private void btQuitter_Click(object sender, RoutedEventArgs e)
        {
            btBack.IsEnabled = true;
            scroller.IsEnabled = true;
            dataGrid.IsEnabled = true;
            quitter.Visibility = Visibility.Hidden;
            tblQuitter.Visibility = Visibility.Hidden;
            tblQuitterTime.Visibility = Visibility.Hidden;
            btQuitter.Visibility = Visibility.Hidden;
            time = 0;
        }

        private void scroller_MouseDown(object sender, MouseButtonEventArgs e)
        {
            eraseTime();
        }

        private void tbBorder_MouseDown(object sender, MouseButtonEventArgs e)
        {
            eraseTime();
        }

        private void dataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var dg = sender as DataGrid;
            NewsPiece newspiece = new NewsPiece(files[files.Length - 1 - dg.Items.IndexOf(dg.SelectedItem)], m)
            {
                Background = this.Background,
                scroller_thing = scroller.VerticalOffset
            };
            newspiece.Show();
            this.Close();
        }

        private void btHome_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainwindow = new MainWindow();
            mainwindow.Show();
            this.Close();
        }
    }
}
