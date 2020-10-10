using System;
using System.IO;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

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
        private readonly List<Question> questions = new List<Question>(10);
        // Danh sach cau tra loi cho cau hoi
        private readonly bool[] answers = new bool[10];
        // Cau hoi hien tai dang hien tren man hinh
        private int currQuestion;
        // Danh cho cac thao tac random
        private bool[] isSelectedArray = null;
        private readonly Random _random = new Random();
        // Timer
        private readonly DispatcherTimer timer = new DispatcherTimer();

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
            // Tao 10 cau hoi
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
                // Chinh sua thanh duong dan tuyet doi
                question.ConvertToAbsolutePath(CURR_FOLDER);
                // Chon 1 vi tri ngau nhien trong 2 vi tri
                // true : vitri1
                // false : vitri2
                int randomizedPicturePosition = _random.Next(2);
                if (randomizedPicturePosition == 0)
                {
                    question.CorrectAnswerPosition = true;
                    question.WrongAnswerPosition = false;
                }
                else if (randomizedPicturePosition == 1)
                {
                    question.CorrectAnswerPosition = false;
                    question.WrongAnswerPosition = true;
                }
                // Luu lai cau tra loi dung
                answers[i] = question.CorrectAnswerPosition;
                // Them cau hoi hoan thien vao bo cau hoi
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
            // Dem thoi gian bat dau
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Tick += new EventHandler(TimerTick);
            timer.Start();
        }
        private void TimerTick(object sender, EventArgs e)
        {
            if (!questions[currQuestion].IsAnswered)
            {
                int timeToAnswer = questions[currQuestion].TimeToAnswer;
                timerOnScreen.Text = timeToAnswer.ToString();
                if (timeToAnswer == 0)
                {
                    questions[currQuestion].IsAnswered = true;
                }
                if (timeToAnswer > 0)
                {
                    questions[currQuestion].TimeToAnswer -= 1;
                }
            }       
        }
        private void ShowQuestionOnScreen(Question question)
        {
            // Dat vao cau hoi
            questionOnScreen.Content = question.QuestionText;
            // Dat vao cau tra loi dung
            BitmapImage bitmapImage = new BitmapImage(new Uri(question.CorrectAnswer, UriKind.Absolute));
            if(question.CorrectAnswerPosition)
            {
                picture1.Source = bitmapImage;
            }
            else picture2.Source = bitmapImage;       
            // Dat vao cau tra loi sai
            bitmapImage = new BitmapImage(new Uri(question.WrongAnswer, UriKind.Absolute));
            if (question.WrongAnswerPosition)
            {
                picture1.Source = bitmapImage;
            }
            else picture2.Source = bitmapImage;
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
