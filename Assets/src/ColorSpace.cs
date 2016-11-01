using UnityEngine;

public abstract class ColorSpace
{
	protected GameObject _gameObject;
	public GameObject gameObject {
		get { 
			return _gameObject; 
		}
	}
	protected GameObject _origin;
	protected ParticleSystem _particleSystem;
	protected ParticleSystem.Particle[] _particles;

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
			Shader.SetGlobalMatrix("_InvertedColorSpaceTransform", _origin.transform.worldToLocalMatrix);
		}
	}

	protected void Init(GameObject gameObject, Material material, Vector3 whitePointPosition) {
		_gameObject = gameObject;
		_gameObject.SetActive(_active);
		_origin = _gameObject.transform.Find("Origin").gameObject;
		_material = material;
		_particleSystem = _gameObject.transform.Find("FreeParticles").gameObject.GetComponent<ParticleSystem>();
		_particles = new ParticleSystem.Particle[_particleSystem.maxParticles];
		_whitePointPosition = whitePointPosition;
	}

	public abstract Color ColorFromWorldPosition(Vector3 position);

	public abstract Vector3 GetRandomObjectivePosition();

	public void Rotate(Vector3 eulerAngles) {
		_origin.transform.Rotate(eulerAngles);
		Shader.SetGlobalMatrix("_InvertedColorSpaceTransform", _origin.transform.worldToLocalMatrix);
	}

	public void SetParticleSize(float size, bool forceCurrentParticles) {
		_particleSystem.startSize = size;
		if (forceCurrentParticles) {
			int len = _particleSystem.GetParticles(_particles);

			for (int i = 0; i < len; i++) {
				_particles[i].startSize = size;
			}

			_particleSystem.SetParticles(_particles, len);
		}
	}
}