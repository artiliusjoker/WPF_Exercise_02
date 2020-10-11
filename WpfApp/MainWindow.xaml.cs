using System;
using System.IO;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using System.Windows.Media;
using System.Diagnostics;

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
        private readonly List<AppQuestion> questions = new List<AppQuestion>(10);
        // Danh sach cau tra loi cho cau hoi
        private readonly bool[] answers = new bool[10];
        // Cau hoi hien tai dang hien tren man hinh
        private int currQuestion;
        // Danh cho cac thao tac random
        private bool[] isSelectedArray = null;
        private readonly Random _random = new Random();
        // Timer
        private readonly DispatcherTimer timer = new DispatcherTimer();
        // Nguoi choi
        private readonly Player player = new Player("DefaultPlayer"); 

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
            AppQuestion newQuestion = new AppQuestion();
            // Chon ngau nhien hinh dung
            string line = lines[RandomizeLine(lines)];
            string[] temp = line.Split(':');
            Question question1 = new Question(temp[0], temp[1]);
            question1.ConvertToAbsolutePath(CURR_FOLDER);

            // Chon ngau nhien hinh sai de danh lac huong
            line = lines[RandomizeLine(lines)];
            temp = line.Split(':');
            Question question2 = new Question(temp[0], temp[1]);
            question2.ConvertToAbsolutePath(CURR_FOLDER);

            // Chon 1 vi tri ngau nhien trong 2 vi tri
            // true : vitri1
            // false : vitri2
            int randomizedPicturePosition = _random.Next(2);
            if (randomizedPicturePosition == 0)
            {
                question1.Position = true;
                question2.Position = false;
                newQuestion.FirstQuestion = question1;
                newQuestion.SecondQuestion = question2;
            }
            else if (randomizedPicturePosition == 1)
            {
                question1.Position = false;
                question2.Position = true;
                newQuestion.FirstQuestion = question2;
                newQuestion.SecondQuestion = question1;
            }
            // Luu lai cau tra loi dung
            answers[i] = question1.Position;

            // Them cau hoi hoan thien vao bo cau hoi
            newQuestion.CorrectQuestion = question1;
            questions.Add(newQuestion);
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
            DisplayQuestionOnScreen(questions[0]);
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
                    NextQuestion();
                }
                if (timeToAnswer > 0)
                {
                    questions[currQuestion].TimeToAnswer -= 1;
                }
            }       
        }
        private void DisplayQuestionOnScreen(AppQuestion question)
        {
            // Dat vao thu tu cau hoi
            currentQuestionIndex.Text = (currQuestion+1).ToString() + "/10";

            // Dat vao cau hoi
            questionOnScreen.Content = question.CorrectQuestion.QuestionText;
            // Dat vao cau tra loi thu nhat
            BitmapImage bitmapImage = new BitmapImage(new Uri(question.FirstQuestion.Answer, UriKind.Absolute));
            picture1.Source = bitmapImage;
            // Dat vao cau tra loi thu hai
            bitmapImage = new BitmapImage(new Uri(question.SecondQuestion.Answer, UriKind.Absolute));
            picture2.Source = bitmapImage;
            // Tra loi roi khong cho sua lai
            if (question.IsAnswered)
            {
                picture1_button.IsEnabled = false;
                picture2_button.IsEnabled = false;
            }
        }
        private void NextQuestion()
        {           
            if (currQuestion < questions.Count - 1)
            {
                currQuestion += 1;
                DisplayQuestionOnScreen(questions[currQuestion]);
            }
            // reset lai button
            if (!questions[currQuestion].IsAnswered)
            {
                picture1_button.IsEnabled = true;
                picture2_button.IsEnabled = true;
            }
        }

        private void Back_Click_Button(object sender, RoutedEventArgs e)
        {
            if(currQuestion > 0)
            {
                currQuestion -= 1;
                DisplayQuestionOnScreen(questions[currQuestion]);
            }
        }

        private void Next_Click_Button(object sender, RoutedEventArgs e)
        {
            NextQuestion();
        }

        private void Picture1_button_Click(object sender, RoutedEventArgs e)
        {
            player.AnswerArray[currQuestion] = true;
            questions[currQuestion].IsAnswered = true;
            questions[currQuestion].FirstQuestion.IsChosen = true;
            if (currQuestion == (questions.Count - 1))
            {
                SubmitAndCalculateScore();
                return;
            }          
            NextQuestion();
        }

        private void Picture2_button_Click(object sender, RoutedEventArgs e)
        {
            player.AnswerArray[currQuestion] = false;
            questions[currQuestion].IsAnswered = true;
            questions[currQuestion].SecondQuestion.IsChosen = true;
            if (currQuestion == (questions.Count - 1))
            {
                SubmitAndCalculateScore();
                return;
            }
            NextQuestion();
        }

        private void ShowCurrentPlayerScore() 
        {
            string message = "Your score is " + player.Score.ToString();
            const string caption = "Result";
            MessageBox.Show(message, caption,
                            MessageBoxButton.OK,
                            MessageBoxImage.Information);
        }

        private void SubmitAndCalculateScore()
        {
            const string message = "Are you sure that you would like to submit your answers?";
            const string caption = "Submit";
            var result = MessageBox.Show(message, caption,
                                         MessageBoxButton.YesNo,
                                         MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                for (int i = 0; i < questions.Count; i++)
                {
                    // Chi tinh nhung cau nao da tra loi
                    if (questions[i].IsAnswered)
                    {
                        if (answers[i] == player.AnswerArray[i])
                        {
                            player.AddScore();
                        }
                    }
                }
            }
            ShowCurrentPlayerScore();
            string input = PromptDialog.ShowAndGetInput("Please input your name");
            if(input != null)
            {
                player.PlayerName = input;
            }
            player.SaveToFile("History.txt");
            Stop();
        }

        private void SubmitButon_Click(object sender, RoutedEventArgs e)
        {
            SubmitAndCalculateScore();
        }
        private void Stop()
        {
            SubmitButon.IsEnabled = false;
            BackButton.IsEnabled = false;
            NextButton.IsEnabled = false;
            picture1_button.IsEnabled = false;
            picture2_button.IsEnabled = false;
            timer.Stop();
        }

        private void HistoryButton_Click(object sender, RoutedEventArgs e)
        {
            HistoryWindow historyWindow = new HistoryWindow();
            historyWindow.Show();
        }
    }
}
