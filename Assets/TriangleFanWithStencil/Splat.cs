using UnityEngine;
using System.Collections;

public class Splat : MonoBehaviour {
	public const float alpha = 0.33f;

	public Vector3[] vertices;
	public Vector3 motionBias;
	public Vector3[] velocities;
	public int life;
	public float roughness;
	public float flow;

	public Mesh mesh;
	public float startTime;

	public float initSize;
	public Color initColor = Color.white;

	void Start () {
		mesh = new Mesh();
		startTime = Time.timeSinceLevelLoad;

		if (vertices == null)
			vertices = new Vector3[0];
		var mf = GetComponent<MeshFilter>();
		if (mf != null)
			mf.sharedMesh = mesh;

		UpdateMesh();
	}

	public void UpdateShape(SplatRenderer.WetMap wetMap) {
		if (life <= 0)
			return;
		life--;

		for (var i = 0; i < vertices.Length; i++) {
			var x = vertices[i];
			var v = velocities[i];
			var d = (1f - alpha) * motionBias + alpha / Random.Range(1f, 1f + roughness) * v;
			var x1 = x + flow * d + (Vector3)(roughness * Random.insideUnitCircle);
			var w = wetMap.GetWater((int)x1.x, (int)x1.y);
			if (w > 0)
				vertices[i] = x1;
		}
		UpdateMesh();
	}


	public float CalcSize() {
		if (vertices.Length < 3)
			return 0f;

		var v0 = vertices[0];
		var v0x = v0.x;
		var v0y = v0.y;
		var e0 = vertices[1] - v0;
		var e0x = e0.x;
		var e0y = e0.y;
		var s = 0f;
		var length = vertices.Length;
		for (var i = 2; i < length; i++) {
			var v2 = vertices[i];
			var e1x = v2.x - v0x;
			var e1y = v2.y - v0y;
			s += e0x * e1y - e0y * e1x;
			e0x = e1x;
			e0y = e1y;
		}
		return s >= 0 ? s : -s;
	}
	public Color GetColor() {
		var c = initColor;
		c.a *= initSize / CalcSize();
		return c;
	}
	
	void UpdateMesh () {
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
