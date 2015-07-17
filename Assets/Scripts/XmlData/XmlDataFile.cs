using UnityEngine;
using System.IO;
using System.Xml;
using System.Collections.Generic;

/// <summary>
/// Base class to manage xml files storing data used by the game
/// </summary>
public abstract class XmlDataFile
{
	/// <summary>
	/// Name of the xml element containing the items data
	/// </summary>
	public const string DATA_NODE_NAME = "item";

	/// <summary>
	/// Name of the element's attribut identifying an item
	/// </summary>
	public const string DATA_NODE_ID_NAME = "id";

	/// <summary>
	/// Path of the xml file to load. The default behavior of the class is to load an Asset
	/// in which case the path is relative to the Resources folder and don't contains the extension
	/// </summary>
	protected abstract string XmlFile { get; }

	private TextAsset Asset;

	/// <summary>
	/// Search an item in <see cref="XmlFile"/>
	/// </summary>
	/// <param name="itemID">Identifier of the item</param>
	/// <returns>The <see cref="XmlNode"/> containing the item's data, or null if the identifier was not found</returns>
	protected XmlNode GetItemNode(string itemID)
	{
		XmlDocument doc = new XmlDocument();
		doc.LoadXml(LoadXml());
		XmlNodeList nodes = doc.GetElementsByTagName(DATA_NODE_NAME);

		foreach (XmlNode node in nodes)
		{
			if (node.Attributes[DATA_NODE_ID_NAME].Value == itemID)
			{
				return node;
			}
		}

		return null;
	}

	/// <summary>
	/// Get the identifiers of all items contained in <see cref="XmlFile"/> 
	/// </summary>
	/// <returns>A list of identifiers</returns>
	public string[] GetItemIds()
	{
		List<string> ids = new List<string>();

		XmlDocument doc = new XmlDocument();
		doc.LoadXml(LoadXml());
		XmlNodeList nodes = doc.GetElementsByTagName(DATA_NODE_NAME);

		foreach (XmlNode node in nodes)
			ids.Add(node.Attributes[DATA_NODE_ID_NAME].Value);

		ids.Sort();
		return ids.ToArray();
	}

	/// <summary>
	/// Get the content of <see cref="XmlFile"/>
	/// </summary>
	/// <returns>Xml contained in <see cref="XmlFile"/></returns>
	protected virtual string LoadXml()
	{
		Asset = Resources.Load<TextAsset>(XmlFile);
		return Asset.text;
	}

	/// <summary>
	/// Unload <see cref="XmlFile"/> if it has been loaded as a Unity resource
	/// </summary>
	public void Unload()
	{
		if (Asset)
			Resources.UnloadAsset(Asset);
	}
}

/// <summary>
/// Base class to manage xml files storing data used by the game
/// </summary>
public abstract class XmlDataFile<T> : XmlDataFile where T : XmlItem, new()
{
	/// <summary>
	/// Search an item in <see cref="XmlFile"/> with a given identifier
	/// </summary>
	/// <param name="itemID">Identifier of the searched item</param>
	/// <returns>The item or null if it was not found</returns>
	public T Load(string itemID)
	{
		XmlNode node = GetItemNode(itemID);
		if (node == null)
			return default(T);

		return LoadData(node);
	}

	/// <summary>
	/// Get an <see cref="XmlItem"/> from an <see cref="XmlNode"/>
	/// </summary>
	/// <param name="node"><see cref="XmlNode"/> containing the item's data</param>
	/// <returns>The item or null if something went wrong</returns>
	protected virtual T LoadData(XmlNode node)
	{
		T item = new T();
		if (item.Load(node))
			return item;
		else
			return default(T);
	}
}
