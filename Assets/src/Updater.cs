using UnityEngine;
using System.Collections;

public class Updater : MonoBehaviour
{
	public delegate void updateMethodType();
	public updateMethodType updateMethod;

	void Update()
	{
		updateMethod();
	}
}

