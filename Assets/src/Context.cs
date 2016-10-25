using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class Context
{
	public static void Init(GameObject gameObject)
	{
		// Build scene
		GameObject.Instantiate(Resources.Load("Prefabs/EventSystem"));
		GameObject.Instantiate(Resources.Load("Prefabs/GUI"));
		GameObject.Instantiate(Resources.Load("Prefabs/Player"));

		Updater updater = gameObject.AddComponent<Updater>();

		List<ISystem> systems = new List<ISystem>();

		systems.Add(InputSystem.instance);
		systems.Add(GameSystem.instance);
		systems.Add(RibbonSystem.instance);

		foreach (ISystem system in systems) {
			system.Setup();
			updater.updateMethod += system.Update;
		}

		// TODO: inter-system event hookup

		foreach (ISystem system in systems) {
			system.Run();
		}
	}
}
