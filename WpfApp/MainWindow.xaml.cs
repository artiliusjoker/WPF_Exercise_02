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
        // Constant
        private readonly string CURR_FOLDER = AppDomain.CurrentDomain.BaseDirectory;
        // Danh sach cau hoi
        private readonly List<Question> questions = new List<Question>();
        // Cau hoi hien tai dang hien tren man hinh
        private int currQuestion;
        // Danh cho cac thao tac random
        private bool[] isSelectedArray = null;
        private readonly Random _random = new Random();

        public MainWindow()
        {
            InitializeComponent();
            StartGame();
        }
    private void InitializeQuestionList() {
            // Doc du lieu
            string[] lines = File.ReadAllLines(CURR_FOLDER + "AppData.txt");
            // Khoi tao mang danh dau
            isSelectedArray = new bool[lines.Length];
            for(int i = 0; i < isSelectedArray.Length; i++)
            {
                isSelectedArray[i] = false;
            }

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
            while (isSelectedArray[randomLine])
            {
                randomLine = _random.Next(lines.Length);
            }
            isSelectedArray[randomLine] = true;
            return randomLine;
        }
        private void StartGame() 
        {
            InitializeQuestionList();
            ShowQuestionOnScreen(questions[0]);
            currQuestion = 0;
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

        private void Back_Click_Button(object sender, RoutedEventArgs e)
        {
            if(currQuestion > 0)
            {
                currQuestion -= 1;
                ShowQuestionOnScreen(questions[currQuestion]);
            }
        }

        private void Next_Click_Button(object sender, RoutedEventArgs e)
        {
            if (currQuestion < questions.Count - 1)
            {
                currQuestion += 1;
                ShowQuestionOnScreen(questions[currQuestion]);
            }
        }
    }
   
}
