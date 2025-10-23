using JigokuBall.GameCore;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class HoleTrigger : MonoBehaviour
{
    [SerializeField] private int scoreValue = 10; // このホールで獲得するスコア
    [SerializeField] private AttemptManager attemptManager; // スコア報告先

    /// <summary>ホールに命中した際に呼び出されます。</summary>
    public void OnHit()
    {
        if (attemptManager == null)
        {
            attemptManager = FindFirstObjectByType<AttemptManager>(FindObjectsInactive.Include);
        }

        if (attemptManager == null)
        {
            Debug.LogWarning("HoleTrigger に AttemptManager が設定されていません。");
            return;
        }

        attemptManager.ReportScore(scoreValue);
    }
}
