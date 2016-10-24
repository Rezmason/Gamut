using UnityEngine;

public abstract class ColorSpace
{
	protected GameObject _gameObject;
	public GameObject gameObject {
		get { 
			return _gameObject; 
		}
	}

	protected Material _material;
	public Material material {
		get {
			return _material;
		}
	}

	protected Vector3 _whitePointPosition;
	public Vector3 whitePointPosition {
		get {
			return _whitePointPosition;
		}
	}

	private bool _active;
	public bool active {
		get { return _active; }
		set { 
			_active = value;
			_gameObject.SetActive(value);
			Shader.SetGlobalMatrix("_InvertedColorSpaceTransform", _gameObject.transform.worldToLocalMatrix);
		}
	}

	protected void Init(GameObject gameObject, Material material, Vector3 whitePointPosition) {
		_gameObject = gameObject;
		_gameObject.SetActive(_active);
		_material = material;
		_whitePointPosition = whitePointPosition;
	}

	public abstract Color ColorFromWorldPosition(Vector3 position);

	public abstract Vector3 GetRandomObjectivePosition();
}