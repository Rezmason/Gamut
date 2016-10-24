using UnityEngine;
using System.Collections;

public class InputHandler : MonoBehaviour {

	const float SPEED_CHANGE_RATIO = 0.99f;
	const float START_SPEED = 80;
	const float MAX_SPEED = 150;
	Vector3 eulerAngles;
	float speed;

	void Start() {
		speed = START_SPEED;
	}
	
	void Update() {
		bool paused = GameSystem.instance.paused;
		float delta = Time.deltaTime;

		if (Input.GetKeyDown("escape")) {
			paused = GameSystem.instance.TogglePaused();
		}

		if (!paused) {
			speed = speed * SPEED_CHANGE_RATIO + (Input.GetKey("space") ? MAX_SPEED : START_SPEED) * (1 - SPEED_CHANGE_RATIO);
			transform.position += transform.forward * delta * speed;

			eulerAngles += new Vector3(
				-Input.GetAxis("Mouse Y") * 20, 
				Input.GetAxis("Mouse X") * 50, 
				Input.GetAxis("Mouse X") * -30
			);

			eulerAngles *= 0.95f;

			transform.Rotate(eulerAngles * delta);
		}
	}
}
