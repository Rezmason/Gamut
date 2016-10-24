using UnityEngine;
using System.Collections;

public class FaceCameraBehavior : MonoBehaviour
{
	public GameObject mainCamera;
	public float spinRate = 40;

	void Update()
	{
		transform.LookAt(transform.position + mainCamera.transform.rotation * Vector3.forward, mainCamera.transform.rotation * Vector3.up);
		transform.Rotate(Vector3.forward * Time.time * spinRate);
	}
}

