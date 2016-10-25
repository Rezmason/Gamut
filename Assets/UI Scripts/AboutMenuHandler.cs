using UnityEngine;

public class AboutMenuHandler : MonoBehaviour {
	public void ReturnToMain() {
		MenuSystem.instance.ShowMainMenu();
	}
}
