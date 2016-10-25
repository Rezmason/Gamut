using UnityEngine;
using System.Collections;

public class Updater : MonoBehaviour
{
	public delegate void updateMethodType();
	public event updateMethodType updateMethod;

	void Update()
	{
		if (updateMethod != null) {
			updateMethod();
		}
	}
}

