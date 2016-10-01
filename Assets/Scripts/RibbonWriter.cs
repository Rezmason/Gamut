using UnityEngine;
using System.Collections;

public class RibbonWriter : MonoBehaviour {

	public GameObject ribbon;

	Mesh mesh;

	Vector3[] vertices;
	Vector2[] edgeUVs;
	int[] indices;

	void Start () {

		MeshFilter mf = ribbon.GetComponent<MeshFilter>();
		mesh = new Mesh();
		mf.mesh = mesh;

		int n = 100;

		vertices = new Vector3[3 * (1 + n)];
		edgeUVs = new Vector2[3 * (1 + n)];
		indices = new int[3 * 4 * n];

		int vertStride = 3;
		int indexStride = 3 * 4;

		Vector3 EDGE_VEC = new Vector3(10, 0, 0);

		Vector3 pos = new Vector3();

		for (int i = 0; i < (1 + n); i++) {

			transform.Translate(0, 0, 20);
			transform.Rotate(new Vector3(10, 0, 0));

			// TODO: Move to a function. Call once at Start, and once every Update. i++
			vertices[i * vertStride + 1] = ribbon.transform.InverseTransformPoint(transform.TransformPoint(pos           ));
			vertices[i * vertStride + 0] = ribbon.transform.InverseTransformPoint(transform.TransformPoint(pos + EDGE_VEC));
			vertices[i * vertStride + 2] = ribbon.transform.InverseTransformPoint(transform.TransformPoint(pos - EDGE_VEC));

			// Set and forget. Only two of these will change on every update– the front middle vertex, and the vertex behind it
			edgeUVs[i * vertStride + 1].x = 1;
			edgeUVs[i * vertStride + 0].x = 2;
			edgeUVs[i * vertStride + 2].x = 2;

			// Set and forget. Only two of these will change on every update– the front triangle, and the triangles behind it

			if (i > 0) {
				indices[(i - 1) * indexStride + 0] = (i - 1) * vertStride + 0;
				indices[(i - 1) * indexStride + 1] = (i + 0) * vertStride + 1;
				indices[(i - 1) * indexStride + 2] = (i + 0) * vertStride + 0;

				indices[(i - 1) * indexStride + 3] = (i - 1) * vertStride + 0;
				indices[(i - 1) * indexStride + 4] = (i - 1) * vertStride + 1;
				indices[(i - 1) * indexStride + 5] = (i + 0) * vertStride + 1;

				indices[(i - 1) * indexStride + 6] = (i - 1) * vertStride + 1;
				indices[(i - 1) * indexStride + 7] = (i - 1) * vertStride + 2;
				indices[(i - 1) * indexStride + 8] = (i + 0) * vertStride + 1;

				indices[(i - 1) * indexStride +  9] = (i - 1) * vertStride + 2;
				indices[(i - 1) * indexStride + 10] = (i + 0) * vertStride + 2;
				indices[(i - 1) * indexStride + 11] = (i + 0) * vertStride + 1;
			}
		}

		mesh.vertices = vertices;
		mesh.uv = edgeUVs;
		mesh.triangles = indices;
	}

	void Update () {
		/*
		Vector3[] vertices = mesh.vertices;

		for (int i = 0; i < mesh.vertexCount; i++) {
			vertices[i].Set(Random.value * 1000, Random.value * 1000, Random.value * 1000);
		}

		mesh.vertices = vertices;
		*/
	}
}
