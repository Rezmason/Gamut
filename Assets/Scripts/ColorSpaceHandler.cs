using UnityEngine;
using System.Collections;

public class ColorSpaceHandler : MonoBehaviour {
	void Start () {
		Shader.SetGlobalMatrix("_InvertedColorSpaceTransform", transform.worldToLocalMatrix);
	}
}
