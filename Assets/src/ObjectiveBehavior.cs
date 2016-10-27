using UnityEngine;
using System.Collections;

class ObjectiveBehavior : MonoBehaviour {

	static float GOAL_ACCUM_PROXIMITY = 1.0f;
	static float PROXIMITY_MAG = 0.005f;
	public event SimpleDelegate collisionHandler;
	public GameObject subject;
	float accumulatedProximity = 0;

	void Update() {
		Vector3 position = transform.worldToLocalMatrix.MultiplyPoint(subject.transform.position);
		float proximity = PROXIMITY_MAG / position.magnitude;
		accumulatedProximity = Mathf.Max(0, accumulatedProximity + proximity) * 0.9f;
		if (accumulatedProximity > GOAL_ACCUM_PROXIMITY) {
			collisionHandler();
		}
	}

	public void Reset() {
		accumulatedProximity = 0;
	}
}
