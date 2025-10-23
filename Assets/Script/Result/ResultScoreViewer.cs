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
        _scoreText.text = Score.GetInstance().ScoreNum.ToString();
    }
}