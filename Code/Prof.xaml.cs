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

namespace Terminal
{
    /// <summary>
    /// Interaction logic for Prof.xaml
    /// </summary>
    public partial class Prof : Window
    {

        private int time = 0;

        public Prof()
        {
            InitializeComponent();
            string docName = @"..\..\Resources\Информация\Профсоюзы\Деятельность.docx";
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
                    docName = docName.Remove(docName.LastIndexOf('\\') + 1) + "Деятельность.pdf";
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
                docName = @"..\..\Resources\Информация\Профсоюзы\Деятельность.pdf";
                string str1 = Assembly.GetExecutingAssembly().Location;
                str1 = str1.Replace("bin\\Debug\\Terminal.exe", docName.Remove(0, 6));
                string str = System.IO.Path.GetTempPath() + "forTerminal.pdf";
                File.Copy(str1, str);
                wb.Navigate(@str);
                File.Delete(@"..\..\Resources\Информация\Профсоюзы\Деятельность.docx");
            }
            catch
            {
                wb.Visibility = Visibility.Hidden;
            }
            DispatcherTimer dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Start();
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            if (wb.IsVisible || quitter.IsVisible)
            {
                time++;
                if (time >= 180 && time <= 240)
                {
                    if (time == 180)
                    {
                        btBack.IsEnabled = false;
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

        private void btBack_Click(object sender, RoutedEventArgs e)
        {
            wb.Dispose();
            Info info = new Info();
            info.Background = this.Background;
            info.Show();
            this.Close();
        }

        private void btHome_Click(object sender, RoutedEventArgs e)
        {
            wb.Dispose();
            MainWindow mainwindow = new MainWindow();
            mainwindow.Show();
            this.Close();
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

        private void radbtSertificates_Checked(object sender, RoutedEventArgs e)
        {
            wb.Dispose();
            InfoChooser infochooser = new InfoChooser(8, 0);
            infochooser.Background = this.Background;
            infochooser.Show();
            this.Close();
        }

        private void radbtObjects_Checked(object sender, RoutedEventArgs e)
        {
            wb.Dispose();
            InfoChooser infochooser = new InfoChooser(9, 0);
            infochooser.Background = this.Background;
            infochooser.Show();
            this.Close();
        }

        private void radbtReviews_Checked(object sender, RoutedEventArgs e)
        {
            wb.Dispose();
            News news = new News(0, false);
            news.Background = this.Background;
            news.Show();
            this.Close();
        }
    }
}
