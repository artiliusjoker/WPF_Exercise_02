using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp
{
    class AppQuestion
    {
        // First Question la hinh nam trong button thu 1
        // Second Question la hinh nam trong button thu 2
        public Question FirstQuestion { get; set; }
        public Question SecondQuestion { get; set; }
        public Question CorrectQuestion { get; set; }
        public bool IsAnswered { get; set; }
        public int TimeToAnswer { get; set; }
        public AppQuestion()
        {          
            TimeToAnswer = 30;
            IsAnswered = false;
        }
    }
}
