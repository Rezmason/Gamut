using UnityEngine;
using System.Collections;

public class CameraUpdater : MonoBehaviour {

	private static Vector3 POSIITON_OFFSET = new Vector3(0, 5, -10);

	public GameObject subject;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 goalPosition = subject.transform.position - POSIITON_OFFSET;
		transform.position = Vector3.Lerp(transform.position, goalPosition, 0.1f);
	}
}
