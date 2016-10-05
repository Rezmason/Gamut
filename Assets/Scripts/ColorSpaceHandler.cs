using UnityEngine;
using System.Collections;
using UnityEngine.Rendering;

[SelectionBase]
public class ColorSpaceHandler : MonoBehaviour {
	void Start () {
		Shader.SetGlobalMatrix("_InvertedColorSpaceTransform", transform.worldToLocalMatrix);

//		Debug.Log((int)BlendMode.One);
	}
}
