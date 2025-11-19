using UnityEngine;
using JigokuBall.GameCore;

namespace DebugTool.Button.DebugToolTypeFunc
{
    /// <summary>
    /// デバッグ用：穴に入ったことをシミュレートするツール
    /// </summary>
    public class SimulateHoleHit : IToolFunc
    {
        public void ToggleActivation(GameObject obj)
        {
            // Ball オブジェクトを取得
            Ball ball = obj.GetComponent<Ball>();
            if (ball == null)
            {
                ball = Object.FindFirstObjectByType<Ball>(FindObjectsInactive.Include);
            }

            if (ball == null)
            {
                Debug.LogError("[DebugTool] Ball コンポーネントが見つかりません");
                return;
            }

            // AttemptManager を取得
            AttemptManager attemptManager = Object.FindFirstObjectByType<AttemptManager>(FindObjectsInactive.Include);
            if (attemptManager == null)
            {
                Debug.LogError("[DebugTool] AttemptManager が見つかりません");
                return;
            }

            Debug.Log("[DebugTool] 穴に入ったことをシミュレートします");

            // スコアを報告
            attemptManager.ReportScore(10);

            // 投球を完了させる
            attemptManager.ResolveAttempt(AttemptResolutionCause.SunkInHole);

            // ボールをリセット
            // ball.Reset();
        }
    }
}
