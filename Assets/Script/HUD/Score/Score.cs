using System;
using UnityEngine;

namespace HUD.Score
{
    public class Score
    {
        public static Score Instance;
        private int scoreNum;
        public int ScoreNum => scoreNum;
        public event Action<int> OnScoreChanged;

        public static Score GetInstance()
        {
            if (Instance == null)
            {
                Instance = new Score();
                Instance.scoreNum = 0;
            }
            return Instance;
        }

        public void AddScore(int newScoreNum)
        {
            scoreNum += newScoreNum;
            UnityEngine.Debug.Log("Score Added: " + newScoreNum.ToString() + ", Total Score: " + scoreNum.ToString());
            OnScoreChanged?.Invoke(newScoreNum);
        }

        public void ResetScore()
        {
            scoreNum = 0;
        }
    }
}