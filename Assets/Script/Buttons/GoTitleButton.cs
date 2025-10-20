using UnityEngine;
using UnityEngine.SceneManagement;

public class GoTitleButton : MonoBehaviour
{
    [SerializeField] private string _title = "startView";

    public void OnClick()
    {
        Debug.Log("タイトルへ戻るボタンが押されました。" + _title);
        SceneManager.LoadScene(_title);
    }
}