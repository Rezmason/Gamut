using UnityEngine;
using System.Collections;

class ObjectiveBehavior : MonoBehaviour {

	const float CLOSE_ENOUGH_DISTANCE = 0.5f;
	const float MIN_DISTANCE = 0.025f;
	public event SimpleDelegate collisionHandler;
	public GameObject subject;
	float lastDistance = Mathf.Infinity;
	void Update() {
		Vector3 position = transform.worldToLocalMatrix.MultiplyPoint(subject.transform.position);
		float distance = position.magnitude;

		if (distance < MIN_DISTANCE) {
			collisionHandler();
			lastDistance = Mathf.Infinity;
		} else if (lastDistance < distance && lastDistance < CLOSE_ENOUGH_DISTANCE) {
			collisionHandler();
			lastDistance = Mathf.Infinity;
		} else {
			lastDistance = distance;
		}
	}
}
