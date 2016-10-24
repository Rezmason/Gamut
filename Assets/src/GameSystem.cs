using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class GameSystem {

	GameObject objective;
	GameObject swatch;
	GameObject player;
	GameObject ribbonHead;
	GameObject mainCamera;
	bool _paused;
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

	public void Init () {
		colorSpaces = new List<ColorSpace>();
		colorSpaces.Add(new HSVCylinderColorSpace());
		colorSpaces.Add(new RGBCubeColorSpace());

		activeColorSpaceIndex = 0;
		activeColorSpace.active = true;

		swatch = GameObject.Find("Swatch");
		player = GameObject.Find("Player");
		ribbonHead = player.transform.Find("RibbonHead").gameObject;
		mainCamera = player.transform.Find("Main Camera").gameObject;

		objective = GameObject.Instantiate(Resources.Load("Prefabs/Objective") as GameObject);
		ObjectiveBehavior objectiveBehavior = objective.AddComponent<ObjectiveBehavior>();
		objectiveBehavior.collisionHandler = RespondToCollision;
		objectiveBehavior.mainCamera = mainCamera;
		objectiveBehavior.ribbonHead = ribbonHead;
		FaceCameraBehavior faceCameraBehavior = objective.AddComponent<FaceCameraBehavior>();
		faceCameraBehavior.mainCamera = mainCamera;


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

	void SetCheckpoint() {
		objective.transform.localPosition = activeColorSpace.GetRandomObjectivePosition();
		Color color = activeColorSpace.ColorFromWorldPosition(objective.transform.position);
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
