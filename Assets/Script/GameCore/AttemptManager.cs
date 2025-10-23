using System;
using UnityEngine;

namespace JigokuBall.GameCore
{
    /// <summary>
    /// 単一プレイヤーの投球セッションを制御し、イベント通知を一元化します。
    /// </summary>
    public sealed class AttemptManager : MonoBehaviour
    {
        [SerializeField] private AttemptRules rules; // セッションのルール定義

        private ScoreService _scoreService; // スコア集計サービス
        private bool _sessionActive; // セッションが進行中か
        private bool _attemptActive; // 現在投球処理中か
        private int _attemptIndex; // 進行中または次に開始する投球インデックス
        private int _currentAttemptAccumulatedScore; // この投球で得たスコアの合計

        /// <summary>セッション開始時に購読者へ通知します。</summary>
        public event Action<SessionInfo> OnSessionStarted;
        /// <summary>投球開始時に購読者へ通知します。</summary>
        public event Action<AttemptInfo> OnAttemptStarted;
        /// <summary>投球決着時に結果を通知します。</summary>
        public event Action<AttemptResult> OnAttemptResolved;
        /// <summary>セッション終了時に最終リザルトを通知します。</summary>
        public event Action<SessionResult> OnSessionEnded;

        /// <summary>セッションが進行中かどうか。</summary>
        public bool SessionActive => _sessionActive;
        /// <summary>現在投球中かどうか。</summary>
        public bool AttemptActive => _attemptActive;
        /// <summary>使用しているルールアセット。</summary>
        public AttemptRules Rules => rules;
        /// <summary>参照しているスコアサービス。</summary>
        public ScoreService ScoreService => _scoreService;
        /// <summary>現在の投球インデックス (0-based)。</summary>
        public int CurrentAttemptIndex => _attemptIndex;

        /// <summary>ルールとスコアサービスを注入します。</summary>
        public void Initialize(AttemptRules injectedRules, ScoreService scoreService)
        {
            rules = injectedRules != null ? injectedRules : rules;
            _scoreService = scoreService ?? throw new ArgumentNullException(nameof(scoreService));
        }

        /// <summary>セッションを開始または再開します。</summary>
        public void StartSession(bool restart = false)
        {
            if (!_sessionActive)
            {
                EnsureDependencies();
            }
            else if (!restart)
            {
                Debug.LogWarning("AttemptManager.StartSession was called while a session is already active. Pass restart = true to force restart.");
                return;
            }

            EnsureDependencies();

            _scoreService.ResetScore();
            _attemptIndex = 0;
            _currentAttemptAccumulatedScore = 0;

            _sessionActive = true;

            var sessionInfo = new SessionInfo(rules.MaxAttempts, 0);
            Debug.Log($"[AttemptManager] セッション開始: 最大{rules.MaxAttempts}投");
            OnSessionStarted?.Invoke(sessionInfo);

            BeginAttempt();
        }

        /// <summary>セッションを強制停止します。</summary>
        public void StopSession()
        {
            if (!_sessionActive)
            {
                return;
            }

            FinishSession();
        }

        /// <summary>現在の投球スコアを加算します。</summary>
        public void ReportScore(int points)
        {
            if (!_sessionActive || !_attemptActive)
            {
                Debug.LogWarning($"ReportScore called while no attempt is active. points={points}");
                return;
            }

            if (points == 0)
            {
                return;
            }

            _currentAttemptAccumulatedScore += points;
            _scoreService?.AddScore(points);
        }

        /// <summary>現在の投球を完了させ、次の状態へ進めます。</summary>
        public void ResolveAttempt(AttemptResolutionCause cause = AttemptResolutionCause.Unknown)
        {
            if (!_sessionActive || !_attemptActive)
            {
                return;
            }

            _attemptActive = false;

            int attemptsRemainingBeforeAdvance = Math.Max(0, rules.MaxAttempts - _attemptIndex);
            var attemptInfo = new AttemptInfo(_attemptIndex, attemptsRemainingBeforeAdvance);
            var result = new AttemptResult(attemptInfo, _currentAttemptAccumulatedScore, _scoreService?.CurrentScore ?? 0, cause);
            Debug.Log($"[AttemptManager] {attemptInfo.DisplayAttemptNumber}/{rules.MaxAttempts}投目終了: 得点 {result.GainedScore}, 累計 {result.AccumulatedScore}, 理由 {cause}");
            OnAttemptResolved?.Invoke(result);

            _attemptIndex++;

            if (_attemptIndex >= rules.MaxAttempts)
            {
                FinishSession();
            }
            else
            {
                BeginAttempt();
            }
        }

        /// <summary>今の投球を回数消費なしでリセットします。</summary>
        public void ResetCurrentAttempt()
        {
            if (!_sessionActive || !_attemptActive)
            {
                return;
            }

            _currentAttemptAccumulatedScore = 0;
            var info = new AttemptInfo(_attemptIndex, Math.Max(0, rules.MaxAttempts - _attemptIndex));
            Debug.Log($"[AttemptManager] {info.DisplayAttemptNumber}/{rules.MaxAttempts}投目をリセットしました");
            OnAttemptStarted?.Invoke(info);
        }

        private void BeginAttempt()
        {
            _attemptActive = true;
            _currentAttemptAccumulatedScore = 0;

            int attemptsRemaining = Math.Max(0, rules.MaxAttempts - _attemptIndex);
            var info = new AttemptInfo(_attemptIndex, attemptsRemaining);
            Debug.Log($"[AttemptManager] {info.DisplayAttemptNumber}/{rules.MaxAttempts}投目を開始します。残り {info.AttemptsRemaining} 投");
            OnAttemptStarted?.Invoke(info);
        }

        private void FinishSession()
        {
            _sessionActive = false;
            _attemptActive = false;

            var finalScore = _scoreService?.CurrentScore ?? 0;
            var result = new SessionResult(finalScore, _attemptIndex);
            Debug.Log($"[AttemptManager] セッション終了: 総得点 {finalScore}, 使用投球 {_attemptIndex}/{rules.MaxAttempts}");
            OnSessionEnded?.Invoke(result);
        }

        private void EnsureDependencies()
        {
            if (rules == null)
            {
                throw new InvalidOperationException("AttemptManager requires AttemptRules. Assign it in the inspector or via Initialize().");
            }

            if (_scoreService == null)
            {
                throw new InvalidOperationException("AttemptManager requires ScoreService. Call Initialize() beforehand.");
            }
        }
    }
}
