using UnityEngine;
using System.Collections;

public class Updater : MonoBehaviour
{
	public event SimpleDelegate updateMethod;

	void Update()
	{
		if (updateMethod != null) {
			updateMethod();
		}
	}
}

