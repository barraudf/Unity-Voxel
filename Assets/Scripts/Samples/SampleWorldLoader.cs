using UnityEngine;
using System.Collections;
using System.Threading;

[RequireComponent(typeof(World))]
public class SampleWorldLoader : MonoBehaviour
{
	public int MaxLoadPerFrame = 1;

	private World _World;

	private int xx = 0, zz = 0;
	private bool done = false;

	private MVChunk chunk;

	private void Awake()
	{
		_World = GetComponent<World>();
	}

	private void Update()
	{
		if (done)
			return;

		int cpt = 0;
		for (int x = xx; x < _World.MaxChunkX; x++)
		{
			xx = x;
			for (int z = zz; z < _World.MaxChunkZ; z++)
			{
				zz = z;
				if (++cpt > MaxLoadPerFrame)
				{
					return;
				}
				_World.LoadChunkColumn(x, z);
			}
			zz = 0;
		}
		xx = 0;
		done = true;
	}
}
