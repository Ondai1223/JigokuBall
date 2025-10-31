using JigokuBall.GameCore;
using UnityEngine;
using UnityEngine.UI;

namespace HUD.Score
{
    /// <summary>
    /// 投球情報（何投目、残り投球数）を表示する UI
    /// </summary>
    public class AttemptInfoDisplay : MonoBehaviour
    {
        [SerializeField] private Text attemptInfoText; // 投球情報表示用テキスト
        [SerializeField] private GameManager gameManager; // AttemptManager にアクセスする

        private AttemptManager _attemptManager;

        private void Awake()
        {
            if (gameManager == null)
            {
                gameManager = FindFirstObjectByType<GameManager>(FindObjectsInactive.Include);
            }
        }

        private void OnEnable()
        {
            Subscribe();
        }

        private void OnDisable()
        {
            Unsubscribe();
        }

        private void Subscribe()
        {
            if (gameManager == null)
            {
                Debug.LogWarning("AttemptInfoDisplay に GameManager が設定されていません。");
                return;
            }

            _attemptManager = gameManager.AttemptManager;
            if (_attemptManager == null)
            {
                Debug.LogWarning("AttemptInfoDisplay: AttemptManager が初期化されていません。");
                return;
            }

            // セッション開始時に表示を更新
            _attemptManager.OnSessionStarted += HandleSessionStarted;
            // 投球開始時に表示を更新
            _attemptManager.OnAttemptStarted += HandleAttemptStarted;
            // 投球完了時に表示を更新
            _attemptManager.OnAttemptResolved += HandleAttemptResolved;
        }

        private void Unsubscribe()
        {
            if (_attemptManager == null)
            {
                return;
            }

            _attemptManager.OnSessionStarted -= HandleSessionStarted;
            _attemptManager.OnAttemptStarted -= HandleAttemptStarted;
            _attemptManager.OnAttemptResolved -= HandleAttemptResolved;
        }

        private void HandleSessionStarted(SessionInfo sessionInfo)
        {
            UpdateAttemptInfo();
        }

        private void HandleAttemptStarted(AttemptInfo attemptInfo)
        {
            UpdateAttemptInfo();
        }

        private void HandleAttemptResolved(AttemptResult attemptResult)
        {
            UpdateAttemptInfo();
        }

        private void UpdateAttemptInfo()
        {
            if (_attemptManager == null || attemptInfoText == null)
            {
                return;
            }

            // 残り投球数を表示
            int remainingAttempts = _attemptManager.Rules.MaxAttempts - _attemptManager.CurrentAttemptIndex;

            attemptInfoText.text = $"{remainingAttempts}";
        }
    }
}
