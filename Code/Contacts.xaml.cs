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
using System.Windows.Threading;
using System.Reflection;

namespace Terminal
{
    /// <summary>
    /// Логика взаимодействия для Contacts.xaml
    /// </summary>
    public partial class Contacts : Window
    {

        private int timer = 0;

        public Contacts()
        {
            InitializeComponent();
            loadInfo();
            DispatcherTimer dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Start();
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            if (this.IsActive)
            {
                timer++;
                if (timer >= 180 && timer <= 240)
                {
                    if (timer == 180)
                    {
                        btBack.IsEnabled = false;
                        scroller.IsEnabled = false;
                        dataGrid.IsEnabled = false;
                        quitter.Visibility = Visibility.Visible;
                        tblQuitter.Visibility = Visibility.Visible;
                        tblQuitterTime.Visibility = Visibility.Visible;
                        btQuitter.Visibility = Visibility.Visible;
                    }
                    int timeForShow = 60 + 180 - timer;
                    tblQuitterTime.Text = timeForShow.ToString() + " секунд";
                    if (timeForShow % 10 == 1 && timeForShow != 11)
                        tblQuitterTime.Text += "у";
                    else if ((timeForShow % 10 == 2 && timeForShow != 12) || (timeForShow % 10 == 3 && timeForShow != 13) || (timeForShow % 10 == 4 && timeForShow != 14))
                        tblQuitterTime.Text += "ы";
                }
                else if (timer > 240)
                {
                    timer = 0;
                    MainWindow mainwindow = new MainWindow();
                    mainwindow.Show();
                    mainwindow.openVideo();
                    this.Close();
                }
            }
        }



        private void eraseTime()
        {
            if (timer < 180)
                timer = 0;
        }

        public class ContactsInfo
        {
            public ImageSource photo { get; set; }
            public string position { get; set; }
            public string name { get; set; }
            public string time { get; set; }
            public string phonenumber { get; set; }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainwindow = new MainWindow();
            mainwindow.Show();
            this.Close();
        }

        private void btBack_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainwindow = new MainWindow();
            mainwindow.Show();
            this.Close();
        }

        private void loadInfo()
        {
            dataGrid.Items.Clear();
            dataGrid.Items.Refresh();
            scroller.ScrollToVerticalOffset(0);
            string[] lines = File.ReadAllLines(@"..\..\Resources\Контакты\Документы\contactInfo.txt");
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
            str1 += "Resources\\Контакты\\Изображения\\";
            int linesCount = (lines.Length / 6);
            if (lines.Length % 6 != 0)
                linesCount++;
            for (int i = 0; i < linesCount; i++)
            {
                ImageSource aphoto;
                if (File.Exists(@"..\..\Resources\Контакты\Изображения\" + lines[i * 6]))
                    aphoto = new BitmapImage(new Uri(@str1 + lines[i * 6]));
                else
                    aphoto = new BitmapImage(new Uri(@str1 + "user.jpg"));
                dataGrid.Items.Add(new ContactsInfo { photo = aphoto, position = lines[i * 6 + 1], name = lines[i * 6 + 2], time = lines[i * 6 + 3], phonenumber = lines[i * 6 + 4] });
            }
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
            timer = 0;
        }

        private void scroller_MouseDown(object sender, MouseButtonEventArgs e)
        {
            eraseTime();
        }

        private void tbBorder_MouseDown(object sender, MouseButtonEventArgs e)
        {
            eraseTime();
        }
    }
}
