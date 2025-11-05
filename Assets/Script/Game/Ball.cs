using HUD.Score;
using JigokuBall.GameCore;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public static System.Action OnHitHole; // 当たったことを知らせるイベント
    
    [SerializeField] private Vector3 initialPosition; // Inspector で固定可能な初期位置
    [SerializeField] Collider ballCollider;
    [SerializeField] ScoreObj score;

    [SerializeField] private AttemptManager attemptManager;
    

    void Awake()
    {
        // initialPosition が未設定（Vector3.zero）なら、現在位置を使用
        if (initialPosition == Vector3.zero)
        {
            initialPosition = transform.position;
        }

        if (ballCollider == null)
        {
            ballCollider = GetComponent<Collider>();
        }

        if (attemptManager == null)
        {
            attemptManager = FindFirstObjectByType<AttemptManager>(FindObjectsInactive.Include);
        }
    }

    void Update()
    {
        // デバッグ用：P キーで穴に入ったことをシミュレート
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.P))
        {
            DebugSimulateHoleHit();
        }
#endif
    }
    
    public void Reset()
    {
        transform.position = initialPosition;

        if (ballCollider != null)
        {
            ballCollider.isTrigger = false; // トリガーを無効にする
        }
    }

    /// <summary>
    /// デバッグ用：穴に入ったことをシミュレート（P キーで実行）
    /// </summary>
    private void DebugSimulateHoleHit()
    {
        if (attemptManager == null)
        {
            Debug.LogError("[Ball] AttemptManager が未設定です。デバッグシミュレーション失敗。");
            return;
        }

        Debug.Log("[Ball] デバッグ：穴に入ったことをシミュレートします（P キー）");
        
        // スコアを報告
        attemptManager.ReportScore(10);
        
        // 投球を完了させる
        attemptManager.ResolveAttempt(AttemptResolutionCause.SunkInHole);
        
        // ボールをリセット
        Reset();
    }

    /// <summary>
    /// Unity ライフサイクル：Collider がトリガーに進入したときに呼ばれます
    /// </summary>
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("HoleCollision"))
        {
            Debug.Log("穴の当たり判定に触れた！");
            var trigger = other.GetComponent<HoleTrigger>();
            trigger?.OnHit();
            Debug.Log($"[Ball] HoleTrigger.OnHit() が呼ばれました。AttemptManager={attemptManager?.name}");
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
