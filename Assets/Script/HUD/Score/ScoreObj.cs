using UnityEngine;
using UnityEngine.UI;

namespace HUD.Score
{
    public class ScoreObj : MonoBehaviour
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
            scoreText.text = "Score: " + Score.GetInstance().ScoreNum.ToString();
            Debug.Log("Score Updated: " + Score.GetInstance().ScoreNum.ToString());
        }
    }
}