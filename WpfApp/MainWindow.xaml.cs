using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace WpfApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            string currFolder = AppDomain.CurrentDomain.BaseDirectory;
            string picLocation = currFolder + "vietnam.png";
            BitmapImage bitmapImage = new BitmapImage(new Uri(picLocation, UriKind.Absolute));
            picture1.Source = bitmapImage;
            picLocation = currFolder + "argentina.png";
            bitmapImage = new BitmapImage(new Uri(picLocation, UriKind.Absolute));
            picture2.Source = bitmapImage;
            question.Content = "asdasd";
        }
    }
}
