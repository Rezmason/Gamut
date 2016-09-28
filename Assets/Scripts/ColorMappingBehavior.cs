using UnityEngine;
using System.Collections;

public class ColorMappingBehavior : MonoBehaviour {

	Material material;

	// Use this for initialization
	void Start () {
		Renderer renderer = GetComponent<Renderer>();
		Material commonMaterial = renderer.material;
		material = new Material(commonMaterial.shader);
		material.CopyPropertiesFromMaterial(commonMaterial);
		renderer.material = material;

		Update();
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 position = transform.position;
		material.color = new Color(
			position.x / 100 + 0.5f,
			position.y / 100 + 0.5f,
			position.z / 100 + 0.5f
		);
		Debug.Log(material.color);
	}
}
