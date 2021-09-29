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
using System.Windows.Controls.Primitives;
using System.ComponentModel;

namespace Terminal
{
    /// <summary>
    /// Логика взаимодействия для About.xaml
    /// </summary>
    public partial class About : Window
    {

        private int time = 0;
        private string[] files;

        public About(bool o, double scroller)
        {
            InitializeComponent();
            if (!o)
            {
                string docName = @"..\..\Resources\Об организации\Документы\Об организации.docx";
                    if (File.Exists(docName))
                {
                    try
                    {
                        WordDocument wordDocument;
                        if (docName[docName.Length - 1] == 'c' || docName[docName.Length - 1] == 'C')
                            wordDocument = new WordDocument(docName, FormatType.Doc);
                        else
                            wordDocument = new WordDocument(docName, FormatType.Docx);
                        DocToPDFConverter converter = new DocToPDFConverter();
                        string str3 = Assembly.GetExecutingAssembly().Location;
                        str3 = str3.Remove(str3.Length - 22, 22);
                        string str4 = str3;
                        str3 += docName.Remove(0, 6);
                        string xps1 = docName.Remove(docName.LastIndexOf('\\') + 1) + "temp" + docName.Remove(0, docName.LastIndexOf('\\'));
                        str4 += xps1.Remove(0, 6);
                        File.Move(str3, str4);
                        docName = docName.Remove(docName.LastIndexOf('\\') + 1) + "Об организации.pdf";
                        PdfDocument pdfDocument = converter.ConvertToPDF(wordDocument);
                        pdfDocument.Save(docName);
                        converter.Dispose();
                        pdfDocument.Close(true);
                        wordDocument.Close();
                    }
                    catch
                    {
                        wb.Visibility = Visibility.Hidden;
                    }
                }
                try
                {
                    docName = @"..\..\Resources\Об организации\Документы\Об организации.pdf";
                    string str1 = Assembly.GetExecutingAssembly().Location;
                    str1 = str1.Replace("bin\\Debug\\Terminal.exe", docName.Remove(0, 6));
                    string str = System.IO.Path.GetTempPath() + "forTerminal.pdf";
                    File.Copy(str1, str);
                    wb.Navigate(@str);
                    File.Delete(@"..\..\Resources\Об организации\Документы\Об организации.docx");
                }
                catch
                {
                    wb.Visibility = Visibility.Hidden;
                }
            }
            else
            {
                wb.Visibility = Visibility.Hidden;
                radbtObjects.IsChecked = true;
                ObjectsLoad(scroller);
            }
            DispatcherTimer dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Start();
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            if (wb.IsVisible || quitter.IsVisible || dataGrid.IsVisible)
            {
                time++;
                if (time >= 180 && time <= 240)
                {
                    if (time == 180)
                    {
                        btBack.IsEnabled = false;
                        scroller.IsEnabled = false;
                        dataGrid.IsEnabled = false;
                        wb.IsEnabled = false;
                        radbtObjects.IsEnabled = false;
                        radbtReviews.IsEnabled = false;
                        radbtSertificates.IsEnabled = false;
                        wb.Visibility = Visibility.Hidden;
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

        public class FourImages
        {
            public ImageSource first { get; set; }
            public ImageSource second { get; set; }
            public ImageSource third { get; set; }
            public ImageSource fourth { get; set; }
        }

        private void btBack_Click(object sender, RoutedEventArgs e)
        {
            wb.Dispose();
            MainWindow mainwindow = new MainWindow();
            mainwindow.Show();
            this.Close();
        }

        private void radbtSertificates_Checked(object sender, RoutedEventArgs e)
        {
            dataGrid.IsEnabled = false;
            loadPhotos("Сертификаты");
        }

        private void radbtReviews_Checked(object sender, RoutedEventArgs e)
        {
            dataGrid.IsEnabled = false;
            loadPhotos("Отзывы");
        }

        private void radbtObjects_Checked(object sender, RoutedEventArgs e)
        {
            ObjectsLoad(0);
        }

        private void loadPhotos(string folderName)
        {
            wb.Visibility = Visibility.Hidden;
            scroller.Visibility = Visibility.Visible;
            dataGrid.Items.Clear();
            dataGrid.Items.Refresh();
            scroller.ScrollToVerticalOffset(0);
            files = Directory.GetFiles(@"..\..\Resources\Об организации\Изображения\" + folderName);
            int fileCount = files.Length, gridCount = fileCount / 4;
            if (fileCount % 4 != 0)
                gridCount++;
            for (int i = 0; i < gridCount; i++)
            {
                List<ImageSource> images = new List<ImageSource>() { };
                for (int j = 0; j < 4; j++)
                    if (i * 4 + j < fileCount)
                    {
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
                        str += files[i * 4 + j].Remove(0, 6);
                        images.Add(new BitmapImage(new Uri(@str)));
                    }
                    else
                        images.Add(new BitmapImage(new Uri("", UriKind.Relative)));
                dataGrid.Items.Add(new FourImages { first = images[0], second = images[1], third = images[2], fourth = images[3] });
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
            if (wb.IsVisible)
                wb.IsEnabled = true;
            radbtObjects.IsEnabled = true;
            radbtReviews.IsEnabled = true;
            radbtSertificates.IsEnabled = true;
            wb.Visibility = Visibility.Visible;
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

        private void documentViewer_TouchDown(object sender, MouseButtonEventArgs e)
        {
            eraseTime();
        }

        private void radbtSertificates_MouseDown(object sender, MouseButtonEventArgs e)
        {
            eraseTime();
        }

        private void radbtReviews_MouseDown(object sender, MouseButtonEventArgs e)
        {
            eraseTime();
        }

        private void radbtObjects_MouseDown(object sender, MouseButtonEventArgs e)
        {
            eraseTime();
        }

        private void tbBorder_MouseDown(object sender, MouseButtonEventArgs e)
        {
            eraseTime();
        }

        private void photoWithText(string path, string text, int userNum)
        {
            // параметры -->
            var background = Brushes.White;
            var textColor = Brushes.Black;

            var gap = 10;
            //var fontSize = 60;

            var dpi = 96;

            var font =
                new Typeface(
                    new FontFamily("Segoe UI"), FontStyles.Normal,
                    FontWeights.Bold, FontStretches.SemiExpanded);
            // <--

            var image = new BitmapImage(new Uri(@path));
            var imageWidth = (double)image.PixelWidth;
            var fontSize = (int)(imageWidth / 20);
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
                    new Point(gap, imageHeight + 3 * gap));
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
            path += ".jpg";
            using (var stream = File.Create(@path))
                encoder.Save(stream);
        }

        void b1SetColor(object sender, RoutedEventArgs e)
        {
            if (radbtObjects.IsChecked.Value)
            {
                DependencyObject dep = (DependencyObject)e.OriginalSource;

                // iteratively traverse the visual tree
                while ((dep != null) &&
                !(dep is DataGridCell) &&
                !(dep is DataGridColumnHeader))
                {
                    dep = VisualTreeHelper.GetParent(dep);
                }

                if (dep == null)
                    return;

                if (dep is DataGridColumnHeader)
                {
                    // do something
                }

                if (dep is DataGridCell)
                {
                    DataGridCell cell = dep as DataGridCell;

                    // navigate further up the tree
                    while ((dep != null) && !(dep is DataGridRow))
                    {
                        dep = VisualTreeHelper.GetParent(dep);
                    }

                    wb.Dispose();
                    DataGridRow row = dep as DataGridRow;
                    int r = FindRowIndex(row);
                    int co = cell.Column.DisplayIndex;
                    img contacts = new img(4 * r + co)
                    {
                        st = scroller.VerticalOffset,
                        Background = this.Background
                    };
                    contacts.Show();
                    this.Close();
                }
            }
        }

        private int FindRowIndex(DataGridRow row)
        {
            DataGrid dataGrid =
                ItemsControl.ItemsControlFromItemContainer(row)
                as DataGrid;

            int index = dataGrid.ItemContainerGenerator.
                IndexFromContainer(row);

            return index;
        }

        private void ObjectsLoad(double scroller_thing)
        {
            dataGrid.IsEnabled = true;
            int userNum = 0;
            string[] lines = File.ReadAllLines(@"..\..\Resources\Об организации\Документы\Объекты.txt");
            string str = Assembly.GetExecutingAssembly().Location;
            int c = str.IndexOf('\\');
            string str1 = "pack://" + str.Substring(0, c) + ",,," + str.Substring(c).Replace("bin\\Debug\\Terminal.exe", "Resources\\Об организации\\Изображения\\Объекты\\");
            int i = 0;
            while (i < lines.Length - 1)
            {
                    if (File.Exists(@"..\..\Resources\Об организации\Изображения\Объекты\" + lines[i]))
                    {
                        if (!File.Exists(@"..\..\Resources\Об организации\Изображения\Объекты\Temp\" + lines[i]))
                            photoWithText(@str1 + lines[i], lines[i + 1], userNum);
                    }
                    //else
                    //{
                    //    if (!File.Exists(@"..\..\Resources\Об организации\Изображения\Объекты\Temp\default" + userNum))
                    //        photoWithText(@str1 + "default.jpg", lines[i + 1], userNum);
                    //    userNum++;
                    //}
                    i += 2;
                    while (i < lines.Length - 1)
                    {
                        if (lines[i] != "")
                            i++;
                        else
                            break;
                    }
                    i++;
            }
            loadPhotos("Объекты\\Temp");
            scroller.ScrollToVerticalOffset(scroller_thing);
        }

    }
}
