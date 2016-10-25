using UnityEngine;
using System.Collections;

using UnityEditor;

public class MainMenuHandler : MonoBehaviour {
	
	public void StartGame() {
		gameObject.SetActive(false);
		GameSystem.instance.StartGame();
	}

	public void AboutGame() {
		gameObject.SetActive(false);
	}

	public void QuitGame() {
		Application.Quit();
	}
}
