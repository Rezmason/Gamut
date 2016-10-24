using UnityEngine;

public abstract class ColorSpace
{
	protected GameObject _gameObject;
	public GameObject gameObject {
		get { return _gameObject; }
	}

	private bool _active;
	public bool active {
		get { return _active; }
		set { 
			_active = value;
			_gameObject.SetActive(value);
			Shader.SetGlobalMatrix("_InvertedColorSpaceTransform", _gameObject.transform.worldToLocalMatrix);
			Debug.Log("Active color space: " + this);
		}
	}

	protected void Init(GameObject gameObject) {
		_gameObject = gameObject;
		_gameObject.SetActive(_active);
	}

	public abstract Color ColorFromWorldPosition(Vector3 position);

	public abstract Vector3 GetRandomObjectivePosition();
}