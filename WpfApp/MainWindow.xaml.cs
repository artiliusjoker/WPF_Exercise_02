using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Media.Imaging;

namespace WpfApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly string CURR_FOLDER = AppDomain.CurrentDomain.BaseDirectory;
        private readonly List<Question> questions = new List<Question>();

        // Danh cho cac thao tac random
        private readonly List<int> chosenIndex = new List<int>();
        private readonly Random _random = new Random();

        public MainWindow()
        {

            InitializeComponent();
            InitializeGame();
        }
    private void InitializeQuestionList() {
            string[] lines = File.ReadAllLines(CURR_FOLDER + "AppData.txt");
            
            for(int i = 0; i < 10; i++)
            {             
                // Chon ngau nhien cau hoi
                string line = lines[RandomizeLine(lines)];
                string[] temp = line.Split(':');
                Question question = new Question(temp[0], temp[1]);
                // Chon ngau nhien cau tra loi sai de danh lac huong
                line = lines[RandomizeLine(lines)];
                temp = line.Split(':');
                question.WrongAnswer = temp[1];
                // Them cau hoi vao bo cau hoi

                question.ConvertToAbsolutePath(CURR_FOLDER);
                questions.Add(question);
            }
        }
        private int RandomizeLine(string[] lines)
        {
            int randomLine = _random.Next(lines.Length);
            while (chosenIndex.Exists(x => x == randomLine))
            {
                randomLine = _random.Next(lines.Length);
            }
            return randomLine;
        }
        private void InitializeGame() 
        {
            InitializeQuestionList();
            ShowQuestionOnScreen(questions[0]);
        }
        private void ShowQuestionOnScreen(Question question)
        {
            questionOnScreen.Content = question.QuestionText;
            bool flag = true;                   
            BitmapImage bitmapImage = new BitmapImage(new Uri(question.CorrectAnswer, UriKind.Absolute));
        
            int randomizedPicturePosition = _random.Next(2);
            // Dat vao cau tra loi dung
            if(randomizedPicturePosition == 0)
            {
                picture1.Source = bitmapImage;              
            }
            else if (randomizedPicturePosition == 1)
            {
                flag = false;
                picture2.Source = bitmapImage;              
            }
            // Dat vao cau tra loi sai
            bitmapImage = new BitmapImage(new Uri(question.WrongAnswer, UriKind.Absolute));
            if(flag)
            {
                picture2.Source = bitmapImage;
            }
            else if(!flag)
            {
                picture1.Source = bitmapImage;
            }
        }
    }
   
}
