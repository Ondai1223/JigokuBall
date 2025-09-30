using UnityEngine;
using HUD.Score;

public class Ball : MonoBehaviour
{
    public static System.Action OnHitHole; // 当たったことを知らせるイベント

    private Vector3 initialPosition;
    [SerializeField] Collider ballCollider;
    [SerializeField] ScoreObj score;
    void Awake()
    {
        initialPosition = transform.position;
    }

    public void Reset()
    {
        transform.position = initialPosition;
        ballCollider.isTrigger = false; // トリガーを無効にする
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("HoleCollision"))
        {
            Debug.Log("穴の当たり判定に触れた！");
            // イベントを発生させる
            other.GetComponent<HoleCollision>().OnHit();
            score.UpdateScore();
        }
        if (other.CompareTag("Hole"))
        {
            Debug.Log("穴に落ちた！");
            
            ballCollider.isTrigger = true;

            // ボールが落ちる目標地点（穴の中心）を設定
            //transform.position = other.transform.position;

            // イベントを発生させる
            if (OnHitHole != null)
            {
                OnHitHole();
            }
        }
    }
}