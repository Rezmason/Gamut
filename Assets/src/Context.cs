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

		GameState gameState = GameState.instance;

		systems.Add(GameSystem.instance);
		systems.Add(InputSystem.instance);
		systems.Add(RibbonSystem.instance);
		systems.Add(MenuSystem.instance);

		foreach (ISystem system in systems) {
			system.Setup();
			updater.updateMethod += system.Update;
		}

		// TODO: inter-system event hookup
		MenuSystem.instance.startGame += GameSystem.instance.StartGame;
		MenuSystem.instance.resumeGame += GameSystem.instance.ResumeGame;
		MenuSystem.instance.abortGame += GameSystem.instance.AbortGame;
		MenuSystem.instance.quitGame += Application.Quit;
		InputSystem.instance.pauseGame += GameSystem.instance.PauseGame;
		InputSystem.instance.pauseGame += MenuSystem.instance.ShowPausedMenu;
		GameSystem.instance.endGame += MenuSystem.instance.ShowGameOverMenu;
		GameSystem.instance.startGame += RibbonSystem.instance.Reset;

		foreach (ISystem system in systems) {
			system.Run();
		}
	}
}
