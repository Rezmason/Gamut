using UnityEngine;
using System.Collections;

public class RibbonWriter : MonoBehaviour {

	public GameObject tail;
	public GameObject head;

	Mesh tailMesh;

	Vector3[] tailVertices;
	Vector2[] tailEdgeUVs;
	int[] tailIndices;

	int currentSegmentIndex;

	Vector3 lastLeftPosition;
	Vector3 lastRightPosition;

	const int TOTAL_TAIL_SEGMENTS = 1000;
	const int VERT_STRIDE = 3;
	const int INDEX_STRIDE = 3 * 4;
	const float MIN_DISTANCE = 3;
	readonly Vector3 EDGE_VEC = new Vector3(10, 0, 0);
	readonly Vector3 MIDDLE_VEC = new Vector3();


	void Start () {
		
		MeshFilter tailMeshFilter = tail.GetComponent<MeshFilter>();
		tailMesh = new Mesh();
		tailMeshFilter.mesh = tailMesh;

		tailVertices = new Vector3[VERT_STRIDE * (1 + TOTAL_TAIL_SEGMENTS)];
		tailEdgeUVs = new Vector2[VERT_STRIDE * (1 + TOTAL_TAIL_SEGMENTS)];
		tailIndices = new int[INDEX_STRIDE * TOTAL_TAIL_SEGMENTS];

		int i;

		for (i = 0; i < (1 + TOTAL_TAIL_SEGMENTS); i++) {
			tailEdgeUVs[i * VERT_STRIDE + 0].x = 2;
			tailEdgeUVs[i * VERT_STRIDE + 1].x = 1;
			tailEdgeUVs[i * VERT_STRIDE + 2].x = 2;
			PositionTailSegment(i);
			FillTailSegment(i);
		}

		currentSegmentIndex = 0;

		UpdateTailMesh();
	}

	int PreviousTailSegment(int index) {
		return (TOTAL_TAIL_SEGMENTS + index - 1) % TOTAL_TAIL_SEGMENTS;
	}

	int NextTailSegment(int index) {
		return (TOTAL_TAIL_SEGMENTS + index + 1) % TOTAL_TAIL_SEGMENTS;
	}

	void Update () {
		Vector3  leftPosition = transform.TransformPoint( EDGE_VEC);
		Vector3 rightPosition = transform.TransformPoint(-EDGE_VEC);
		float distance = Vector3.Distance(leftPosition, lastLeftPosition) + Vector3.Distance(rightPosition, lastRightPosition);

		if (distance > MIN_DISTANCE) {
			lastLeftPosition  = leftPosition;
			lastRightPosition = rightPosition;

			currentSegmentIndex = NextTailSegment(currentSegmentIndex);
			FillTailSegment(currentSegmentIndex);
			ClearTailSegment(NextTailSegment(currentSegmentIndex));
		}

		PositionTailSegment(currentSegmentIndex);
		UpdateTailMesh();
	}

	void PositionTailSegment(int index) {
		tailVertices[index * VERT_STRIDE + 1] = tail.transform.InverseTransformPoint(transform.TransformPoint(MIDDLE_VEC));
		tailVertices[index * VERT_STRIDE + 0] = tail.transform.InverseTransformPoint(transform.TransformPoint(  EDGE_VEC));
		tailVertices[index * VERT_STRIDE + 2] = tail.transform.InverseTransformPoint(transform.TransformPoint( -EDGE_VEC));
	}

	void FillTailSegment(int index) {
		int lastIndex = PreviousTailSegment(index);
		tailIndices[lastIndex * INDEX_STRIDE +  0] = lastIndex * VERT_STRIDE + 0;
		tailIndices[lastIndex * INDEX_STRIDE +  1] = index     * VERT_STRIDE + 1;
		tailIndices[lastIndex * INDEX_STRIDE +  2] = index     * VERT_STRIDE + 0;

		tailIndices[lastIndex * INDEX_STRIDE +  3] = lastIndex * VERT_STRIDE + 0;
		tailIndices[lastIndex * INDEX_STRIDE +  4] = lastIndex * VERT_STRIDE + 1;
		tailIndices[lastIndex * INDEX_STRIDE +  5] = index     * VERT_STRIDE + 1;

		tailIndices[lastIndex * INDEX_STRIDE +  6] = lastIndex * VERT_STRIDE + 1;
		tailIndices[lastIndex * INDEX_STRIDE +  7] = lastIndex * VERT_STRIDE + 2;
		tailIndices[lastIndex * INDEX_STRIDE +  8] = index     * VERT_STRIDE + 1;

		tailIndices[lastIndex * INDEX_STRIDE +  9] = lastIndex * VERT_STRIDE + 2;
		tailIndices[lastIndex * INDEX_STRIDE + 10] = index     * VERT_STRIDE + 2;
		tailIndices[lastIndex * INDEX_STRIDE + 11] = index     * VERT_STRIDE + 1;
	}

	void ClearTailSegment(int index) {
		int lastIndex = PreviousTailSegment(index);
		for (int i = 0; i < INDEX_STRIDE; i++) {
			tailIndices[lastIndex * INDEX_STRIDE + i] = lastIndex * VERT_STRIDE + 1;
		}
	}

	void UpdateTailMesh() {
		tailMesh.vertices = tailVertices;
		tailMesh.uv = tailEdgeUVs;
		tailMesh.triangles = tailIndices;
		tailMesh.RecalculateBounds();
	}
}
