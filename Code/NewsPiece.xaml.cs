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
    /// Логика взаимодействия для NewsPiece.xaml
    /// </summary>
    public partial class NewsPiece : Window
    {
        private int time = 0;
        private bool vid = false;
        private readonly bool mw;
        public double scroller_thing;

        public NewsPiece(string filePath, bool m)
        {
            InitializeComponent();
            mw = m;
            loadDoc(filePath);
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

        private void btBack_Click(object sender, RoutedEventArgs e)
        {
            News news = new News(scroller_thing, mw)
            {
                Background = this.Background
            };
            news.Show();
            this.Close();
        }

        private void loadDoc(string filePath)
        {
            bool omg = false;
            int i2 = 0;
            string[] lines = File.ReadAllLines(@filePath);
            string str = Assembly.GetExecutingAssembly().Location, str1 = "pack://", strr = "";
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
            if (mw)
            {
                strr = "Resources\\Новости\\";
                str1 += strr;
            }
            else
            {
                strr = "Resources\\Информация\\Профсоюзы\\Новости\\";
                str1 += strr;
            }
            if (File.Exists(@"..\..\" + strr + "Видео\\" + lines[2]))
            {
                buttonVideo.Visibility = Visibility.Visible;
                string str2 = Assembly.GetExecutingAssembly().Location;
                if (mw)
                    str2 = str2.Replace("bin\\Debug\\Terminal.exe", "Resources\\Новости\\Видео\\" + lines[2]);
                else
                    str2 = str2.Replace("bin\\Debug\\Terminal.exe", "Resources\\Информация\\Профсоюзы\\Новости\\Видео\\" + lines[2]);
                video.Source = new Uri(@str2);
                if (File.Exists("..\\..\\" + strr + "Изображения\\" + lines[0]))
                    image.Source = new BitmapImage(new Uri(@str1 + "Изображения\\" + lines[0]));
                else
                    image.Source = new BitmapImage(new Uri(@str1 + "Изображения\\default.jpg"));
            }
            else if (File.Exists(@"..\..\" + strr + "Изображения\\" + lines[0]))
                imageFull.Source = new BitmapImage(new Uri(@str1 + "Изображения\\" + lines[0]));
            else
                imageFull.Source = new BitmapImage(new Uri(@str1 + "Изображения\\default.jpg"));
            header.Content = lines[4];
            date.Content = lines[6];
            for (int i = 8; i < lines.Length; i++)
            {
                if (lines[i] == "")
                {
                    omg = true;
                    i2 = i + 1;
                    break;
                }
                text.Text += "\r\n" + lines[i];
            }
            if (omg)
            {
                image.Visibility = Visibility.Hidden;
                imageFull.Visibility = Visibility.Hidden;
                video.Visibility = Visibility.Hidden;
                text.Visibility = Visibility.Hidden;
                scroller.Visibility = Visibility.Hidden;
                dataGrid.Visibility = Visibility.Visible;
                ImageSource aphoto;
                if (File.Exists(@"..\..\" + strr + "Изображения\\" + lines[0]))
                    aphoto = new BitmapImage(new Uri(@str1 + "Изображения\\" + lines[0]));
                else
                    aphoto = new BitmapImage(new Uri(@str1 + "Изображения\\default.jpg"));
                dataGrid.Items.Add(new ContactsInfo { photo = aphoto, position = text.Text});
                for (int i = i2; i < lines.Length; i++)
                {
                    ImageSource aphoto1;
                    if (File.Exists(@"..\..\" + strr + "Изображения\\" + lines[i]))
                        aphoto1 = new BitmapImage(new Uri(@str1 + "Изображения\\" + lines[i]));
                    else
                        aphoto1 = new BitmapImage(new Uri(@str1 + "Изображения\\default.jpg"));
                    i++;
                    string texty = "";
                    while (lines[i] != "")
                    {
                        texty += "\r\n" + lines[i++];
                        if (i >= lines.Length)
                            break;
                    }
                    dataGrid.Items.Add(new ContactsInfo { photo = aphoto1, position = texty });
                }
            }
        }

        private void btHome_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainwindow = new MainWindow();
            mainwindow.Show();
            this.Close();
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

        private void btQuitter_Click(object sender, RoutedEventArgs e)
        {
            btBack.IsEnabled = true;
            quitter.Visibility = Visibility.Hidden;
            tblQuitter.Visibility = Visibility.Hidden;
            tblQuitterTime.Visibility = Visibility.Hidden;
            btQuitter.Visibility = Visibility.Hidden;
            time = 0;
        }

        private void tbBorder_MouseDown(object sender, MouseButtonEventArgs e)
        {
            eraseTime();
        }

        private void video_MouseDown(object sender, MouseButtonEventArgs e)
        {
            eraseTime();
            if (!vid)
            {
                buttonVideo.Visibility = Visibility.Hidden;
                video.LoadedBehavior = System.Windows.Controls.MediaState.Play;
            }
            else
            {
                video.LoadedBehavior = System.Windows.Controls.MediaState.Pause;
                buttonVideo.Visibility = Visibility.Visible;
                buttonVideo.Content = "▶";
            }
            vid = !vid;
        }

        private void video_MediaEnded(object sender, RoutedEventArgs e)
        {
            video.LoadedBehavior = System.Windows.Controls.MediaState.Stop;
            video.Position = TimeSpan.FromSeconds(0);
            buttonVideo.Visibility = Visibility.Visible;
            buttonVideo.Content = "▶";
            vid = false;
        }

        private void buttonVideo_MouseDown(object sender, MouseButtonEventArgs e)
        {
            buttonVideo.Visibility = Visibility.Hidden;
            video.LoadedBehavior = System.Windows.Controls.MediaState.Play;
            vid = !vid;
        }

        private void scroller_MouseDown(object sender, MouseButtonEventArgs e)
        {
            eraseTime();
        }

        private void imageFull_MouseDown(object sender, MouseButtonEventArgs e)
        {
            eraseTime();
        }

        private void image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            eraseTime();
        }

        private void text_MouseDown(object sender, MouseButtonEventArgs e)
        {
            eraseTime();
        }

        private void dataGrid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            eraseTime();
        }

        public class ContactsInfo
        {
            public ImageSource photo { get; set; }
            public string position { get; set; }
        }
    }
}
