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
    /// Логика взаимодействия для InfoChooser.xaml
    /// </summary>
    public partial class InfoChooser : Window
    {
        private int time = 0;
        private int content;
        private string[] files;
        private bool c = false;
        private bool d = false;
        private bool s = false;
        private List<string> notxps = new List<string>();
        private List<string> names = new List<string>();

        public InfoChooser(int contentType, double scroller_thing)
        {
            InitializeComponent();
            content = contentType;
            switch (contentType)
            {
                case 1:
                    header.Content = "Профсоюзы";
                    loadFiles("Профсоюзы");
                    break;
                case 2:
                    header.Content = "Борьба с коррупцией";
                    loadFiles("Борьба с коррупцией");
                    break;
                case 3:
                    header.Content = "Приказы";
                    loadFiles("Приказы");
                    break;
                case 4:
                    header.Content = "Доска почёта";
                    loadFiles("Доска почета");
                    break;
                case 5:
                    header.Content = "Охрана труда";
                    loadFiles("Охрана труда");
                    break;
                case 6:
                    header.Content = "Административные процедуры";
                    loadFiles("Административные процедуры");
                    break;
                case 7:
                    c = true;
                    header.Content = "Отдел кадров";
                    loadFiles("Отдел кадров");
                    break;
                case 8:
                    d = true;
                    header.Content = "Информация";
                    loadFiles("Информация");
                    break;
                case 9:
                    d = true;
                    header.Content = "Административные процедуры";
                    loadFiles("Административные процедуры");
                    break;
                case 10:
                    s = true;
                    header.Content = "Услуги";
                    loadFiles("Услуги");
                    break;
                default:
                    break;
            }
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
            if (d)
            {
                if (File.Exists(System.IO.Path.GetTempPath() + "forTerminal.pdf"))
                    File.Delete(System.IO.Path.GetTempPath() + "forTerminal.pdf");
                Prof info = new Prof();
                info.Background = this.Background;
                info.Show();
            }
            else if (!c && !s)
            {
                Info info = new Info();
                info.Background = this.Background;
                info.Show();
            }
            else
            {
                MainWindow mw = new MainWindow();
                mw.Background = this.Background;
                mw.Show();
            }
            this.Close();
        }

        private void btHome_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainwindow = new MainWindow();
            mainwindow.Show();
            this.Close();
        }

        public class ContactsInfo
        {
            public ImageSource photo { get; set; }
            public string filename { get; set; }
        }

        private string reverseString(string s)
        {
            char[] arr = s.ToCharArray();
            Array.Reverse(arr);
            return new string(arr);
        }

        private void loadFiles(string folderName)
        {
            dataGrid.Items.Clear();
            dataGrid.Items.Refresh();
            scroller.ScrollToVerticalOffset(0);
            try
            {
                if (s)
                    files = Directory.GetFiles(@"..\..\Resources\Услуги\Документы");
                else if (d)
                    files = Directory.GetFiles(@"..\..\Resources\Информация\Профсоюзы\" + folderName);
                else if (folderName != "Отдел кадров")
                    files = Directory.GetFiles(@"..\..\Resources\Информация\" + folderName);
                else
                    files = Directory.GetFiles(@"..\..\Resources\Отдел Кадров\Документы");
                string str = Assembly.GetExecutingAssembly().Location;
                int c = str.IndexOf('\\') + 1;
                string str1 = "pack://" + str.Substring(0, c) + ",,," + str.Substring(c).Replace("bin\\Debug\\Terminal.exe", "Resources\\Дополнительно\\Иконки\\");
                for (int i = 0; i < files.Length; i++)
                {
                    bool flagImg = false, flag = true;
                    ImageSource aphoto = new BitmapImage(new Uri(@str1 + "file.jpg"));
                    string fileName = "";
                    int currChar = files[i].Length - 1;
                    int cn = files[i].LastIndexOf('\\');
                    while (currChar != cn)
                    {
                        fileName += files[i][currChar];
                        currChar--;
                        if (!flagImg && fileName.Length > 3)
                        {
                            if (fileName == "spx." || fileName == "SPX.")
                            {
                                flag = false;
                                break;
                            }
                            else if (fileName == "cod." || fileName == "xcod." || fileName == "COD." || fileName == "XCOD.")
                            {
                                aphoto = new BitmapImage(new Uri(@str1 + "doc.jpg"));
                                fileName = "";
                                flagImg = true;
                            }
                            else if (fileName == "slx." || fileName == "xslx." || fileName == "SLX." || fileName == "XSLX.")
                            {
                                aphoto = new BitmapImage(new Uri(@str1 + "xls.jpg"));
                                fileName = "";
                                flagImg = true;
                            }
                            else if (fileName == "gpj." || fileName == "gepj." || fileName == "GPJ." || fileName == "GEPJ.")
                            {
                                aphoto = new BitmapImage(new Uri(@str1 + "jpeg.jpg"));
                                fileName = "";
                                flagImg = true;
                            }
                            else if (fileName == "fdp." || fileName == "FDP.")
                            {
                                aphoto = new BitmapImage(new Uri(@str1 + "pdf.jpg"));
                                fileName = "";
                                flagImg = true;
                            }
                        }
                    }
                    fileName = reverseString(fileName);
                    if (flag)
                    {
                        notxps.Add(files[i]);
                        names.Add(fileName);
                        dataGrid.Items.Add(new ContactsInfo { photo = aphoto, filename = fileName });
                    }
                }
            }
            catch
            {
                dataGrid.Visibility = Visibility.Hidden;
                tblError.Visibility = Visibility.Visible;
            }
        }

        private void scroller_MouseDown(object sender, MouseButtonEventArgs e)
        {
            eraseTime();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            eraseTime();
        }

        private void dataGrid_MouseDown(object sender, MouseButtonEventArgs e)
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

        private void dataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (File.Exists(System.IO.Path.GetTempPath() + "forTerminal.pdf"))
                File.Delete(System.IO.Path.GetTempPath() + "forTerminal.pdf");
            var dg = sender as DataGrid;
            InfoDoc infodoc = new InfoDoc(notxps[dg.Items.IndexOf(dg.SelectedItem)], names[dg.Items.IndexOf(dg.SelectedItem)]);
            infodoc.Background = this.Background;
            infodoc.scroller_thing = scroller.VerticalOffset;
            infodoc.contentType = content;
            infodoc.Show();
            this.Close();
        }
    }
}
