using UnityEngine;
using UnityEngine.UI;

namespace HUD.Score
{
    public class Score : MonoBehaviour
    {
        private int scoreNum = 0;
        [SerializeField] private Text scoreText;
        [SerializeField] private int scoreRate = 1;

        void Start()
        {
            scoreText.text = "Score: 0";
        }

        public void UpdateScore()
        {
            scoreText.text = "Score: " + scoreNum.ToString();
        }

        public void AddScore(int newScoreNum)
        {
            scoreNum += newScoreNum;
            UpdateScore();
        }
    }
}