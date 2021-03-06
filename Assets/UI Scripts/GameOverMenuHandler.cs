﻿using UnityEngine;
using UnityEngine.UI;

public class GameOverMenuHandler : MonoBehaviour {

	void OnEnable() {
		transform.Find("FinalScore").gameObject.GetComponent<Text>().text = GameState.instance.score.ToString();
	}

	public void ReturnToMain() {
		MenuSystem.instance.ShowMainMenu();
	}
}
