﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CheckpointSystem : MonoSingleton<CheckpointSystem> {

	public static CheckpointSystem Instance;

	public GameObject objective;
	public GameObject swatch;
	public GameObject colorSpace;

	Color color;

	void Start () {
		Instance = this;

		color = new Color();
		color.a = 1;
		SetCheckpoint();
	}

	void SetCheckpoint() {
		color.r = Random.value;
		color.g = Random.value;
		color.b = Random.value;
		swatch.GetComponent<Image>().color = color;
	}

	// Update is called once per frame
	void Update () {
		
	}
}