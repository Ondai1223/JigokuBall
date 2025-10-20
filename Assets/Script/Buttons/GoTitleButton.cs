using UnityEngine;
using UnityEngine.SceneManagement;

public class GoTitleButton : MonoBehaviour
{
    [SerializeField] private string _title = "startView";

    public void OnClick()
    {
        if (string.IsNullOrEmpty(_title))
        {
            Debug.LogError("Scene name is empty or null!");
            return;
        }

        Debug.Log($"Attempting to load scene: {_title}");

        try
        {
            SceneManager.LoadScene(_title);
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Failed to load scene: {_title}. Exception: {ex.Message}");
        }
    }
}