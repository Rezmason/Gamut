using UnityEngine;
using System.Collections;

public class RibbonSystem : Thingleton<RibbonSystem>, ISystem {

	GameObject player;
	GameObject tail;
	public GameObject head;

	Mesh tailMesh;
	Mesh headMesh;

	Vector3[] tailVertices;
	Vector2[] tailEdgeUV2s;
	int[] tailIndices;

	Vector2[] headEdgeUV2s;

	int currentSegmentIndex;

	Vector3 lastLeftPosition;
	Vector3 lastRightPosition;
	Vector3 lastPosition;

	float totalDistanceTraveled;

	const float RIBBON_SCALE = 6f;
	const int TOTAL_TAIL_SEGMENTS = 10000;
	const int VERT_STRIDE = 3;
	const int INDEX_STRIDE = 3 * 4;
	const float MIN_DISTANCE = 3;
	readonly Vector3 LEFT_VEC = new Vector3(-RIBBON_SCALE, 0, -RIBBON_SCALE);
	readonly Vector3 MIDDLE_VEC = new Vector3(0, 0, -RIBBON_SCALE);
	readonly Vector3 RIGHT_VEC = new Vector3(RIBBON_SCALE, 0, -RIBBON_SCALE);
	GameState state;

	public void Setup() {
		state = GameState.instance;

		player = GameObject.FindWithTag("Player");
		head = player.transform.Find("RibbonHead").gameObject;

		head.transform.localScale = new Vector3(RIBBON_SCALE, RIBBON_SCALE, RIBBON_SCALE);
		head.transform.localPosition = new Vector3(0, 0, -RIBBON_SCALE);

		Material colorSpaceMaterial = state.activeColorSpace.material;

		head.GetComponent<MeshRenderer>().material = colorSpaceMaterial;

		tail = new GameObject();
		tail.AddComponent<MeshRenderer>().material = colorSpaceMaterial;
		MeshFilter tailMeshFilter = tail.AddComponent<MeshFilter>();
		tailMesh = new Mesh();
		tailMeshFilter.mesh = tailMesh;

		headMesh = head.GetComponent<MeshFilter>().mesh;

		tailVertices = new Vector3[VERT_STRIDE * (1 + TOTAL_TAIL_SEGMENTS)];
		tailEdgeUV2s = new Vector2[VERT_STRIDE * (1 + TOTAL_TAIL_SEGMENTS)];
		tailIndices = new int[INDEX_STRIDE * TOTAL_TAIL_SEGMENTS];
		headEdgeUV2s = headMesh.uv;

		for (int i = 0; i < (1 + TOTAL_TAIL_SEGMENTS); i++) {
			tailEdgeUV2s[i * VERT_STRIDE + 0].x = 2;
			tailEdgeUV2s[i * VERT_STRIDE + 1].x = 1;
			tailEdgeUV2s[i * VERT_STRIDE + 2].x = 2;
		}

		Reset();
	}

	public void Reset() {
		for (int i = 0; i < (1 + TOTAL_TAIL_SEGMENTS); i++) {
			PositionTailSegment(i);
			FillTailSegment(i);
		}
		currentSegmentIndex = 0;
		totalDistanceTraveled = 0;
		UpdateTailMesh();
	}

	int PreviousTailSegment(int index) {
		return (TOTAL_TAIL_SEGMENTS + index - 1) % TOTAL_TAIL_SEGMENTS;
	}

	int NextTailSegment(int index) {
		return (TOTAL_TAIL_SEGMENTS + index + 1) % TOTAL_TAIL_SEGMENTS;
	}

	public void Update () {
		if (!state.gameRunning) {
			head.SetActive(false);
			tail.SetActive(false);
			return;
		}

		head.SetActive(true);
		tail.SetActive(true);

		Transform transform = player.transform;

		Vector3 leftPosition = transform.TransformPoint(LEFT_VEC);
		Vector3 rightPosition = transform.TransformPoint(RIGHT_VEC);

		float distance = Vector3.Distance(leftPosition, lastLeftPosition) + Vector3.Distance(rightPosition, lastRightPosition);

		if (distance > MIN_DISTANCE) {
			lastLeftPosition = leftPosition;
			lastRightPosition = rightPosition;

			currentSegmentIndex = NextTailSegment(currentSegmentIndex);
			FillTailSegment(currentSegmentIndex);
			ClearTailSegment(NextTailSegment(currentSegmentIndex));
		}

		Vector3 position = transform.TransformPoint(MIDDLE_VEC);
		float distanceTraveled = Vector3.Distance(lastPosition, position);
		totalDistanceTraveled += distanceTraveled;
		float speed = distanceTraveled / Time.deltaTime;
		lastPosition = position;

		float dash = Mathf.Sin(totalDistanceTraveled * 0.13f) * 0.5f + 1.5f + (speed - 50) * 0.003f;
		float swoop = Mathf.Sin(totalDistanceTraveled * 0.06f) * 3f;

		for (int i = 0; i < headMesh.vertexCount; i++) {
			headEdgeUV2s[i].y = dash;
		}
		head.transform.localPosition = new Vector3(0, swoop, -RIBBON_SCALE);

		PositionTailSegment(currentSegmentIndex, dash, swoop);
		UpdateTailMesh();
	}

	public void Run() {

	}

	void PositionTailSegment(int index, float dash = 0, float swoop = 0) {
		Transform transform = player.transform;
		tailVertices[index * VERT_STRIDE + 1] = tail.transform.InverseTransformPoint(transform.TransformPoint(MIDDLE_VEC) + transform.up * swoop);
		tailVertices[index * VERT_STRIDE + 0] = tail.transform.InverseTransformPoint(transform.TransformPoint(  LEFT_VEC) + transform.up * swoop);
		tailVertices[index * VERT_STRIDE + 2] = tail.transform.InverseTransformPoint(transform.TransformPoint( RIGHT_VEC) + transform.up * swoop);

		tailEdgeUV2s[index * VERT_STRIDE + 1].y = dash;
		tailEdgeUV2s[index * VERT_STRIDE + 0].y = dash;
		tailEdgeUV2s[index * VERT_STRIDE + 2].y = dash;
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
		tailMesh.uv2 = tailEdgeUV2s;
		tailMesh.triangles = tailIndices;
		tailMesh.RecalculateBounds();

		headMesh.uv2 = headEdgeUV2s;
	}
}
