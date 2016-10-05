using UnityEngine;
using System.Collections;

public class InputHandler : MonoBehaviour {

	const float SPEED_CHANGE_RATIO = 0.99f;
	Vector3 eulerAngles;
	float speed;
	bool paused;

	// Use this for initialization
	void Start() {
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
		speed = 100;
	}
	
	// Update is called once per frame
	void Update() {
		float delta = Time.deltaTime;

		if (Input.GetKeyDown("escape")) {
			paused = !paused;
		}

		if (!paused) {
			speed = speed * SPEED_CHANGE_RATIO + (Input.GetKey("space") ? 200 : 100) * (1 - SPEED_CHANGE_RATIO);
			transform.position += transform.forward * delta * speed;
			UpdateNaiveControlScheme(delta);
		}
	}

	void UpdateNaiveControlScheme(float delta) {

		eulerAngles += new Vector3(
          -Input.GetAxis("Mouse Y") * 40, 
          Input.GetAxis("Mouse X") * 80, 
          Input.GetAxis("Mouse X") * -50
      	);

		eulerAngles *= 0.95f;

		transform.Rotate(eulerAngles * delta);
	}
}
