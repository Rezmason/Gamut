using UnityEngine;
using System.Collections;

class ObjectiveBehavior : MonoBehaviour {

	const float CLOSE_ENOUGH_DISTANCE = 0.5f;
	const float MIN_DISTANCE = 0.025f;
	public event SimpleDelegate collisionHandler;
	public GameObject subject;
	float _lastDistance = Mathf.Infinity;
	public float lastDistance { get { return _lastDistance; } }
	void Update() {
		Vector3 position = transform.worldToLocalMatrix.MultiplyPoint(subject.transform.position);
		float distance = position.magnitude;

		if (distance < MIN_DISTANCE) {
			collisionHandler();
			_lastDistance = Mathf.Infinity;
		} else if (_lastDistance < distance && _lastDistance < CLOSE_ENOUGH_DISTANCE) {
			collisionHandler();
			_lastDistance = Mathf.Infinity;
		} else {
			_lastDistance = distance;
		}
	}
}
