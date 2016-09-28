using UnityEngine;
using System.Collections;

public class InputHandler : MonoBehaviour {

	// Use this for initialization
	void Start() {
		
	}
	
	// Update is called once per frame
	void Update() {
		float delta = Time.deltaTime;
		UpdateNaiveControlScheme(delta);
	}

	void UpdateNaiveControlScheme(float delta) {
		// Naive control scheme

		// quasi-A/D axis
		bool rollLeft  = Input.GetKey("a");
		bool rollRight = Input.GetKey("d");
		if (rollLeft && !rollRight) {
			transform.Rotate(new Vector3(0, 0,  200 * delta));
		} else if (rollRight && !rollLeft) {
			transform.Rotate(new Vector3(0, 0, -200 * delta));
		}

		// quasi-mouseX axis
		bool turnLeft = Input.GetKey("left");
		bool turnRight = Input.GetKey("right");
		if (turnLeft && !turnRight) {
			transform.Rotate(new Vector3(0, -200 * delta, 0));
		} else if (turnRight && !turnLeft) {
			transform.Rotate(new Vector3(0,  200 * delta, 0));
		}

		// quasi-mouseY axis
		bool turnUp = Input.GetKey("up");
		bool turnDown = Input.GetKey("down");
		if (turnUp && !turnDown) {
			transform.Rotate(new Vector3(-200 * delta, 0, 0));
		} else if (turnDown && !turnUp) {
			transform.Rotate(new Vector3( 200 * delta, 0, 0));
		}

		// quasi-W/S axis
		bool increaseVelocity = Input.GetKey("w");
		bool decreaseVelocity = Input.GetKey("s");
		if (increaseVelocity && !decreaseVelocity) {
			transform.Translate(0, 0, 100 * delta);
		} else if (!increaseVelocity && decreaseVelocity) {
			transform.Translate(0, 0, -100 * delta);
		}
	}
}
