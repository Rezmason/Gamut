using UnityEngine;
using System.Collections;

public class InputHandler : MonoBehaviour {

	// Use this for initialization
	void Start() {
		Cursor.lockState = CursorLockMode.Locked;
	}
	
	// Update is called once per frame
	void Update() {
		float delta = Time.deltaTime;
		float speed = Input.GetKey("space") ? 200 : 100;
		transform.position += transform.forward * delta * speed;
		UpdateNaiveControlScheme(delta);
	}

	void UpdateNaiveControlScheme(float delta) {
		Vector3 eulerAngles = new Vector3(
          -Input.GetAxis("Mouse Y") * 50, 
          Input.GetAxis("Mouse X") * 100, 
          Input.GetAxis("Mouse X") * -50
      	);
		transform.Rotate(eulerAngles * delta);
	}
}
