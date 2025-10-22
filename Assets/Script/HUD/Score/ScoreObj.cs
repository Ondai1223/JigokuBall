using UnityEngine;
using UnityEngine.UI;
using TMPro;
namespace HUD.Score
{
    public class ScoreObj : MonoBehaviour
    {
        private int scoreNum = 0;
        [SerializeField] private TextMeshProUGUI scoreText;
        [SerializeField] private int scoreRate = 1;

        void Start()
        {
            scoreText.text = "0";
        }

        public void UpdateScore()
        {
            scoreText.text = Score.GetInstance().ScoreNum.ToString();
            Debug.Log("Score Updated: " + Score.GetInstance().ScoreNum.ToString());
        }
    }
}