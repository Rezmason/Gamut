using UnityEngine;
using System.Collections;

public class InputSystem : Thingleton<InputSystem>, ISystem {

	public event SimpleDelegate pauseGame;
	const float SPEED_CHANGE_RATIO = 0.075f;
	const float FOV_INCREASE_RATIO = 0.05f;
	const float FOV_DECREASE_RATIO = 0.01f;
	const float START_SPEED = 80;
	const float MAX_SPEED = 140;
	GameObject player;
	GameState state;

	public void Setup() {
		player = GameObject.FindWithTag("Player");
		state = GameState.instance;
	}

	public void Update() {

		if (state.gameRunning && Input.GetKeyDown("escape")) pauseGame();

		Camera cam = Camera.current;

		if (state.gameRunning) {

			bool gasPedal = Input.GetKey("space") || Input.GetMouseButton(0);

			float delta = Time.deltaTime;
			state.SetSpeed(Mathf.Lerp(state.speed, (gasPedal ? MAX_SPEED : START_SPEED), SPEED_CHANGE_RATIO));
			player.transform.position += player.transform.forward * delta * state.speed;
			if (cam != null) {
				cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, 60 + state.speed * 0.5f, FOV_INCREASE_RATIO);
				cam.transform.localRotation = Quaternion.Lerp(cam.transform.localRotation, Quaternion.identity, 0.1f);
			}

			Vector3 eulerAngles = state.eulerAngles;
			eulerAngles += new Vector3(
				-Input.GetAxis("Mouse Y") * 20, 
				Input.GetAxis("Mouse X") * 10, 
				Input.GetAxis("Mouse X") * -(10 + state.speed * 0.01f)
			);
			eulerAngles *= 0.95f;
			player.transform.Rotate(eulerAngles * delta);
			state.SetEulerAngles(eulerAngles);
		} else if (cam != null) {
			if (GameState.instance.paused) {
				cam.transform.Rotate(0, 0, 0.2f);
			} else {
				cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, 100, FOV_DECREASE_RATIO);
				cam.transform.Rotate(0.3f, 0, 0);
			}
		}
	}

	public void Run() {

	}
}
