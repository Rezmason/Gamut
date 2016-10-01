using UnityEngine;
using System.Collections;

[SelectionBase]
public class ColorSpaceHandler : MonoBehaviour {
	void Start () {
		Shader.SetGlobalMatrix("_InvertedColorSpaceTransform", transform.worldToLocalMatrix);
	}
}
