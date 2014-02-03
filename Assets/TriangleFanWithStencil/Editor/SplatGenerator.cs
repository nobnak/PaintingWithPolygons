using UnityEngine;
using System.Collections.Generic;
using UnityEditor;

public static class SplatGenerator {
	[MenuItem("Assets/Custom/GenSimpleBrush")]
	public static void GenSimpleBrush() {
		var width = 45;

		var brush = CreateSimpleBrush (width);
		var splat = CreateSplat(Vector3.zero, width);
		splat.transform.parent = brush.transform;
		PrefabUtility.CreatePrefab("Assets/TriangleFanWithStencil/SimpleBrush.prefab", brush);
	}

	[MenuItem("Assets/Custom/GenWetOnDryBrush")]
	public static void GenWetOnDryBrush() {
	}

	[MenuItem("Assets/Custom/GenCrunchyBrush")]
	public static void GenCruncyBrush() {
		var width = 45;

		var brush = CreateSimpleBrush(width * 2);
		var splat = CreateSplat(Vector3.zero, width, 15, 5, 0.25f);
		splat.transform.parent = brush.transform;
		PrefabUtility.CreatePrefab("Assets/TriangleFanWithStencil/CrunchyBrush.prefab", brush);
	}

	public static GameObject CreateSimpleBrush (int width) {
		var r = width / 2;
		var xs = new List<int> ();
		var ys = new List<int> ();
		for (var y = -r; y <= r; y++) {
			for (var x = -r; x <= r; x++) {
				var sqr = x * x + y * y;
				if ((r * r) < sqr)
					continue;
				xs.Add (x);
				ys.Add (y);
			}
		}
		var go = new GameObject ("Simple Brush");
		var brush = go.AddComponent<Brush> ();
		brush.waterRegionXs = xs.ToArray ();
		brush.waterRegionYs = ys.ToArray ();
		return go;
	}

	public static GameObject CreateSplat(Vector3 offset, int width) {
		return CreateSplat(offset, width, 30, 1f, 1f);
	}
	public static GameObject CreateSplat(Vector3 offset, int width, int life, float roughness, float flow) {
		var r = width / 2;
		var n = 25;
		var radialSpeed = 2f;
		
		var dt = 2f * Mathf.PI / n;
		var vertices = new Vector3[n];
		var velocities = new Vector3[n];
		for (var i = 0; i < n; i++) {
			var p = new Vector3(Mathf.Cos(i * dt), Mathf.Sin(i * dt), 0f);
			vertices[i] = (float)r * p + offset;
			velocities[i] = radialSpeed * p;
		}
		
		var go = new GameObject("Splat");
		var splat = go.AddComponent<Splat>();
		splat.vertices = vertices;
		splat.velocities = velocities;
		splat.initSize = splat.CalcSize();
		splat.life = life;
		splat.roughness = roughness;
		splat.flow = flow;
		splat.initColor = Color.magenta;
		return go;
	}
}
