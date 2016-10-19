using UnityEngine;
using System.Collections;

public class InputHandler : MonoBehaviour {

	const float SPEED_CHANGE_RATIO = 0.99f;
	Vector3 eulerAngles;
	float speed;

	// Use this for initialization
	void Start() {
		speed = 50;
	}
	
	// Update is called once per frame
	void Update() {
		bool paused = GameSystem.instance.paused;
		float delta = Time.deltaTime;

		if (Input.GetKeyDown("escape")) {
			paused = GameSystem.instance.TogglePaused();
		}

		if (!paused) {
			speed = speed * SPEED_CHANGE_RATIO + (Input.GetKey("space") ? 100 : 50) * (1 - SPEED_CHANGE_RATIO);
			transform.position += transform.forward * delta * speed;

			eulerAngles += new Vector3(
				-Input.GetAxis("Mouse Y") * 20, 
				Input.GetAxis("Mouse X") * 40, 
				Input.GetAxis("Mouse X") * -25
			);

			eulerAngles *= 0.95f;

			transform.Rotate(eulerAngles * delta);
		}
	}
}
