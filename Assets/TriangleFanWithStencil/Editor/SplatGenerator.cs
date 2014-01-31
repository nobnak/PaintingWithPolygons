using UnityEngine;
using System.Collections;
using UnityEditor;

public static class SplatGenerator {
	[MenuItem("Assets/Custom/GenSimple")]
	public static void GenSimple() {
		var size = 10f;
		var n = 25;
		var radialSpeed = 0.1f;

		var dt = 2f * Mathf.PI / n;
		var vertices = new Vector3[n];
		var velocities = new Vector3[n];
		for (var i = 0; i < n; i++) {
			var p = new Vector3(Mathf.Cos(i * dt), Mathf.Sin(i * dt), 0f);
			vertices[i] = size * p;
			velocities[i] = radialSpeed * p;
		}

		var go = new GameObject("Splat");
		var splat = go.AddComponent<Splat>();
		splat.vertices = vertices;
		splat.velocities = velocities;
		PrefabUtility.CreatePrefab("Assets/TriangleFanWithStencil/Splat.prefab", go);
	}
}
