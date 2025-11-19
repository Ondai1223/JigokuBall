using UnityEngine;

namespace JigokuBall.GameCore
{
    /// <summary>
    /// セッション開始前の初期化と AttemptManager への依存注入を担います。
    /// </summary>
    public sealed class GameManager : MonoBehaviour
    {
        [SerializeField] private AttemptRules rules; // 使用するルールアセット
        [SerializeField] private AttemptManager attemptManager; // シーン内のセッション管理コンポーネント
        [SerializeField] private bool autoStartOnAwake = true; // Awake 後に自動で開始するかどうか

        /// <summary>現在使用中の AttemptManager。</summary>
        public AttemptManager AttemptManager => attemptManager;
        /// <summary>参照しているルールアセット。</summary>
        public AttemptRules Rules => rules;

        /// <summary>依存関係の解決と初期化を行います。</summary>
        private void Awake()
        {
            if (attemptManager == null)
            {
                attemptManager = GetComponentInChildren<AttemptManager>(true);
                if (attemptManager == null)
                {
                    attemptManager = FindFirstObjectByType<AttemptManager>(FindObjectsInactive.Include);
                }
            }

            if (attemptManager == null)
            {
                Debug.LogError("GameManager に AttemptManager の参照が設定されていません。");
                enabled = false;
                return;
            }

            if (rules == null)
            {
                Debug.LogError("GameManager に AttemptRules の参照が設定されていません。");
                enabled = false;
                return;
            }

            attemptManager.Initialize(rules);
        }

        /// <summary>初期化後に自動でセッションを開始します。</summary>
        private void Start()
        {
            if (!enabled)
            {
                return;
            }

            if (autoStartOnAwake)
            {
                try
                {
                    attemptManager.StartSession();
                }
                catch (System.Exception ex)
                {
                    Debug.LogError($"セッション開始時に例外が発生しました: {ex}");
                }
            }
        }

        /// <summary>セッションを再開します。</summary>
        public void RestartSession()
        {
            if (!enabled || attemptManager == null)
            {
                return;
            }

            attemptManager.StartSession(restart: true);
        }
    }
}
