using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
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

namespace Terminal
{
    /// <summary>
    /// Interaction logic for Starter.xaml
    /// </summary>
    public partial class Starter : Window
    {
        public Starter()
        {
            InitializeComponent();
            string[] files = Directory.GetFiles(@"..\..\Resources\Информация\Доска почета\Изображения\Temp");
            for (int i = 0; i < files.Length; i++)
                File.Delete(files[i]);
            files = Directory.GetFiles(@"..\..\Resources\Об организации\Изображения\Объекты\Temp");
            for (int i = 0; i < files.Length; i++)
                File.Delete(files[i]);
            MainWindow mw = new MainWindow();
            mw.Show();
            this.Close();
        }
    }
}
