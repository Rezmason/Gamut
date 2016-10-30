using UnityEngine;

public class PauseMenuHandler : MonoBehaviour {
	public void ResumeGame() { MenuSystem.instance.ResumeGame(); }
	public void AbortGame() { MenuSystem.instance.AbortGame(); }
}
