using UnityEngine;

public class HSVCylinderColorSpace : ColorSpace
{
	public HSVCylinderColorSpace()
	{
		Init(
			GameObject.Instantiate(Resources.Load("Prefabs/ColorSpaces/ColorSpace_HSV_Cylinder")) as GameObject,
			GameObject.Instantiate(Resources.Load("Materials/HSV_Cylinder_Material")) as Material,
			new Vector3(0, 500, 0)
		);
	}

	public override Color ColorFromWorldPosition(Vector3 worldPosition) {

		Vector3 localPosition = _origin.transform.worldToLocalMatrix.MultiplyVector(worldPosition);

		float hue = Mathf.Atan2(-localPosition.x, -localPosition.z) / (2 * Mathf.PI) + 0.5f;
		float sat = Mathf.Sqrt(localPosition.x * localPosition.x + localPosition.z * localPosition.z) * 2;
		float val = localPosition.y + 0.5f;

		return Color.HSVToRGB(hue, sat, val);
	}

	public override Vector3 GetRandomObjectivePosition()
	{
		float angle = Random.value * Mathf.PI * 2; // Random hue
		float mag = (1 - Mathf.Pow(Random.value, 2)) * 500; // Bias towards more saturated colors
		float val = (1 - Mathf.Pow(Random.value, 2)); // Bias towards brighter colors

		// Hey, nobody's impartial!

		return new Vector3(Mathf.Cos(angle) * mag, val, Mathf.Sin(angle) * mag);
	}
}
