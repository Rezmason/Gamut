using UnityEngine;

public class PauseMenuHandler : MonoBehaviour {
	public void ResumeGame() {
		MenuSystem.instance.ResumeGame();
	}
}
