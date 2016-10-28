﻿using UnityEngine;
using System.Collections;

public class MainMenuHandler : MonoBehaviour {

	public void StartGame() { MenuSystem.instance.StartGame(); }
	public void AboutGame() { MenuSystem.instance.ShowAboutMenu(); }
	public void QuitGame() { MenuSystem.instance.QuitGame(); }
}
