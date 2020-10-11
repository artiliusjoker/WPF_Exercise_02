using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp
{
    class Player
    {
        public string PlayerName { get; set; }
        public bool[] AnswerArray = null;
        public int Score { get; set; }

        public Player(string playerName)
        {
            PlayerName = playerName;
            AnswerArray = new bool[10];
            Score = 0;
        }
        public Player(string playerName, int score)
        {
            Score = score;
            PlayerName = playerName;
        }
        public void AddScore()
        {
            Score += 1;
        }
    }
}
