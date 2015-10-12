using UnityEngine;
using System.Collections;

public class SampleBlockZAxis : Block
{
	#region Singleton
	private static readonly SampleBlockZAxis _Instance = new SampleBlockZAxis();
	static SampleBlockZAxis() { Colors.Add(typeof(SampleBlockZAxis), new Color32() { a = 255, r = 255, g = 0, b = 0 }); }
	private SampleBlockZAxis() { }
	public static SampleBlockZAxis Instance { get { return _Instance; } }
	#endregion
}
