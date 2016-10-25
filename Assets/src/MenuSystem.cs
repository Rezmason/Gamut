using UnityEngine;
using System.Collections.Generic;

public class MenuSystem : Thingleton<MenuSystem>, ISystem {

	GameObject mainMenu;
	GameObject pausedMenu;
	GameObject aboutMenu;
	GameObject gameOverMenu;

	List<GameObject> menus;

	public event SimpleDelegate startGame;
	public event SimpleDelegate quitGame;
	public event SimpleDelegate resumeGame;
	public event SimpleDelegate abortGame;

	public void Setup() {
		
		GameObject gui = GameObject.FindWithTag("GUI");
		mainMenu = gui.transform.Find("MainMenu").gameObject;
		pausedMenu = gui.transform.Find("PausedMenu").gameObject;
		aboutMenu = gui.transform.Find("AboutMenu").gameObject;
		gameOverMenu = gui.transform.Find("GameOverMenu").gameObject;

		menus = new List<GameObject>();
		menus.Add(mainMenu);
		menus.Add(pausedMenu);
		menus.Add(aboutMenu);
		menus.Add(gameOverMenu);

		ShowMainMenu();
	}

	public void Update() {}

	public void Run() {}

	public void StartGame() { 
		HideMenus();
		if (startGame != null) startGame();
	}

	public void ShowAboutMenu() {
		HideMenus();
		aboutMenu.SetActive(true);
	}

	public void ShowMainMenu() {
		HideMenus();
		mainMenu.SetActive(true);
	}

	public void ShowPausedMenu() { 
		HideMenus();
		pausedMenu.SetActive(true); 
	}

	public void ShowGameOverMenu() {
		HideMenus();
		gameOverMenu.SetActive(true);
	}

	public void QuitGame() { if (quitGame != null) quitGame(); }

	public void ResumeGame() { 
		HideMenus();
		if (resumeGame != null) resumeGame(); 
	}

	void HideMenus() {
		foreach (GameObject menu in menus) {
			menu.SetActive(false);
		}
	}
}
