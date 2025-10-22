using UnityEngine;
using UnityEngine.SceneManagement;
using HUD.Score;

public class GoTitleButton : MonoBehaviour
{
    [SerializeField] private string _title = "startView";

    public void OnClick()
    {
        Score.GetInstance().ResetScore();
        SceneManager.LoadScene(_title);
    }
}