using UnityEngine;
using System.Collections;

public class SplatRenderer : MonoBehaviour {
	public Material splatMat;
	public Material postEffectMat;
	public Splat[] splats;

	private Mesh _rectangle;

	// Use this for initialization
	void Start () {
		if (splats == null || splats.Length == 0)
			splats = FindObjectsOfType<Splat>();

		_rectangle = new Mesh();
		_rectangle.vertices = new Vector3[]{ 
			new Vector3(-1e6f, -1e6f, 0f), 
			new Vector3(1e6f, -1e6f, 0f), 
			new Vector3(-1e6f, 1e6f, 0f), 
			new Vector3(1e6f, 1e6f, 0f) };
		var triangles = new int[6];
		triangles[0] = 0;
		triangles[1] = 3;
		triangles[2] = 1;
		triangles[3] = 0;
		triangles[4] = 2;
		triangles[5] = 3;
		_rectangle.triangles = triangles;
		_rectangle.bounds = new Bounds(Vector3.zero, float.MaxValue * Vector3.one);
	}

	void OnPostRender() {
		foreach (var splat in splats) {
			GL.Clear(true, false, Color.black);

			splatMat.SetPass(0);
			Graphics.DrawMeshNow(splat.mesh, Matrix4x4.identity);

			var bounds = splat.mesh.bounds;
			var rectVertices = _rectangle.vertices;
			var v0 = bounds.min;
			var v3 = bounds.max;
			rectVertices[0] = v0;
			rectVertices[1] = new Vector3(v3.x, v0.y, 0f);
			rectVertices[2] = new Vector3(v0.x, v3.y, 0f);
			rectVertices[3] = v3;
			_rectangle.vertices = rectVertices;

			splatMat.SetPass(1);
			Graphics.DrawMeshNow(_rectangle, Matrix4x4.identity);

		}
	}
}
