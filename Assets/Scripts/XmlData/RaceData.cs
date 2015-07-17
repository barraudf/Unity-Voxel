using UnityEngine;
using System.Xml;
using System.Collections.Generic;


public class RaceData : XmlItem
{
	public const string PART_IDENTIFIER_NAME = "part";
	public string PrefabName;
	public float Scale = 1.0f;
	
	
	private Dictionary<string, string> DefaultModels;

	/// <summary>
	/// Load one child <see cref="XmlElement"/>
	/// </summary>
	/// <param name="node">the node</param>
	/// <returns>True if the data could be loaded, false otherwise</returns>
	protected override bool LoadData(XmlNode node)
	{
		switch (node.Name)
		{
			case "PrefabName":
				PrefabName = node.InnerText;
				break;
			case "Scale":
				TryParseFloat(node.InnerText, out Scale);
				break;
			case "DefaultModels":
				DefaultModels = LoadDefaultModels(node.ChildNodes);
				break;
		}

		return true;
	}

	/// <summary>
	/// Load the names of the default models to use for each part of the character prefab
	/// </summary>
	/// <param name="list">List to load</param>
	/// <returns>Dictionary of models</returns>
	protected Dictionary<string, string> LoadDefaultModels(XmlNodeList list)
	{
		Dictionary<string, string> models = new Dictionary<string, string>();
		
		foreach(XmlNode node in list)
		{
			models.Add(node.Attributes[PART_IDENTIFIER_NAME].Value, node.InnerText);
		}

		return models;
	}

	/// <summary>
	/// Get the name of the default model for the requested part if available.
	/// </summary>
	/// <param name="part">Name of the part to look for, as used in GameObject.Find()</param>
	/// <returns>Name of the model if found, or empty string otherwise</returns>
	public string GetDefaultModelName(string part)
	{
		if (DefaultModels.ContainsKey(part))
			return DefaultModels[part];
		else
			return string.Empty;
	}

	public GameObject InstantiateCharacter(Vector3 position)
	{
		return InstantiateCharacter(position, Quaternion.identity);
	}

	public GameObject InstantiateCharacter(Vector3 position, Quaternion rotation)
	{
		GameObject prefabAsset = Resources.Load<GameObject>(PrefabName);

		if(!prefabAsset)
		{
			Debug.LogErrorFormat("Unable to load prefab \"{0}\"",PrefabName);
			return null;
		}

		GameObject gameObject = Transform.Instantiate(prefabAsset, position, rotation) as GameObject;

		if (!gameObject)
		{
			Debug.LogErrorFormat("Unable to instantiate prefab \"{0}\"", PrefabName);
			return null;
		}

		ModelManager modelManager = gameObject.GetComponent<ModelManager>();

		if (modelManager == null)
		{
			Debug.LogErrorFormat("Prefab \"{0}\" has no ModelManager component", PrefabName);
			return null;
		}

		modelManager.RaceData = this;

		return gameObject;
	}
}
