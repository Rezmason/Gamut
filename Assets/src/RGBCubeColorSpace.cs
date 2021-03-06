﻿using UnityEngine;

public class RGBCubeColorSpace : ColorSpace
{
	public RGBCubeColorSpace()
	{
		Init(
			GameObject.Instantiate(Resources.Load("Prefabs/ColorSpaces/ColorSpace_RGB_Cube")) as GameObject,
			GameObject.Instantiate(Resources.Load("Materials/RGB_Cube_Material")) as Material,
			Vector3.zero
		);
	}
		
	public override Color ColorFromWorldPosition(Vector3 position) {
		return Color.white;
	}

	public override Vector3 GetRandomObjectivePosition()
	{
		return Vector3.zero;
	}
}

