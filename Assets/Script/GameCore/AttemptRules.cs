using UnityEngine;

namespace JigokuBall.GameCore
{
    /// <summary>
    /// 単一プレイヤーセッションのルールを ScriptableObject で保持します。
    /// </summary>
    [CreateAssetMenu(fileName = "AttemptRules", menuName = "JigokuBall/AttemptRules")]
    public sealed class AttemptRules : ScriptableObject
    {
        [Header("投球回数の上限")]
        [Min(1)]
        [SerializeField] private int maxAttempts = 3;

        [Header("各投球の制限時間(秒)")]
        [Tooltip("0 の場合は制限時間なしです")]
        [Min(0f)]
        [SerializeField] private float attemptTimeoutSeconds = 0f;

        public int MaxAttempts => Mathf.Max(1, maxAttempts); // セッション内で許可する投球回数
        public float AttemptTimeoutSeconds => Mathf.Max(0f, attemptTimeoutSeconds); // 1 投あたりのタイムアウト秒数
        public bool HasAttemptTimeout => AttemptTimeoutSeconds > 0f; // タイムアウト設定が有効か
    }
}
