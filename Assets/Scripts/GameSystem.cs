using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameSystem {

	GameObject objective;
	GameObject swatch;
	GameObject ribbonHead;
	bool _paused;

	public bool paused {
		get { return _paused; }
	}

	static GameSystem _instance;
	public static GameSystem instance
	{
		get { 
			if (_instance == null) {
				_instance = new GameSystem();
			}
			return _instance; 
		}
	}

	GameSystem() {
		_paused = true;
	}

	Color color;

	public void Init () {
		swatch = GameObject.Find("Swatch");
		ribbonHead = GameObject.Find("RibbonHead");

		objective = GameObject.Instantiate(Resources.Load("Prefabs/Objective") as GameObject);
		CheckpointBehavior checkpointBehavior = objective.AddComponent<CheckpointBehavior>();
		checkpointBehavior.collisionHandler = RespondToCollision;
		checkpointBehavior.ribbonHead = ribbonHead;
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

	public bool TogglePaused()
	{
		_paused = !_paused;
		Cursor.lockState = _paused ? CursorLockMode.None : CursorLockMode.Locked;
		Cursor.visible = _paused;
		return _paused;
	}
}

class CheckpointBehavior : MonoBehaviour {

	static float GOAL_ACCUM_PROXIMITY = 1.0f;
	public delegate void CollisionHandlerType();
	public CollisionHandlerType collisionHandler = null;
	public GameObject ribbonHead;
	float accumulatedProximity = 0;

	void Update() {
		float proximity = 1 / Vector3.Distance(ribbonHead.transform.position, transform.position);
		accumulatedProximity = Mathf.Max(0, accumulatedProximity + proximity) * 0.9f;
		if (accumulatedProximity > GOAL_ACCUM_PROXIMITY) {
			accumulatedProximity = 0;
			collisionHandler();
		}
		//Debug.Log(accumulatedProximity.ToString());
	}
}