using System.Windows;

namespace WpfApp
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class PromptDialog : Window
    {
        public PromptDialog(string question)
        {
            InitializeComponent();
            txtQuestion.Text = question;
        }

        public string Text
        {
            get
            {
                return txtResponse.Text;
            }
        }

        public static string ShowAndGetInput(string question)
        {
            PromptDialog prompt = new PromptDialog(question);
            prompt.ShowDialog();
            if (prompt.DialogResult == true)
                return prompt.Text;
            return null;
        }

        private void BtnOk_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
