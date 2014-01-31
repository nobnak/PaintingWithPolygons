﻿using UnityEngine;
using System.Collections;

public class SplatPainter : MonoBehaviour {
	public GameObject splatfab;

	private SplatRenderer _splatRenderer;

	// Use this for initialization
	void Start () {
		_splatRenderer = GetComponent<SplatRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
		if (!Input.GetMouseButton(0))
			return;

		var pos = camera.ScreenToWorldPoint(Input.mousePosition);
		pos.z = 0f;
		var spGO = (GameObject)Instantiate(splatfab);
		var splat = spGO.GetComponent<Splat>();
		var vertices = splat.vertices;
		for (var i = 0; i < vertices.Length; i++) {
			vertices[i] += pos;
		}

		var tmpSplats = new Splat[_splatRenderer.splats.Length + 1];
		System.Array.Copy(_splatRenderer.splats, tmpSplats, _splatRenderer.splats.Length);
		tmpSplats[_splatRenderer.splats.Length] = splat;
		_splatRenderer.splats = tmpSplats;
	}
}
