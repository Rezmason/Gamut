using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class Context
{
	static List<GameObject> colorSpaces;
	static int activeColorSpace;

	public static void Init(GameObject gameObject)
	{
		GameObject.Instantiate(Resources.Load("Prefabs/EventSystem"));
		GameObject.Instantiate(Resources.Load("Prefabs/GUI"));
		// TODO: populate rest of the scene from prefabs
		colorSpaces = new List<GameObject>();
		colorSpaces.Add(GameObject.Instantiate(Resources.Load("Prefabs/ColorSpaces/ColorSpace_HSV_Cylinder")) as GameObject);
		colorSpaces.Add(GameObject.Instantiate(Resources.Load("Prefabs/ColorSpaces/ColorSpace_RGB_Cube")) as GameObject);

		foreach (GameObject colorSpace in colorSpaces) {
			colorSpace.SetActive(false);
		}

		activeColorSpace = 0;
		colorSpaces[activeColorSpace].SetActive(true);

		GameSystem.instance.Init();
		// TODO: Turn RibbonWriter script into RibbonDancer system
	}
}
