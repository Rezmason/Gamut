using UnityEngine;
using System.Collections;

public class InputSystem : Thingleton<InputSystem>, ISystem {

	const float SPEED_CHANGE_RATIO = 0.99f;
	const float START_SPEED = 80;
	const float MAX_SPEED = 150;
	Vector3 eulerAngles = new Vector3();
	float speed = START_SPEED;
	GameObject player;

	public void Setup() {
		player = GameObject.FindWithTag("Player");
	}

	public void Update() {
		if (Input.GetKeyDown("escape")) {
			GameSystem.instance.TogglePaused();
		}

		if (GameSystem.instance.gameRunning) {
			float delta = Time.deltaTime;
			speed = speed * SPEED_CHANGE_RATIO + (Input.GetKey("space") ? MAX_SPEED : START_SPEED) * (1 - SPEED_CHANGE_RATIO);
			player.transform.position += player.transform.forward * delta * speed;
			Camera cam = Camera.current;
			if (cam != null) {
				cam.fieldOfView = cam.fieldOfView * SPEED_CHANGE_RATIO + (60 + speed * 0.5f) * (1 - SPEED_CHANGE_RATIO);
			}

			eulerAngles += new Vector3(
				-Input.GetAxis("Mouse Y") * 20, 
				Input.GetAxis("Mouse X") * 50, 
				Input.GetAxis("Mouse X") * -30 + speed * 0.01f
			);

			eulerAngles *= 0.95f;
			player.transform.Rotate(eulerAngles * delta);
		}
	}

	public void Run() {

	}
}
