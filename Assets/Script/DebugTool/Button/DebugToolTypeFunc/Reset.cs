using UnityEngine;

namespace DebugTool.Button.DebugToolTypeFunc
{
    public class Reset : MonoBehaviour, IToolFunc
    {
        public void ToggleActivation(GameObject obj)
        {
            // objがBall.csを持っているかどうかで条件分岐
            if(obj.TryGetComponent<BallThrower>(out BallThrower ball))
            {
                ball.ResetBall(); // BallThrower.csのResetBallメソッドを呼び出す
            }
            else
            {
                Debug.LogWarning("BallThrower.csを持つオブジェクトをアタッチしてください");
            }
        }
    }
}