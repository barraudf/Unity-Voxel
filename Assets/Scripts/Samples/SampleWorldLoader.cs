using UnityEngine;
using System.Collections;
using System.Threading;

[RequireComponent(typeof(World))]
public class SampleWorldLoader : MonoBehaviour
{
	private World _World;

	private void Awake()
	{
		_World = GetComponent<World>();
	}

	private void Start()
	{
		for (int x = 0; x < _World.MaxChunkX; x++)
			for (int z = 0; z < _World.MaxChunkZ; z++)
			{
				_World.LoadChunkColumn(x, z);
			}
	}
}
