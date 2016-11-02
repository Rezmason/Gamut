using UnityEngine;
using System.Collections.Generic;

public class GameState : Thingleton<GameState> {
	int _score;
	public int score { get { return _score; } }
	public void SetScore (int value) { _score = value; }

	bool _started = false;
	public bool started { get { return _started; } }
	public void SetStarted (bool value) { _started = value; }

	bool _paused = false;
	public bool paused { get { return _paused; } }
	public void SetPaused (bool value) { _paused = value; }

	public bool gameRunning { get { return _started && !_paused; } }

	List<ColorSpace> colorSpaces;
	public void SetColorSpaces(List<ColorSpace> value) { colorSpaces = value; }
	int activeColorSpaceIndex;
	public void SetActiveColorSpaceIndex(int value) { activeColorSpaceIndex = value; }
	public ColorSpace activeColorSpace { get { return colorSpaces[activeColorSpaceIndex]; } }

	float _speed = 0;
	public float speed { get { return _speed; } }
	public void SetSpeed(float value) { _speed = value; }

}

