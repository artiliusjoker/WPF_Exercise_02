using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace WpfApp
{
    class Question
    {
        public string QuestionText { get; set; }
        public string CorrectAnswer { get; set; }
        public string WrongAnswer{get; set;}
        // Khoi tao cau tra loi
        public Question(string countryName, string correctAnswer)
        {
            StringBuilder questionBuilder = new StringBuilder();
            questionBuilder.Append("Which is the flag of ");
            questionBuilder.Append(countryName);
            questionBuilder.Append(" ?");
            QuestionText = questionBuilder.ToString();
            CorrectAnswer = correctAnswer;
        }
        // Thiet lap cau tra loi sai

        // Kiem tra cau tra loi dung
        public bool IsCorrect(string answer)
        {
            int compareResult = CorrectAnswer.CompareTo(answer);
            return (compareResult == 0);
        }
        public void ConvertToAbsolutePath(string currFolder)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(currFolder);
            stringBuilder.Append("\\assets\\");
            stringBuilder.Append(CorrectAnswer);
            CorrectAnswer = stringBuilder.ToString();

            stringBuilder = new StringBuilder();
            stringBuilder.Append(currFolder);
            stringBuilder.Append("\\assets\\");
            stringBuilder.Append(WrongAnswer);
            WrongAnswer = stringBuilder.ToString();
        }
        public override string ToString()
        {
            return QuestionText;
        }
    }
}
