using UnityEngine;
using HUD.Score;

public class HoleCollision : MonoBehaviour
{
    [SerializeField] private int scoreValue = 10;
    [SerializeField] private Score score;
    public void OnHit()
    {
        score.AddScore(scoreValue); // スコアを10点追加する例
    }
}