using JigokuBall.GameCore;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
namespace HUD.Score
{
    public class ScoreObj : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI scoreText;
        [SerializeField] private GameManager gameManager; // ScoreService にアクセスする GameManager


        private ScoreService scoreService; // 現在購読しているスコアサービス

        private void Awake()
        {
            if (gameManager == null)
            {
                gameManager = FindFirstObjectByType<GameManager>(FindObjectsInactive.Include);
            }
        }

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
            if (gameManager == null)
            {
                Debug.LogWarning("ScoreObj に GameManager が設定されていません。");
                return;
            }

            scoreService = gameManager.ScoreService;
            if (scoreService == null)
            {
                Debug.LogWarning("ScoreObj: ScoreService が初期化されていません。");
                return;
            }

            scoreService.OnScoreChanged += HandleScoreChanged;
            UpdateScoreLabel(scoreService.CurrentScore);
        }

        private void Unsubscribe()
        {
            if (scoreService == null)
            {
                return;
            }

            scoreService.OnScoreChanged -= HandleScoreChanged;
        }

        private void HandleScoreChanged(ScoreChanged change)
        {

            UpdateScoreLabel(scoreService.CurrentScore);
        }

        private void UpdateScoreLabel(int value)
        {
            scoreText.text = value.ToString();
            Debug.Log("Score Updated: " + value.ToString());

        }
    }
}
