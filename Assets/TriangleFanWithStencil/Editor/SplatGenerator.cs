using UnityEngine;
using System.Collections;
using UnityEditor;

public static class SplatGenerator {
	[MenuItem("Assets/Custom/GenSimple")]
	public static void GenSimple() {
		var width = 10f;

		var n = 25;
		var radialSpeed = 10f;

		var dt = 2f * Mathf.PI / n;
		var vertices = new Vector3[n];
		var velocities = new Vector3[n];
		for (var i = 0; i < n; i++) {
			var p = new Vector3(Mathf.Cos(i * dt), Mathf.Sin(i * dt), 0f);
			vertices[i] = width * p;
			velocities[i] = radialSpeed * p;
		}

		var go = new GameObject("Splat");
		var splat = go.AddComponent<Splat>();
		splat.vertices = vertices;
		splat.velocities = velocities;
		splat.initSize = splat.CalcSize();
		splat.life = 30;
		splat.roughness = 1;
		splat.flow = 1;
		PrefabUtility.CreatePrefab("Assets/TriangleFanWithStencil/Splat.prefab", go);
	}
}
