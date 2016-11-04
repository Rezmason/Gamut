using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class GameSystem : Thingleton<GameSystem>, ISystem {

	public event SimpleDelegate endGame;
	public event SimpleDelegate startGame;
	const float MIN_DISTANCE = 200;
	GameObject objective;
	GameObject nib;
	GameObject nibArt;
	Vector3 nibStartPosition;
	float nibPeek;
	Vector3 nibStartAngles;
	Text tScore;
	Text tClock;
	Text tHelp;
	GameObject player;
	GameObject hud;
	GameObject scoreBurst;
	ObjectiveBehavior objectiveBehavior;
	ParticleSystem scoreParticles;
	float timeRemaining;
	float time;

	GameState state;

	public void Setup () {

		state = GameState.instance;
		SetupColorSpaces();

		player = GameObject.FindWithTag("Player");
		nib = player.transform.Find("MainCamera/Nib").gameObject;
		nibArt = nib.transform.Find("NibArt").gameObject;
		nibStartPosition = nib.transform.localPosition;
		nibStartAngles = nib.transform.localEulerAngles;
		hud = nib.transform.Find("HUD").gameObject;
		tScore = hud.transform.Find("Score").gameObject.GetComponent<Text>();
		tClock = hud.transform.Find("Clock").gameObject.GetComponent<Text>();
		tHelp = hud.transform.Find("HelpText").gameObject.GetComponent<Text>();

		tHelp.text = "HELLO";

		objective = GameObject.Instantiate(Resources.Load("Prefabs/Objective") as GameObject);
		objectiveBehavior = objective.AddComponent<ObjectiveBehavior>();
		objectiveBehavior.collisionHandler += RespondToCollision;
		objectiveBehavior.subject = player;
		objective.AddComponent<FaceCameraBehavior>().scaleMag = 700;

		scoreBurst = GameObject.Instantiate(Resources.Load("Prefabs/ScoreBurst") as GameObject);
		scoreParticles = scoreBurst.GetComponent<ParticleSystem>();
		scoreBurst.transform.position = player.transform.position;
		scoreParticles.Stop();

		UpdateState();
	}

	public void Update() {
		if (state.gameRunning) {
			timeRemaining = Mathf.Max(0, timeRemaining - Time.deltaTime);
			tClock.text = timeRemaining.ToString("00.0");

			state.activeColorSpace.Rotate(new Vector3(0, state.score * 0.01f, 0));
			UpdateSpotColor();
			UpdateNib();
			if (timeRemaining == 0) {
				EndGame();
			}
		}
	}

	public void Run() {

	}

	void SetupColorSpaces() {
		List<ColorSpace> colorSpaces = new List<ColorSpace>();
		colorSpaces.Add(new HSVCylinderColorSpace());
		colorSpaces.Add(new RGBCubeColorSpace());
		state.SetColorSpaces(colorSpaces);
		state.SetActiveColorSpaceIndex(0);
		state.activeColorSpace.active = true;
	}

	void UpdateSpotColor() {
		Color color = state.activeColorSpace.ColorFromWorldPosition(objective.transform.position);
		objective.GetComponent<MeshRenderer>().material.color = color;
		nibArt.GetComponent<Renderer>().material.color = color;
	}

	void ResetTime() {
		timeRemaining = 10 + 20 / (state.score * 0.1f + 1);
		tClock.text = timeRemaining.ToString("00.0");
	}

	void SetCheckpoint() {
		Vector3 position = objective.transform.position;
		Vector3 nextPosition = position;

		float interp = Mathf.Min(1, 1 - 1 / (state.score * 0.1f + 1));

		Vector3 easyPosition = player.transform.position;
		float easyAngle = Random.value * Mathf.PI * 2;
		easyPosition += player.transform.forward * MIN_DISTANCE;
		easyPosition += player.transform.right * 80 * Mathf.Cos(easyAngle);
		easyPosition += player.transform.up    * 80 * Mathf.Sin(easyAngle);

		int triesLeft = 20;
		while (Vector3.Distance(nextPosition, position) < MIN_DISTANCE && triesLeft != 0) {
			nextPosition = Vector3.Lerp(easyPosition, state.activeColorSpace.GetRandomObjectivePosition(), interp);
			triesLeft--;
		}

		objective.transform.position = nextPosition;
		objective.GetComponent<FaceCameraBehavior>().baseScale = 10 + 100 / (state.score + 1);
		UpdateParticleSize();
		UpdateSpotColor();
	}

	void RespondToCollision() {
		state.SetScore(state.score + 1);
		scoreBurst.transform.position = objective.transform.position;
		scoreParticles.Clear();
		scoreParticles.Play();
		tScore.text = state.score.ToString("000");
		SetCheckpoint();
		ResetTime();
	}

	public void StartGame() {
		time = 0;
		state.SetStarted(true);
		state.SetPaused(false);

		player.transform.position = Vector3.back * MIN_DISTANCE;
		player.transform.LookAt(Vector3.zero);
		state.activeColorSpace.Reset();
		state.SetScore(0);
		ResetTime();
		tScore.text = state.score.ToString("000");
		SetCheckpoint();
		nibPeek = 1;

		UpdateState();
		if (startGame != null) {
			startGame();
		}
	}

	void EndGame() {
		state.SetStarted(false);
		UpdateState();
		if (endGame != null) {
			endGame();
		}
	}

	public void AbortGame() {
		state.SetStarted(false);
		UpdateState();
	}

	public void PauseGame() {
		if (state.started) {
			state.SetPaused(true);
			UpdateState();
		}
	}

	public void ResumeGame() {
		if (state.started) {
			state.SetPaused(false);
			UpdateState();
		}
	}

	public void UpdateState() {
		bool running = state.gameRunning;
		Cursor.lockState = running ? CursorLockMode.Locked : CursorLockMode.None;
		Cursor.visible = !running;
		objective.SetActive(running);
		hud.SetActive(running);
		nib.SetActive(running);
		UpdateParticleSize(true);
	}

	void UpdateParticleSize(bool forceCurrentParticles = false) {
		state.activeColorSpace.SetParticleSize(state.gameRunning ? 0.01f * (1 - 1 / (state.score + 1.1f)) : 0.01f, forceCurrentParticles);
	}

	void UpdateNib() {

		time += Time.deltaTime * state.speed;

		Vector3 nibPosition = nibStartPosition;
		nibPosition.y += Mathf.Sin(time * 0.1f) * 2 * state.speed / 50;
		nibPeek *= 0.95f;
		nibPosition.z += nibPeek * -50;
		nib.transform.localPosition = nibPosition;

		Vector3 angles = new Vector3();
		angles.x -= Mathf.Cos(time * 0.1f) * 2 * state.speed / 50;

		angles.x += state.eulerAngles.x * 0.07f;
		angles.y += state.eulerAngles.y * 0.05f;
		angles.z += state.eulerAngles.z * 0.2f - state.eulerAngles.y * 0.08f;

		nib.transform.localEulerAngles = nibStartAngles + angles;
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
			copy.GetComponent<MeshRenderer>().material.color = state.activeColorSpace.ColorFromWorldPosition(pos);
		}
	}
}
