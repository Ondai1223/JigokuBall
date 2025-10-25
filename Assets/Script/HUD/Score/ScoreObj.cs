using UnityEngine;
using UnityEngine.UI;
using TMPro;
namespace HUD.Score
{
    public class ScoreObj : MonoBehaviour
    {
        [SerializeField] private Text scoreText;

        private void OnEnable()
        {
            Subscribe();
        }

        private void OnDisable()
        {
            Unsubscribe();
        }

        private void Subscribe()
        {
            Score score = Score.GetInstance();
            if (score == null)
            {
                Debug.LogWarning("ScoreObj: Score シングルトンが見つかりません。");
                return;
            }

            score.OnScoreChanged += HandleScoreChanged;
            UpdateScoreLabel(score.ScoreNum);
        }

        private void Unsubscribe()
        {
            Score score = Score.GetInstance();
            if (score == null)
            {
                return;
            }

            score.OnScoreChanged -= HandleScoreChanged;
        }

        private void HandleScoreChanged(int delta)
        {
            // Score シングルトンから現在値を取得
            int currentScore = Score.GetInstance().ScoreNum;
            UpdateScoreLabel(currentScore + delta);
        }

        private void UpdateScoreLabel(int value)
        {
            scoreText.text = Score.GetInstance().ScoreNum.ToString();
            Debug.Log("Score Updated: " + Score.GetInstance().ScoreNum.ToString());

        }
    }
}
