using System;
using System.Globalization;
using System.Xml;

/// <summary>
/// Base class to manage items stored in xml files
/// </summary>
public abstract class XmlItem
{
	/// <summary>
	/// Identifier of the item. There can't be two identical identifier in one xml file
	/// </summary>
	public string ID;

	/// <summary>
	/// Load data from an <see cref="XmlNode"/> into members
	/// </summary>
	/// <param name="node">xml containing item's data</param>
	/// <returns>True if the item could be loaded or false otherwise</returns>
	public bool Load(XmlNode node)
	{
		ID = node.Attributes[XmlDataFile.DATA_NODE_ID_NAME].Value;

		foreach (XmlNode child in node.ChildNodes)
			if (!LoadData(child))
				return false;

		return true;
	}

	/// <summary>
	/// Load one child <see cref="XmlElement"/>
	/// </summary>
	/// <param name="node">the node</param>
	/// <returns>True if the data could be loaded, false otherwise</returns>
	protected abstract bool LoadData(XmlNode node);

	/// <summary>
	/// Helper function to load a float.
	/// Format is "0.0", independently of the current culture 
	/// </summary>
	/// <param name="s">The string containing the float to parse</param>
	/// <param name="result">The float in which to store the result</param>
	/// <returns>True if the parse was successful, or false otherwise</returns>
	protected bool TryParseFloat(string s, out float result)
	{
		return float.TryParse(s, NumberStyles.Float | NumberStyles.AllowThousands, CultureInfo.InvariantCulture, out result);
	}
}
