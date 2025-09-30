using UnityEngine;

namespace DebugTool.Button.DebugToolTypeFunc
{
    public class Activator : MonoBehaviour, IToolFunc
    {
        private bool isActive = false;

        public void ToggleActivation(GameObject obj)
        {
            if (isActive)
            {
                Deactivate(obj);
            }
            else
            {
                Activate(obj);
            }
            isActive = !isActive;
        }
        private void Activate(GameObject obj)
        {
            obj.SetActive(true);
        }
        private void Deactivate(GameObject obj)
        {
            obj.SetActive(false);
        }
    }
}