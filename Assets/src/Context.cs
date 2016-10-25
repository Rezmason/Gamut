using UnityEngine;
using UnityEngine.EventSystems;

public class Context
{
	public static void Init(GameObject gameObject)
	{
		GameObject.Instantiate(Resources.Load("Prefabs/EventSystem"));
		GameObject.Instantiate(Resources.Load("Prefabs/GUI"));
		GameObject.Instantiate(Resources.Load("Prefabs/Player"));

		InputSystem.instance.Init();
		GameSystem.instance.Init();
		RibbonSystem.instance.Init();
		// TODO: Turn RibbonWriter script into RibbonSystem

		gameObject.AddComponent<Updater>().updateMethod = UpdateHandler;
	}

	static void UpdateHandler() {
		InputSystem.instance.Update();
		RibbonSystem.instance.Update();
		GameSystem.instance.Update();
	}
}
