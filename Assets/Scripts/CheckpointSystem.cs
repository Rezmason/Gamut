using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CheckpointSystem {

	GameObject objective;
	GameObject swatch;

	static CheckpointSystem _instance;
	public static CheckpointSystem instance
	{
		get { 
			if (_instance == null) {
				_instance = new CheckpointSystem();
			}
			return _instance; 
		}
	}

	CheckpointSystem() {
		
	}

	Color color;

	public void Init () {
		swatch = GameObject.Find("Swatch");

		objective = GameObject.Instantiate(Resources.Load("Prefabs/Objective") as GameObject);
		objective.AddComponent<CheckpointBehavior>().collisionHandler = RespondToCollision;
		objective.transform.localScale = new Vector3(10, 10, 10);

		color = new Color();
		color.a = 1;
		SetCheckpoint();
	}

	void SetCheckpoint() {
		color.r = Random.value;
		color.g = Random.value;
		color.b = Random.value;
		swatch.GetComponent<Image>().color = color;

		objective.transform.localPosition = new Vector3(Random.value - 0.5f, Random.value - 0.5f, Random.value - 0.5f) * 200;
	}

	void RespondToCollision() {
		Debug.Log("!!");
		SetCheckpoint();
	}
}

class CheckpointBehavior : MonoBehaviour {

	public delegate void CollisionHandlerType();
	public CollisionHandlerType collisionHandler = null;

	void OnCollisionEnter (Collision collision) {
		Debug.Log(collision.collider.name);
		collisionHandler();
	}
}