using UnityEngine;

public class HSVCylinderColorSpace : ColorSpace
{
	public HSVCylinderColorSpace()
	{
		Init(GameObject.Instantiate(Resources.Load("Prefabs/ColorSpaces/ColorSpace_HSV_Cylinder")) as GameObject);
	}

	public override Color ColorFromWorldPosition(Vector3 worldPosition) {

		Vector3 localPosition = _gameObject.transform.worldToLocalMatrix.MultiplyVector(worldPosition);

		float hue = Mathf.Atan2(-localPosition.x, -localPosition.z) / (2 * Mathf.PI) + 0.5f;
		float sat = Mathf.Sqrt(localPosition.x * localPosition.x + localPosition.z * localPosition.z) * 2;
		float val = localPosition.y + 0.5f;

		return Color.HSVToRGB(hue, sat, val);
	}

	public override Vector3 GetRandomObjectivePosition()
	{
		return new Vector3(Random.value - 0.5f, Random.value - 0.5f, Random.value - 0.5f) * 500;
	}
}
