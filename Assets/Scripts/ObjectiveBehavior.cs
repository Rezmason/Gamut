using UnityEngine;
using System.Collections;

class ObjectiveBehavior : MonoBehaviour {

	static float GOAL_ACCUM_PROXIMITY = 1.0f;
	static float PROXIMITY_MAG = 2.0f;
	public delegate void CollisionHandlerType();
	public CollisionHandlerType collisionHandler = null;
	public GameObject ribbonHead;
	public GameObject mainCamera;
	float accumulatedProximity = 0;

	void Update() {
		AccumulateProximity();
		FaceCamera();
		ScaleWithCameraDistance();
	}

	void AccumulateProximity() {
		float proximity = PROXIMITY_MAG / Vector3.Distance(ribbonHead.transform.position, transform.position);
		accumulatedProximity = Mathf.Max(0, accumulatedProximity + proximity) * 0.9f;
		if (accumulatedProximity > GOAL_ACCUM_PROXIMITY) {
			accumulatedProximity = 0;
			collisionHandler();
		}
		//Debug.Log(accumulatedProximity.ToString());
	}

	void FaceCamera() {
		transform.LookAt(transform.position + mainCamera.transform.rotation * Vector3.forward, mainCamera.transform.rotation * Vector3.up);
		transform.Rotate(Vector3.forward * Time.time * 40);
	}

	void ScaleWithCameraDistance() {
		float dist = Vector3.Distance(transform.position, mainCamera.transform.position);
		transform.localScale = Vector3.one * (20 + 500000 / Mathf.Pow(dist, 2));
	}
}
