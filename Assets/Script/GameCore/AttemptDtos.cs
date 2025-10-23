using System;

namespace JigokuBall.GameCore
{
    /// <summary>
    /// セッション全体の情報を保持します。
    /// </summary>
    public readonly struct SessionInfo
    {
        public SessionInfo(int attemptLimit, int attemptsUsed)
        {
            AttemptLimit = attemptLimit;
            if (attemptsUsed < 0)
            {
                AttemptsUsed = 0;
            }
            else if (attemptsUsed > attemptLimit)
            {
                AttemptsUsed = attemptLimit;
            }
            else
            {
                AttemptsUsed = attemptsUsed;
            }
        }

        public int AttemptLimit { get; } // セッションで許可されている最大投球数
        public int AttemptsUsed { get; } // 既に使用した投球数
        public int AttemptsRemaining => Math.Max(0, AttemptLimit - AttemptsUsed); // 残り投球数
    }

    /// <summary>
    /// 単一投球の開始時情報。
    /// </summary>
    public readonly struct AttemptInfo
    {
        public AttemptInfo(int attemptIndex, int attemptsRemaining)
        {
            AttemptIndex = attemptIndex;
            AttemptsRemaining = Math.Max(0, attemptsRemaining);
        }

        public int AttemptIndex { get; } // 今回の投球インデックス (0-based)
        public int AttemptsRemaining { get; } // 投球開始時点で残っている回数
        public int DisplayAttemptNumber => AttemptIndex + 1; // UI 用 1-based 表示
    }

    /// <summary>
    /// 投球完了時に通知する結果情報。
    /// </summary>
    public readonly struct AttemptResult
    {
        public AttemptResult(AttemptInfo attemptInfo, int gainedScore, int accumulatedScore, AttemptResolutionCause cause)
        {
            AttemptInfo = attemptInfo;
            GainedScore = gainedScore;
            AccumulatedScore = accumulatedScore;
            Cause = cause;
        }

        public AttemptInfo AttemptInfo { get; } // 投球識別子
        public int GainedScore { get; } // この投球で得たスコア
        public int AccumulatedScore { get; } // セッション累計スコア
        public AttemptResolutionCause Cause { get; } // 投球終了理由
    }

    /// <summary>
    /// 総合スコア更新を伝える値オブジェクト。
    /// </summary>
    public readonly struct ScoreChanged
    {
        public ScoreChanged(int newScore, int delta)
        {
            NewScore = newScore;
            Delta = delta;
        }

        public int NewScore { get; } // 最新の累計スコア
        public int Delta { get; } // 今回加算されたスコア差分
    }

    /// <summary>
    /// セッション完了時に通知するリザルト。
    /// </summary>
    public sealed class SessionResult
    {
        public SessionResult(int finalScore, int attemptsUsed)
        {
            FinalScore = finalScore;
            AttemptsUsed = attemptsUsed;
        }

        public int FinalScore { get; } // 最終スコア
        public int AttemptsUsed { get; } // 使用した投球数
    }

    /// <summary>
    /// 投球が終了した理由を分類します。
    /// </summary>
    public enum AttemptResolutionCause
    {
        Unknown = 0,
        SunkInHole,
        OutOfBounds,
        Timeout,
        Manual,
        Reset
    }
}
