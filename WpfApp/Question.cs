using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace WpfApp
{
    class Question
    {
        public string QuestionText { get; set; }
        public string Answer { get; set; }
        public bool Position { get; set; }
        public bool IsChosen { get; set; }
        // Khoi tao cau tra loi      
        public Question(string countryName, string answer)
        {
            StringBuilder questionBuilder = new StringBuilder();
            questionBuilder.Append("Which is the flag of ");
            questionBuilder.Append(countryName);
            questionBuilder.Append(" ?");
            QuestionText = questionBuilder.ToString();
            Answer = answer;
            IsChosen = false;
        }
        // Chuyen thanh dia chi tuyet doi
        public void ConvertToAbsolutePath(string currFolder)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(currFolder);
            stringBuilder.Append("\\assets\\");
            stringBuilder.Append(Answer);
            Answer = stringBuilder.ToString();
        }        
        public override string ToString()
        {
            return QuestionText;
        }
    }
}
