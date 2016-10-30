﻿using UnityEngine;
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
	float timeRemaining;

	GameState state;

	public void Setup () {

		state = GameState.instance;
		SetupColorSpaces();
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
		objective.AddComponent<FaceCameraBehavior>().scaleMag = 700;

		scoreBurst = GameObject.Instantiate(Resources.Load("Prefabs/ScoreBurst") as GameObject);
		scoreParticles = scoreBurst.GetComponent<ParticleSystem>();
		scoreBurst.transform.position = subject.transform.position;
		scoreParticles.Stop();

		UpdateState();
	}

	public void Update() {
		if (state.gameRunning) {
			timeRemaining = Mathf.Max(0, timeRemaining - Time.deltaTime);
			tClock.text = timeRemaining.ToString("0.00");

			state.activeColorSpace.Rotate(new Vector3(0, state.score * 0.01f, 0));
			UpdateSpotColor();

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
		swatch.GetComponent<Image>().color = color;
	}

	void ResetTime() {
		timeRemaining = 10 + 20 / (state.score * 0.1f + 1);
		tClock.text = timeRemaining.ToString("0.00");
	}

	void SetCheckpoint() {
		Vector3 position = objective.transform.position;
		Vector3 nextPosition = position;

		while (Vector3.Distance(nextPosition, position) < MIN_DISTANCE) {
			nextPosition = state.activeColorSpace.GetRandomObjectivePosition();
		}

		objective.transform.position = nextPosition;
		UpdateSpotColor();
	}

	void RespondToCollision() {
		state.SetScore(state.score + 1);
		scoreBurst.transform.position = objective.transform.position;
		scoreParticles.Clear();
		scoreParticles.Play();
		tScore.text = state.score.ToString();
		SetCheckpoint();
		ResetTime();
	}

	public void StartGame() {
		state.SetStarted(true);
		state.SetPaused(false);

		state.SetScore(0);
		ResetTime();
		tScore.text = state.score.ToString();
		SetCheckpoint();

		UpdateState();
		startGame();
	}

	void EndGame() {
		state.SetStarted(false);
		UpdateState();
		endGame();
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
