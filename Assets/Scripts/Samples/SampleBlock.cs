using UnityEngine;
using System.Collections;

public sealed class SampleBlock : Block
{
	#region Singleton
	private static readonly SampleBlock _Instance = new SampleBlock();
	static SampleBlock() { Colors.Add(typeof(SampleBlock), new Color32() { a = 255, r = 255, g = 0, b = 255 }); }
	private SampleBlock() { }
	public static SampleBlock Instance { get { return _Instance; } }
	#endregion
}
