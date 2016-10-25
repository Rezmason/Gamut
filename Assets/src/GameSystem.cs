using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class GameSystem : Thingleton<GameSystem> {

	GameObject objective;
	GameObject swatch;
	Text tScore;
	Text tClock;
	GameObject player;
	GameObject ribbonHead;
	uint score;
	float timeRemaining;
	bool _paused = true;
	List<ColorSpace> colorSpaces;
	int activeColorSpaceIndex;

	public ColorSpace activeColorSpace { 
		get { 
			return colorSpaces[activeColorSpaceIndex];
		} 
	}

	public bool paused {
		get { return _paused; }
	}

	public override void Init () {
		colorSpaces = new List<ColorSpace>();
		colorSpaces.Add(new HSVCylinderColorSpace());
		colorSpaces.Add(new RGBCubeColorSpace());

		activeColorSpaceIndex = 0;
		activeColorSpace.active = true;

		swatch = GameObject.FindWithTag("Swatch");
		tScore = swatch.transform.Find("Score").gameObject.GetComponent<Text>();
		tClock = swatch.transform.Find("Clock").gameObject.GetComponent<Text>();
		player = GameObject.FindWithTag("Player");
		ribbonHead = player.transform.Find("RibbonHead").gameObject;

		objective = GameObject.Instantiate(Resources.Load("Prefabs/Objective") as GameObject);
		ObjectiveBehavior objectiveBehavior = objective.AddComponent<ObjectiveBehavior>();
		objectiveBehavior.collisionHandler = RespondToCollision;
		objectiveBehavior.ribbonHead = ribbonHead;
		objective.AddComponent<FaceCameraBehavior>();

		score = 0;
		ResetTime();
		tScore.text = score.ToString();
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
			copy.GetComponent<MeshRenderer>().material.color = activeColorSpace.ColorFromWorldPosition(pos);
		}
		*/
	}

	public void Update() {
		if (!_paused) {
			timeRemaining = Mathf.Max(0, timeRemaining - Time.deltaTime);
			tClock.text = timeRemaining.ToString("0.00");
			if (timeRemaining == 0) {
				TogglePaused();
			}
		}
	}

	void ResetTime() {
		timeRemaining = 10 + 20 / (score * 0.1f + 1);
		tClock.text = timeRemaining.ToString("0.00");
	}

	void SetCheckpoint() {
		objective.transform.position = activeColorSpace.GetRandomObjectivePosition();
		Color color = activeColorSpace.ColorFromWorldPosition(objective.transform.position);
		objective.GetComponent<MeshRenderer>().material.color = color;
		swatch.GetComponent<Image>().color = color;
	}

	void RespondToCollision() {
		score++;
		tScore.text = score.ToString();
		SetCheckpoint();
		ResetTime();
	}

	public bool TogglePaused()
	{
		_paused = !_paused;
		Cursor.lockState = _paused ? CursorLockMode.None : CursorLockMode.Locked;
		Cursor.visible = _paused;
		return _paused;
	}
}
