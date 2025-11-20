using UnityEngine;
using UnityEngine.UI;
using TMPro;
namespace HUD.Score
{
    public class ScoreObj : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI scoreText;

        // private void OnEnable()
        // {
        //     Subscribe();
        // }

        // private void OnDisable()
        // {
        //     Unsubscribe();
        // }

        private void Subscribe()
        {
            Score score = Score.GetInstance();
            if (score == null)
            {
                Debug.LogWarning("ScoreObj: Score シングルトンが見つかりません。");
                return;
            }

            score.OnScoreChanged += HandleScoreChanged;
            UpdateScoreLabel(Score.Instance.ScoreNum);
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

        public void UpdateScoreLabel(int value)
        {
            if (scoreText == null)
            {
                Debug.LogWarning("ScoreObj: scoreText が設定されていません。");
                return;
            }
            scoreText.text = value.ToString();
            Debug.Log("Score Updated: " + value.ToString());
        }
    }
}
