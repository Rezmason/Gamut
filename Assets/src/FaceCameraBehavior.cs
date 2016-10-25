using UnityEngine;

public class FaceCameraBehavior : MonoBehaviour
{
	public float spinRate = 40;
	public float baseScale = 10;
	public float scaleMag = 700;

	void Update()
	{
		Camera cam = Camera.current;
		if (cam == null) {
			return;
		}

		transform.LookAt(transform.position + cam.transform.rotation * Vector3.forward, cam.transform.rotation * Vector3.up);
		transform.Rotate(Vector3.forward * Time.time * spinRate);

		float dist = Vector3.Distance(transform.position, cam.transform.position);
		transform.localScale = Vector3.one * (baseScale + Mathf.Pow(scaleMag / dist, 2));
	}
}

