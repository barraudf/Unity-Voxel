using UnityEngine;
using System.Collections;

public class SampleBlockYAxis : Block
{
	#region Singleton
	private static readonly SampleBlockYAxis _Instance = new SampleBlockYAxis();
	static SampleBlockYAxis() { Colors.Add(typeof(SampleBlockYAxis), new Color32() { a = 255, r = 0, g = 255, b = 0 }); }
	private SampleBlockYAxis() { }
	public static SampleBlockYAxis Instance { get { return _Instance; } }
	#endregion
}
