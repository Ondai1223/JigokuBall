using System;

namespace JigokuBall.GameCore
{
    /// <summary>
    /// セッション全体のスコアを集計し、更新イベントを配信します。
    /// </summary>
    public sealed class ScoreService
    {
        private int _score; // 現在の累計スコア

        /// <summary>スコアが変更されたときに通知するイベント。</summary>
        public event Action<ScoreChanged> OnScoreChanged;

        /// <summary>最新の累計スコアを取得します。</summary>
        public int CurrentScore => _score;

        /// <summary>スコアをリセットし、必要に応じて購読者へ通知します。</summary>
        public void ResetScore(bool notify = true)
        {
            int previous = _score;
            _score = 0;

            if (notify)
            {
                int delta = previous != 0 ? -previous : 0;
                OnScoreChanged?.Invoke(new ScoreChanged(_score, delta));
            }
        }

        /// <summary>スコアを加算し、イベントで通知します。</summary>
        public void AddScore(int delta)
        {
            if (delta == 0)
            {
                return;
            }

            _score += delta;
            OnScoreChanged?.Invoke(new ScoreChanged(_score, delta));
        }
    }
}
