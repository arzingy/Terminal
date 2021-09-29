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
using System.Globalization;
using System.Reflection;

namespace Terminal
{
    /// <summary>
    /// Interaction logic for Doska.xaml
    /// </summary>
    public partial class Doska : Window
    {
        private int timer = 0;
        private int userNum = 0;

        public Doska()
        {
            InitializeComponent();
            loadInfo();
            DispatcherTimer dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Start();
        }

        private void photoWithText(string path, string text)
        {
            // параметры -->

            var background = Brushes.SteelBlue;
            var textColor = Brushes.White;

            var gap = 10;
            var fontSize = 26;

            var dpi = 96;

            var font =
                new Typeface(
                    new FontFamily("Segoe UI"), FontStyles.Normal,
                    FontWeights.Bold, FontStretches.SemiExpanded);
            // <--

            var image = new BitmapImage(new Uri(@path));
            var imageWidth = (double)image.PixelWidth;
            var imageHeight = (double)image.PixelHeight;

            var formattedText =
                new FormattedText(
                    text, CultureInfo.CurrentCulture, FlowDirection.LeftToRight,
                    font, fontSize, textColor, dpi)
                {
                    MaxTextWidth = imageWidth,
                    TextAlignment = TextAlignment.Center
                };
            _ = formattedText.Width;
            var textHeight = formattedText.Height;

            var totalWidth = (int)Math.Ceiling(imageWidth + 2 * gap);
            var totalHeight = (int)Math.Ceiling(imageHeight + 3 * gap + textHeight);

            var drawingVisual = new DrawingVisual();
            using (var drawingContext = drawingVisual.RenderOpen())
            {
                drawingContext.DrawRectangle(
                    background, null,
                    new Rect(0, 0, totalWidth, totalHeight));

                drawingContext.DrawImage(
                    image,
                    new Rect(gap, gap, imageWidth, imageHeight));
                drawingContext.DrawText(
                    formattedText,
                    new Point(gap, imageHeight + 2 * gap));
            }

            var bmp =
                new RenderTargetBitmap(
                    totalWidth, totalHeight, dpi, dpi,
                    PixelFormats.Pbgra32);
            bmp.Render(drawingVisual);

            var encoder = new JpegBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bmp));
            path = path.Remove(0, 7).Replace(",,,", "\\");
            while (path[path.Length - 1] != '.')
                path = path.Remove(path.Length - 1);
            path = path.Remove(path.Length - 1);
            int i = path.Length - 1;
            for (; path[i] != '\\'; i--) ;
            string str3 = path.Substring(i);
            path = path.Remove(i);
            path += "\\Temp\\";
            path += str3;
            if (path.Substring(path.Length - 5) == "\\user")
                path += userNum;
            path += "_withtext.jpg";
            using (var stream = File.Create(@path))
                encoder.Save(stream);
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

        public class FiveImages
        {
            public ImageSource first { get; set; }
            public ImageSource second { get; set; }
            public ImageSource third { get; set; }
            public ImageSource fourth { get; set; }
            public ImageSource fifth { get; set; }
        }

        private void btBack_Click(object sender, RoutedEventArgs e)
        {
            dataGrid.Items.Clear();
            dataGrid.ItemsSource = null;
            dataGrid.Items.Refresh();
            Info mainwindow = new Info
            {
                Background = this.Background
            };
            mainwindow.Show();
            this.Close();
        }

        private void loadInfo()
        {
            scroller.Visibility = Visibility.Visible;
            dataGrid.Items.Clear();
            dataGrid.Items.Refresh();
            scroller.ScrollToVerticalOffset(0);
            string[] lines = File.ReadAllLines(@"..\..\Resources\Информация\Доска почета\Документы\doskaInfo.txt");
            string str = Assembly.GetExecutingAssembly().Location;
            int c = str.IndexOf('\\');
            string str1 = "pack://" + str.Substring(0, c) + ",,," + str.Substring(c).Replace("bin\\Debug\\Terminal.exe", "Resources\\Информация\\Доска почета\\Изображения\\");
            int linesCount = (lines.Length / 4);
            if (lines.Length % 4 != 0)
                linesCount++;
            int linesCounter = 0, photoCounter = 0;
            while (linesCounter < linesCount)
            {
                List<ImageSource> images = new List<ImageSource>() { };
                _ = new List<string>() { };
                for (; linesCounter < linesCount; linesCounter++)
                {
                    if (photoCounter == 5)
                        break;
                    ImageSource aphoto;
                    if (File.Exists(@"..\..\Resources\Информация\Доска почета\Изображения\" + lines[linesCounter * 4]))
                    {
                        string rash = lines[linesCounter * 4].Substring(lines[linesCounter * 4].LastIndexOf('.'));
                        lines[linesCounter * 4] = lines[linesCounter * 4].Remove(lines[linesCounter * 4].LastIndexOf('.'));
                        if (!File.Exists(@"..\..\Resources\Информация\Доска почета\Изображения\Temp\" + lines[linesCounter * 4] + "_withtext.jpg"))
                            photoWithText(@str1 + lines[linesCounter * 4] + rash, lines[linesCounter * 4 + 1] + "\n" + lines[linesCounter * 4 + 2]);
                        aphoto = new BitmapImage(new Uri(@str1 + "Temp\\" + lines[linesCounter * 4] + "_withtext.jpg"));
                    }
                    else
                    {
                        if (!File.Exists(@"..\..\Resources\Информация\Доска почета\Изображения\Temp\user" + userNum + "_withtext.jpg"))
                            photoWithText(@str1 + "user.jpg", lines[linesCounter * 4 + 1] + "\n" + lines[linesCounter * 4 + 2]);
                        aphoto = new BitmapImage(new Uri(@str1 + "Temp\\user" + userNum + "_withtext.jpg"));
                        userNum++;
                    }
                    images.Add(aphoto);
                    
                    photoCounter++;
                }
                if (photoCounter != 5)
                    for (; photoCounter != 5; photoCounter++)
                        images.Add(new BitmapImage(new Uri("", UriKind.Relative)));
                dataGrid.Items.Add(new FiveImages { first = images[0], second = images[1], third = images[2], fourth = images[3], fifth = images[4] });
                photoCounter = 0;
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
