using UnityEngine;
using UnityEngine.UI;
using TMPro;
using HUD.Score;

public class ResultScoreViewer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _scoreText;

    void Start()
    {
        SetScore();
    }

    public void SetScore()
    {
        int currentScore = Score.GetInstance().ScoreNum;
        _scoreText.text = currentScore.ToString();
    }
}