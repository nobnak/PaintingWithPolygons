using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Camera))]
public class SplatRenderer : MonoBehaviour {
	public Material splatMat;
	public IList<Splat> splats;

	private Mesh _rectangle;
	private int _width;
	private int _height;
	private WaterMap _waterMap;

	// Use this for initialization
	void Start () {
		_width = Screen.width;
		_height = Screen.height;
		splats = new List<Splat>();
		_rectangle = new Mesh();
		_waterMap = new WaterMap(_width, _height);

		Application.targetFrameRate = 30;
		camera.orthographicSize = 0.5f * _height;
		
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
			splatMat.color = splat.GetColor();

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

			_waterMap.Update();
		}
	}

	public void Add(Splat splat) {
		splats.Add(splat);
	}

	public class WaterMap {
		private byte[] _wetMap;
		private int _width;
		private int _height;

		public WaterMap(int width, int height) {
			this._width = width;
			this._height = height;
			this._wetMap = new byte[_width * _height];
		}

		public void Update() {
			for (var y = 0; y < _height; y++) {
				for (var x = 0; x < _width; x++) {
					var i = x + y * _width;
					var w = _wetMap[i];
					if (w > 0)
						_wetMap[i] = (byte)(w - 1);
				}
			}
		}
		public void Fill(int[] xs, int[] ys, int xOffset, int yOffset) {
			var length = xs.Length;
			for (var i = 0; i < length; i++) {
				var x = xs[i] + xOffset;
				var y = ys[i] + yOffset;
				if (x < 0 || _width <= x || y < 0 || _height <= y)
					continue;
				var pixelIndex = x + y * _width;
				_wetMap[pixelIndex] = (byte)255;
			}
		}
		public byte GetWater(int x, int y) {
			if (x < 0 || _width <= x || y < 0 || _height <= y)
				return 0;
			var pixelIndex = x + y * _width;
			return _wetMap[pixelIndex];
		}
	}
}
