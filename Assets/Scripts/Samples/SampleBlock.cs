using UnityEngine;
using System.Collections;

public class SampleBlock : Block
{
	static SampleBlock()
	{
		Colors.Add(typeof(SampleBlock), new Color32() { a = 255, r = 255, g = 0, b = 255 });
	}
}
