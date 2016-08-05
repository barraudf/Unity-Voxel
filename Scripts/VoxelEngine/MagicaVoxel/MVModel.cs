using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshCollider))]
public class MVModel : ChunkContainer
{
	#region Fields
	protected MVChunk _Chunk;
	#endregion Fields

	protected override void Awake()
	{
		base.Awake();
		
		_Loader = new MVLoader();
		_Unloader = new SimpleUnloader();
		_MeshBuilder = new GreedyMeshBuilder();
	}

	protected void FixedUpdate()
	{
		if (_Chunk.DeleteRequested && !_Chunk.Busy)
		{
			Destroy(this);
		}
	}
}
