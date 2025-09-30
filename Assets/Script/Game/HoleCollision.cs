using UnityEngine;
using HUD.Score;

public class HoleCollision : MonoBehaviour
{
    [SerializeField] private int scoreValue = 10;
    public void OnHit()
    {
        Score.GetInstance().AddScore(scoreValue);
    }
}