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
    /// Логика взаимодействия для InfoDoc.xaml
    /// </summary>
    public partial class InfoDoc : Window
    {
        private int time = 0;
        public int contentType;
        public double scroller_thing = -1;

        public InfoDoc(string filePath, string fileName)
        {
            InitializeComponent();
            loadDoc(filePath);
            header.Content = fileName;
            DispatcherTimer dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Start();
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            if (wb.IsVisible || tblError.IsVisible || quitter.IsVisible)
            {
                time++;
                if (time >= 60 && time <= 240)
                {
                    if (time == 60)
                    {
                        btBack.IsEnabled = false;
                        wb.IsEnabled = false;
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
            wb.Visibility = Visibility.Hidden;
            if (scroller_thing != -1)
            {
                InfoChooser infochooser = new InfoChooser(contentType, scroller_thing);
                infochooser.Background = this.Background;
                infochooser.Show();
            }
            else
            {
                Pr infochooser = new Pr();
                infochooser.Background = this.Background;
                infochooser.Show();
            }
            this.Close();
        }

        private void loadDoc(string fileName)
        {
            wb.Visibility = Visibility.Visible;
            string xps = @fileName;
            try
            {
                if (xps[xps.Length - 1] == 'c' || xps[xps.Length - 2] == 'c' || xps[xps.Length - 1] == 'C' || xps[xps.Length - 2] == 'C')
                {
                    WordDocument wordDocument;
                    if (xps[xps.Length - 1] == 'c' || xps[xps.Length - 1] == 'C')
                        wordDocument = new WordDocument(xps, FormatType.Doc);
                    else
                        wordDocument = new WordDocument(xps, FormatType.Docx);
                    DocToPDFConverter converter = new DocToPDFConverter();
                    string str3 = Assembly.GetExecutingAssembly().Location;
                    str3 = str3.Remove(str3.Length - 22, 22);
                    string str4 = str3;
                    str3 += xps.Remove(0, 6);
                    string xps1 = xps.Remove(xps.LastIndexOf('\\') + 1) + "temp" + xps.Remove(0, xps.LastIndexOf('\\'));
                    str4 += xps1.Remove(0, 6);
                    File.Move(str3, str4);
                    xps = xps.Remove(xps.LastIndexOf('.')) + ".pdf";
                    PdfDocument pdfDocument = converter.ConvertToPDF(wordDocument);
                    pdfDocument.Save(xps);
                    converter.Dispose();
                    pdfDocument.Close(true);
                    wordDocument.Close();
                    string xps8 = xps;
                    for (int i = xps.Length - 1; xps[i] != '\\'; i--)
                        xps = xps.Remove(i);
                    xps += "forTerminal.pdf";
                    File.Copy(xps8, xps);
                }
                else
                {
                    string xps1 = xps;
                    for (int i = xps.Length - 1; xps[i] != '\\'; i--)
                        xps = xps.Remove(i);
                    xps += "forTerminal.pdf";
                    File.Copy(xps1, xps);
                }
                string str1 = Assembly.GetExecutingAssembly().Location;
                str1 = str1.Remove(str1.Length - 22, 22);
                str1 += xps.Remove(0, 6);
                string str = System.IO.Path.GetTempPath() + "forTerminal.pdf";
                File.Move(str1, str);
                wb.Navigate(@str);
            }
            catch
            {
                wb.Visibility = Visibility.Hidden;
                tblError.Visibility = Visibility.Visible;
            }
        }

        private void btHome_Click(object sender, RoutedEventArgs e)
        {
            wb.Dispose();
            wb.Visibility = Visibility.Hidden;
            MainWindow mainwindow = new MainWindow();
            mainwindow.Show();
            this.Close();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            eraseTime();
        }

        private void documentViewer_TouchDown(object sender, TouchEventArgs e)
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
            wb.IsEnabled = true;
            wb.Visibility = Visibility.Visible;
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

        private void wb_MouseDown(object sender, MouseButtonEventArgs e)
        {
            eraseTime();
        }

        private void scroller_MouseDown(object sender, MouseButtonEventArgs e)
        {
            eraseTime();
        }
    }
}
