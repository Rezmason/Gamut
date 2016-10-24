using UnityEngine;
using UnityEngine.EventSystems;

public class Context
{
	public static void Init(GameObject gameObject)
	{
		GameObject.Instantiate(Resources.Load("Prefabs/EventSystem"));
		GameObject.Instantiate(Resources.Load("Prefabs/GUI"));
		// TODO: populate rest of the scene from prefabs

		GameSystem.instance.Init();
		// TODO: Turn RibbonWriter script into RibbonDancer system
	}
}
