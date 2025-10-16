using UnityEngine;
using UnityEngine.UI;
using DebugTool.Button.DebugToolTypeFunc;

namespace DebugTool.Button
{
	public class DebugToolActivatorButton : MonoBehaviour
	{
		[SerializeField] private GameObject obj; // デバッグツールのパネル
		[SerializeField] private ToolType toolType;
		private IToolFunc toolFunc;

		void Awake()
		{
			CheckActiveState();
		}

		public void OnClick()
		{
			toolFunc.ToggleActivation(obj);
		}

		private void CheckActiveState()
		{
			switch(toolType)
				{
				case ToolType.Activator:
					toolFunc = new Activator();
					break;
				case ToolType.Reset:
					toolFunc = new Reset();
					break;
				default:
					Debug.LogError("Unsupported tool type");
					break;
			}
		}
	}

	public enum ToolType
	{
		Activator,
		Reset,
	}
}