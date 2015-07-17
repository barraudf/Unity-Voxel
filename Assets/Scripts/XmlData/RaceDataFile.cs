using UnityEngine;
using System.Xml;

public class RaceDataFile : XmlDataFile<RaceData>
{
	protected override string XmlFile
	{
		get { return "xml/Races"; }
	}
}
