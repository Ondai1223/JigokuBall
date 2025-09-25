using UnityEngine;

public class GameModeManager : MonoBehaviour
{
    // ゲームモード全体の管理を行うスクリプト
    // ゲームの状態のフラグを立てておく
    private GameMode currentGameMode;
    private ThrownState currentThrownState;
    public GameMode CurrentGameMode { get { return currentGameMode; } }
    public ThrownState CurrentThrownState { get { return currentThrownState; } }

    void Start()
    {
        currentGameMode = GameMode.EGM_Title;
        currentThrownState = ThrownState.ETS_Zero;
    }

    public enum GameMode
    {
        EGM_Title, // タイトル画面
        EGM_Game, // プレイ中
        EGM_Result // result画面
    }
    public enum ThrownState
    {
        ETS_Zero, //まだ投げていない状態
        ETS_Once, //一回投げた状態
        ETS_Twice, //二回投げた状態
        ETS_Thrice, //三回投げた状態
        ETS_Fourth, //四回投げた状態
        ETS_Fifth //五回投げた状態
    }

    public void nextGameMode()
    {
        switch (currentGameMode)
        {
            case GameMode.EGM_Title:
                currentGameMode = GameMode.EGM_Game;
                break;
            case GameMode.EGM_Game:
                currentGameMode = GameMode.EGM_Result;
                break;
            case GameMode.EGM_Result:
                currentGameMode = GameMode.EGM_Title;
                break;
        }
    }
    public void nextThrownState()
    {
        switch (currentThrownState)
        {
            case ThrownState.ETS_Zero:
                currentThrownState = ThrownState.ETS_Once;
                break;
            case ThrownState.ETS_Once:
                currentThrownState = ThrownState.ETS_Twice;
                break;
            case ThrownState.ETS_Twice:
                currentThrownState = ThrownState.ETS_Thrice;
                break;
            case ThrownState.ETS_Thrice:
                currentThrownState = ThrownState.ETS_Fourth;
                break;
            case ThrownState.ETS_Fourth:
                currentThrownState = ThrownState.ETS_Fifth;
                break;
            case ThrownState.ETS_Fifth:
                // これ以上進まない
                break;
        }
    }
}
