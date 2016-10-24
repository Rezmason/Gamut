using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class GameSystem {

	GameObject objective;
	GameObject swatch;
	GameObject ribbonHead;
	bool _paused;
	static List<ColorSpace> colorSpaces;
	static int activeColorSpace;

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
		colorSpaces = new List<ColorSpace>();
		colorSpaces.Add(new HSVCylinderColorSpace());
		colorSpaces.Add(new RGBCubeColorSpace());

		activeColorSpace = 0;
		colorSpaces[activeColorSpace].active = true;

		swatch = GameObject.Find("Swatch");
		ribbonHead = GameObject.Find("RibbonHead");

		objective = GameObject.Instantiate(Resources.Load("Prefabs/Objective") as GameObject);
		CheckpointBehavior checkpointBehavior = objective.AddComponent<CheckpointBehavior>();
		checkpointBehavior.collisionHandler = RespondToCollision;
		checkpointBehavior.ribbonHead = ribbonHead;
		//objective.transform.localScale = new Vector3(10, 10, 10);

		color = new Color();
		color.a = 1;
		SetCheckpoint();

		/*
		for (float i = 0; i < 1; i += 0.005f) {
			float magnitude = 500;
			Vector3 pos = new Vector3(
				Mathf.Cos(i * Mathf.PI * 2) * magnitude,
				Mathf.Sin(i * Mathf.PI * 10) * magnitude,
				Mathf.Sin(i * Mathf.PI * 2) * magnitude
			);
			GameObject copy = GameObject.Instantiate(objective);
			copy.transform.position = pos;
			copy.transform.localScale = Vector3.one * 10;
			copy.GetComponent<MeshRenderer>().material.color = colorSpaces[activeColorSpace].ColorFromWorldPosition(pos);
		}
		*/
	}

	void SetCheckpoint() {
		objective.transform.localPosition = colorSpaces[activeColorSpace].GetRandomObjectivePosition();
		Color color = colorSpaces[activeColorSpace].ColorFromWorldPosition(objective.transform.position);
		objective.GetComponent<MeshRenderer>().material.color = color;
		swatch.GetComponent<Image>().color = color;
	}

	void RespondToCollision() {
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
