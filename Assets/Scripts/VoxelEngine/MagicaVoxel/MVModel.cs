using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshCollider))]
public class MVModel : ChunkContainer
{
	#region Fields
	protected MVChunk _Chunk;
	#endregion Fields

	protected void Awake()
	{
		_Loader = new MVLoader();
		_Unloader = new SimpleUnloader();
		_MeshBuilder = new GreedyMeshBuilder();
	}

	private void FixedUpdate()
	{
		if (_Chunk.DeleteRequested && !_Chunk.Busy)
		{
			Destroy(this);
		}
	}
}
