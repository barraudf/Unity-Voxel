using UnityEngine;
using System.Collections;

public class SampleBlockXAxis : Block
{
	#region Singleton
	private static readonly SampleBlockXAxis _Instance = new SampleBlockXAxis();
	static SampleBlockXAxis() { Colors.Add(typeof(SampleBlockXAxis), new Color32() { a = 255, r = 0, g = 0, b = 255 }); }
	private SampleBlockXAxis() { }
	public static SampleBlockXAxis Instance { get { return _Instance; } }
	#endregion
}
