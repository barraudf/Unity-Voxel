using UnityEngine;
using System.Collections.Generic;
using System.Linq;


public class TargetEditor : MonoBehaviour
{
	public float TargetMaxDistance = 100f;
	public Color BoxColor = Color.black;
	public float SelectionBoxScale = 1.1f;

	protected Vector3[] _Vertices;
	protected RaycastHit _Hit;
	protected bool _TargetHit = false;
	protected GridPosition _LastHitBlockPosition;
	protected GridPosition _LastChunkHitPosition;
	protected Chunk _LastChunkHit;
	protected Vector3 _HitBoxPosition;
	protected static Material _SelectionBoxMaterial = null;

	protected void FixedUpdate()
	{
		if (Physics.Raycast(transform.position, transform.forward, out _Hit, TargetMaxDistance))
		{
			ChunkMesh chunkMesh = _Hit.collider.GetComponent<ChunkMesh>();
			Chunk chunk = chunkMesh == null ? null : chunkMesh.Chunk;

			if (chunk != null)
			{
				Vector3 hbSize;
				GridPosition hbBlockPosition;
				if (chunk.GetHitBox(_Hit, out hbBlockPosition, out hbSize))
				{
					if (!_TargetHit || !hbBlockPosition.Equals(_LastHitBlockPosition))
					{
						_TargetHit = true;

						float offset = (SelectionBoxScale - 1) * 0.5f;
						_HitBoxPosition = (new Vector3(
							(float)hbBlockPosition.x - offset,
							(float)hbBlockPosition.y - offset,
							(float)hbBlockPosition.z - offset) - chunk.BlockOrigin - chunk.ChunkOrigin) * chunk.BlockScale + chunkMesh.transform.position;
						BuildSelectionBox(hbSize);
						_LastHitBlockPosition = hbBlockPosition;
						_LastChunkHit = chunk;

						WorldChunk c = chunk as WorldChunk;
						if (c != null)
							_LastChunkHitPosition = c.Position;
					}

					return;
				}
			}
		}

		_TargetHit = false;
	}

	protected void Update()
	{
		if (_TargetHit)
		{
			if (Input.GetButtonDown("Fire1"))
				_LastChunkHit.SetBlock(_Hit, null);
			else if (Input.GetButtonDown("Fire2"))
				_LastChunkHit.SetBlock(_Hit, SampleBlock.Instance, true);
		}
	}

	protected void Awake()
	{
		CreateMaterial();
	}

	protected void OnPostRender()
	{
		if (_TargetHit)
		{
			_SelectionBoxMaterial.SetPass(0);

			GL.PushMatrix();
			GL.Begin(GL.LINES);
			GL.Color(BoxColor);

			for (int i = 1; i < _Vertices.Length; i += 2)
			{
				GL.Vertex(_HitBoxPosition + _Vertices[i - 1]);
				GL.Vertex(_HitBoxPosition + _Vertices[i]);
			}

			GL.End();
			GL.PopMatrix();
		}
	}

	protected static void CreateMaterial()
	{
		// from : http://docs.unity3d.com/ScriptReference/GL.html
		if (_SelectionBoxMaterial == null)
		{
			// Unity has a built-in shader that is useful for drawing
			// simple colored things.
			Shader shader = Shader.Find("Hidden/Internal-Colored");
			_SelectionBoxMaterial = new Material(shader);
			_SelectionBoxMaterial.hideFlags = HideFlags.HideAndDontSave;
			// Turn on alpha blending
			_SelectionBoxMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
			_SelectionBoxMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
			// Turn backface culling off
			_SelectionBoxMaterial.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
			// Turn off depth writes
			_SelectionBoxMaterial.SetInt("_ZWrite", 0);
		}
	}

	protected void BuildSelectionBox(Vector3 size)
	{
		List<Vector3> vertices = new List<Vector3>(24);

		vertices.Add(SelectionBoxScale * Vector3.zero);
		vertices.Add(SelectionBoxScale * Vector3.right * size.x);
		vertices.Add(SelectionBoxScale * Vector3.right * size.x);
		vertices.Add(SelectionBoxScale * (Vector3.right * size.x + Vector3.forward * size.z));
		vertices.Add(SelectionBoxScale * (Vector3.right * size.x + Vector3.forward * size.z));
		vertices.Add(SelectionBoxScale * Vector3.forward * size.z);
		vertices.Add(SelectionBoxScale * Vector3.forward * size.z);
		vertices.Add(SelectionBoxScale * Vector3.zero);
		vertices.Add(SelectionBoxScale * Vector3.up * size.y);
		vertices.Add(SelectionBoxScale * (Vector3.right * size.x + Vector3.up * size.y));
		vertices.Add(SelectionBoxScale * (Vector3.right * size.x + Vector3.up * size.y));
		vertices.Add(SelectionBoxScale * (Vector3.forward * size.z + Vector3.up * size.y + Vector3.right * size.x));
		vertices.Add(SelectionBoxScale * (Vector3.forward * size.z + Vector3.up * size.y + Vector3.right * size.x));
		vertices.Add(SelectionBoxScale * (Vector3.forward * size.z + Vector3.up * size.y));
		vertices.Add(SelectionBoxScale * (Vector3.forward * size.z + Vector3.up * size.y));
		vertices.Add(SelectionBoxScale * Vector3.up * size.y);
		vertices.Add(SelectionBoxScale * Vector3.up * size.y);
		vertices.Add(SelectionBoxScale * Vector3.zero);
		vertices.Add(SelectionBoxScale * Vector3.forward * size.z);
		vertices.Add(SelectionBoxScale * (Vector3.forward * size.z + Vector3.up * size.y));
		vertices.Add(SelectionBoxScale * (Vector3.forward * size.z + Vector3.up * size.y + Vector3.right * size.x));
		vertices.Add(SelectionBoxScale * (Vector3.right * size.x + Vector3.forward * size.z));
		vertices.Add(SelectionBoxScale * Vector3.right * size.x);
		vertices.Add(SelectionBoxScale * (Vector3.right * size.x + Vector3.up * size.y));

		_Vertices = vertices.ToArray();
	}
}
