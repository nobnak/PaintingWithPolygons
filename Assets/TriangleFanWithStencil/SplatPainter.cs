
using UnityEngine;
using System.Collections;

public class SplatPainter : MonoBehaviour {
	public GameObject[] brushfabs;

	private GameObject _selectedBrushfab;

	private SplatRenderer _splatRenderer;

	// Use this for initialization
	void Start () {
		_selectedBrushfab = brushfabs[0];
		_splatRenderer = GetComponent<SplatRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
		if (!Input.GetMouseButton(0))
			return;

		var pos = GetComponent<Camera>().ScreenToWorldPoint(Input.mousePosition);
		pos.z = 0f;
		var spGO = (GameObject)Instantiate(_selectedBrushfab);
		var brush = spGO.GetComponent<Brush>();
		var splats = spGO.GetComponentsInChildren<Splat>();
		foreach (var splat in splats) {
			var vertices = splat.vertices;
			for (var i = 0; i < vertices.Length; i++) {
				vertices[i] += pos;
			}
		}
		_splatRenderer.Add(brush, (int)pos.x, (int)pos.y);

		foreach (var splat in splats)
			splat.transform.parent = null;
		Destroy(spGO, 0f);
	}

	void OnGUI() {
		GUILayout.BeginVertical();
		foreach (var brushfab in brushfabs) {
			if (GUILayout.Button(brushfab.name))
				_selectedBrushfab = brushfab;
		}
		GUILayout.EndVertical();
	}
}
