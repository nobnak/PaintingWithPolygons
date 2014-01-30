using UnityEngine;
using System.Collections;

public class Splat : MonoBehaviour {
	public Vector3[] vertices;
	public Mesh mesh;

	// Use this for initialization
	void Start () {
		mesh = new Mesh();
		if (vertices == null)
			vertices = new Vector3[0];
		var mf = GetComponent<MeshFilter>();
		if (mf != null)
			mf.sharedMesh = mesh;
	}
	
	// Update is called once per frame
	void Update () {
		if (!Input.GetMouseButtonDown(0))
			return;

		var pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		pos.z = transform.position.z;
		var prevVertices = vertices;
		vertices = new Vector3[prevVertices.Length + 1];
		System.Array.Copy(prevVertices, vertices, prevVertices.Length);
		vertices[prevVertices.Length] = pos;

		if (vertices.Length < 3)
			return;
		var counter = 0;
		var fanTriangles = new int[3 * (vertices.Length - 2)];
		for (var i = 2; i < vertices.Length; i++) {
			fanTriangles[counter++] = 0;
			fanTriangles[counter++] = i - 1;
			fanTriangles[counter++] = i;
		}

		mesh.Clear();
		mesh.vertices = vertices;
		mesh.triangles = fanTriangles;
		mesh.RecalculateBounds();
	}
}
