using UnityEngine;

public class EntryPointHandler : MonoBehaviour {
	void Awake () {
		Context.Init(gameObject);
	}
}
