using UnityEngine;
using UnityEngine.UI;

public class GoResultButton : MonoBehaviour
{
    [SerializeField] private GameObject _resultModal;

    public void OnClick()
    {
        _resultModal.SetActive(true);
    }

}