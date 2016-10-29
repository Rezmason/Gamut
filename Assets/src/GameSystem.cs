using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class GameSystem : Thingleton<GameSystem>, ISystem {

	public event SimpleDelegate endGame;
	public event SimpleDelegate startGame;
	const float MIN_DISTANCE = 200;
	GameObject objective;
	GameObject swatch;
	Text tScore;
	Text tClock;
	GameObject player;
	GameObject subject;
	GameObject hud;
	GameObject scoreBurst;
	ParticleSystem scoreParticles;
	uint _score;
	public uint score { get { return _score; } }
	float timeRemaining;
	List<ColorSpace> colorSpaces;
	int activeColorSpaceIndex;

	public ColorSpace activeColorSpace { 
		get { 
			return colorSpaces[activeColorSpaceIndex];
		} 
	}

	public bool gameRunning {
		get { return _started && !_paused; }
	}

	bool _started = false;
	public bool started {
		get { return _started; }
	}

	bool _paused = false;
	public bool paused {
		get { return _paused; }
	}

	public void Setup () {
		colorSpaces = new List<ColorSpace>();
		colorSpaces.Add(new HSVCylinderColorSpace());
		colorSpaces.Add(new RGBCubeColorSpace());

		activeColorSpaceIndex = 0;
		activeColorSpace.active = true;

		hud = GameObject.FindWithTag("GUI").transform.Find("HUD").gameObject;

		swatch = hud.transform.Find("Swatch").gameObject;
		tScore = hud.transform.Find("Score").gameObject.GetComponent<Text>();
		tClock = hud.transform.Find("Clock").gameObject.GetComponent<Text>();
		player = GameObject.FindWithTag("Player");
		subject = player.transform.Find("Subject").gameObject;

		objective = GameObject.Instantiate(Resources.Load("Prefabs/Objective") as GameObject);
		ObjectiveBehavior objectiveBehavior = objective.AddComponent<ObjectiveBehavior>();
		objectiveBehavior.collisionHandler += RespondToCollision;
		objectiveBehavior.subject = subject;
		objective.AddComponent<FaceCameraBehavior>().scaleMag = 900;

		scoreBurst = GameObject.Instantiate(Resources.Load("Prefabs/ScoreBurst") as GameObject);
		scoreParticles = scoreBurst.GetComponent<ParticleSystem>();
		scoreBurst.transform.position = subject.transform.position;
		scoreParticles.Stop();

		UpdateState();
	}

	public void Update() {
		if (gameRunning) {
			timeRemaining = Mathf.Max(0, timeRemaining - Time.deltaTime);
			tClock.text = timeRemaining.ToString("0.00");

			activeColorSpace.Rotate(new Vector3(0, score * 0.01f, 0));
			UpdateSpotColor();

			if (timeRemaining == 0) {
				EndGame();
			}
		}
	}

	public void Run() {

	}

	void UpdateSpotColor() {
		Color color = activeColorSpace.ColorFromWorldPosition(objective.transform.position);
		objective.GetComponent<MeshRenderer>().material.color = color;
		swatch.GetComponent<Image>().color = color;
	}

	void ResetTime() {
		timeRemaining = 10 + 20 / (score * 0.1f + 1);
		tClock.text = timeRemaining.ToString("0.00");
	}

	void SetCheckpoint() {
		Vector3 position = objective.transform.position;
		Vector3 nextPosition = position;

		while (Vector3.Distance(nextPosition, position) < MIN_DISTANCE) {
			nextPosition = activeColorSpace.GetRandomObjectivePosition();
		}

		objective.GetComponent<ObjectiveBehavior>().Reset();
		objective.transform.position = nextPosition;
		UpdateSpotColor();
	}

	void RespondToCollision() {
		_score++;
		scoreBurst.transform.position = objective.transform.position;
		scoreParticles.Clear();
		scoreParticles.Play();
		tScore.text = score.ToString();
		SetCheckpoint();
		ResetTime();
	}

	public void StartGame() {
		_started = true;
		_paused = false;

		_score = 0;
		ResetTime();
		tScore.text = score.ToString();
		SetCheckpoint();

		UpdateState();
		startGame();
	}

	void EndGame() {
		_started = false;
		UpdateState();
		endGame();
	}

	public void AbortGame() {
		_started = false;
		UpdateState();
	}

	public void PauseGame() {
		if (_started) {
			_paused = true;
			UpdateState();
		}
	}

	public void ResumeGame() {
		if (_started) {
			_paused = false;
			UpdateState();
		}
	}

	public void UpdateState() {
		bool running = gameRunning;
		Cursor.lockState = running ? CursorLockMode.Locked : CursorLockMode.None;
		Cursor.visible = !running;
		objective.SetActive(running);
		hud.SetActive(running);
	}

	void TestColorSpace() {
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
	}
}
