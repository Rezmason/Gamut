using UnityEngine;
using UnityEngine.SceneManagement;

public class Context
{
	static Scene scene;

	public static void Init(GameObject gameObject)
	{
		CheckpointSystem.instance.Init();
		// TODO: populate scene from prefabs
	}
}
