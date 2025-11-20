using UnityEngine;
using UnityEngine.UI;
using TMPro;
using HUD.Score;

public class ResultScoreViewer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _scoreText;

    

    public void SetScore(int score)
    {
        _scoreText.text = score.ToString();
    }

    public void ShowViewer(int score)
    {
        int currentScore = Score.GetInstance().ScoreNum;
        _scoreText.text = currentScore.ToString();
        gameObject.SetActive(true);
    }
}