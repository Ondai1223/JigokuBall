using HUD.Score;
using JigokuBall.GameCore;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public static System.Action OnHitHole; // 当たったことを知らせるイベント
    
    private Vector3 initialPosition;
    [SerializeField] Collider ballCollider;
    [SerializeField] ScoreObj score;

    [SerializeField] private AttemptManager attemptManager;
    

    void Awake()
    {
        initialPosition = transform.position;

        if (ballCollider == null)
        {
            ballCollider = GetComponent<Collider>();
        }

        if (attemptManager == null)
        {
            attemptManager = FindFirstObjectByType<AttemptManager>(FindObjectsInactive.Include);
        }
    }

    
    public void Reset()
    {

        if (ballCollider != null)
        {
            ballCollider.isTrigger = false; // トリガーを無効にする
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("HoleCollision"))
        {
            Debug.Log("穴の当たり判定に触れた！");
            var trigger = other.GetComponent<HoleTrigger>();
            trigger?.OnHit();
        }

        if (other.CompareTag("Hole"))
        {
            Debug.Log("穴に落ちた！");
            if (ballCollider != null)
            {
                ballCollider.isTrigger = true;
            }

            OnHitHole?.Invoke();

            if (attemptManager == null)
            {
                attemptManager = FindFirstObjectByType<AttemptManager>(FindObjectsInactive.Include);
            }

            if (attemptManager != null)
            {
                attemptManager.ResolveAttempt(AttemptResolutionCause.SunkInHole);
            }
            else
            {
                Debug.LogWarning("AttemptManager が未設定のため ResolveAttempt を呼び出せません。");
            }
        }
    }
}
