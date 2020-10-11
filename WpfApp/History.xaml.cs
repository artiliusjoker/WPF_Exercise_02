using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;

namespace WpfApp
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class HistoryWindow : Window
    {
        private readonly string CURR_FOLDER = AppDomain.CurrentDomain.BaseDirectory;

        public HistoryWindow()
        {
            InitializeComponent();
            ReadList();
        }
        public void ReadList()
        {
            var list = new ObservableCollection<Player>();
            string[] lines = File.ReadAllLines(CURR_FOLDER + "History.txt");
            for (int i = 0; i < lines.Length; i++)
            {
                string temp = lines[i];
                string[] words = temp.Split(':');
                Player player = new Player(words[0], Int32.Parse(words[1]));
                list.Add(player);
            }
            this.historyData.ItemsSource = list;
        }
    }
}
