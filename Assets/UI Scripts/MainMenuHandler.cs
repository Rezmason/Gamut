using UnityEngine;
using System.Collections;

using UnityEditor;

public class MainMenuHandler : MonoBehaviour {

	public void StartGame() { MenuSystem.instance.StartGame(); }
	public void AboutGame() { MenuSystem.instance.ShowAboutMenu(); }
	public void QuitGame() { MenuSystem.instance.QuitGame(); }
}
