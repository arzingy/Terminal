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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Windows.Threading;
using System.Reflection;

namespace Terminal
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int time = 0;
        private bool exit = false;
        private readonly string videoName;

        public MainWindow()
        {
            InitializeComponent();
            TaskBar.IsVisible = false;
            string str = Assembly.GetExecutingAssembly().Location;
            int c = str.IndexOf('\\') + 1;
            string str1 = "pack://" + str.Substring(0, c) + ",,," + str.Substring(c).Replace("bin\\Debug\\Terminal.exe", "Resources\\");
            string[] lines = File.ReadAllLines("..\\..\\Resources\\Дополнительно\\info.txt");
            if (File.Exists("..\\..\\Resources\\Дополнительно\\Обои\\" + lines[0]))
            {
                ImageBrush myBrush = new ImageBrush
                {
                    ImageSource = new BitmapImage(new Uri(@str1 + "Дополнительно\\Обои\\" + lines[0]))
                };
                this.Background = myBrush;
            }
            logo.Source = new BitmapImage(new Uri(@str1 + "Главная страница\\Изображения\\" + lines[1]));
            videoName = lines[2];
            DispatcherTimer dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Start();
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            time++;
            if (time == 60)
                openVideo();
        }

        private void btContacts_Click(object sender, RoutedEventArgs e)
        {
            doimp();
            Contacts contacts = new Contacts
            {
                Background = this.Background
            };
            contacts.Show();
            this.Close();
        }

        private void btAbout_Click(object sender, RoutedEventArgs e)
        {
            doimp();
            About about = new About(false, 0)
            {
                Background = this.Background
            };
            about.Show();
            this.Close();
        }

        private void btServices_Click(object sender, RoutedEventArgs e)
        {
            doimp();
            InfoChooser infochooser = new InfoChooser(10, 0)
            {
                Background = this.Background
            };
            infochooser.Show();
            this.Close();
        }

        private void btProducts_Click(object sender, RoutedEventArgs e)
        {
            doimp();
            Pr products = new Pr
            {
                Background = this.Background
            };
            products.Show();
            this.Close();
        }

        private void btInfo_Click(object sender, RoutedEventArgs e)
        {
            doimp();
            Info info = new Info
            {
                Background = this.Background
            };
            info.Show();
            this.Close();
        }

        private void btNews_Click(object sender, RoutedEventArgs e)
        {
            doimp();
            News news = new News(0, true)
            {
                Background = this.Background
            };
            news.Show();
            this.Close();
        }

        private void btStaff_Click(object sender, RoutedEventArgs e)
        {
            doimp();
            InfoChooser infochooser = new InfoChooser(7, 0)
            {
                Background = this.Background
            };
            infochooser.Show();
            this.Close();
        }

        private void btClose_Click(object sender, RoutedEventArgs e)
        {
            TaskBar.IsVisible = true;
            this.Close();
        }

        private void bt1Close_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            exit = !exit;
            bt1Close.Visibility = Visibility.Hidden;
            if (exit)
            {
                btClose.Visibility = Visibility.Visible;
                btClose.IsEnabled = true;
            }
            else
            {
                btClose.Visibility = Visibility.Hidden;
                btClose.IsEnabled = false;
            }
        }

        private void Organization_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && e.ClickCount == 2)
                bt1Close.Visibility = Visibility.Visible;
        }

        private void logo_MouseDown(object sender, MouseButtonEventArgs e)
        {
            openVideo();
        }

        private void video_MediaEnded(object sender, RoutedEventArgs e)
        {
            if (video.Visibility == Visibility.Visible)
                video.Position = TimeSpan.FromSeconds(0);
            else {
                video.Close();
                forVideo.Visibility = Visibility.Hidden;
                time = 0;
            }
        }

        private void video_MouseDown(object sender, MouseButtonEventArgs e)
        {
            video.Visibility = Visibility.Hidden;
            video.Position = TimeSpan.MaxValue;
        }

        public void openVideo()
        {
            if (this.IsActive && File.Exists("..\\..\\Resources\\Главная страница\\Видео\\" + videoName))
            {
                forVideo.Visibility = Visibility.Visible;
                video.Visibility = Visibility.Visible;
                string str1 = Assembly.GetExecutingAssembly().Location;
                str1 = str1.Remove(str1.Length - 22, 22);
                str1 += "Resources\\Главная страница\\Видео\\" + videoName;
                video.Source = new Uri(@str1);
                video.Play();
            }
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            time = 0;
        }

        private void doimp()
        {
            if (File.Exists(System.IO.Path.GetTempPath() + "forTerminal.pdf"))
                File.Delete(System.IO.Path.GetTempPath() + "forTerminal.pdf");
        }
    }
}
