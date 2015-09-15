using UnityEngine;
using System.Collections;

public class SampleBlock2 : Block
{
	#region Singleton
	private static readonly SampleBlock2 _Instance = new SampleBlock2();
	static SampleBlock2() { Colors.Add(typeof(SampleBlock2), new Color32() { a = 255, r = 127, g = 127, b = 127 }); }
	private SampleBlock2() { }
	public static SampleBlock2 Instance { get { return _Instance; } }
	#endregion
}
