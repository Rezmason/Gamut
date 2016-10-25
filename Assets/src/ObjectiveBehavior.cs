﻿using UnityEngine;
using System.Collections;

class ObjectiveBehavior : MonoBehaviour {

	static float MIN_DISTANCE = 15;
	public delegate void CollisionHandlerType();
	public CollisionHandlerType collisionHandler = null;
	public GameObject subject;

	void Update() {
		if (Vector3.Distance(subject.transform.position, transform.position) < MIN_DISTANCE) {
			collisionHandler();
		}
	}
}
