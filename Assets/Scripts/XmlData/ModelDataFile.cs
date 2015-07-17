using UnityEngine;
using System.Xml;

public class ModelDataFile : XmlDataFile<ModelData>
{
	private string _XmlFile;

	protected override string XmlFile
	{
		get { return _XmlFile; }
	}

	public ModelDataFile()
		: base()
	{
		_XmlFile = "xml/Models";
	}

	public ModelDataFile(string xmlFile)
	{
		_XmlFile = xmlFile;
	}

	protected override ModelData LoadData(XmlNode node)
	{
		ModelData model = new ModelData();
		if (model.Load(node))
		{
			if (string.IsNullOrEmpty(model.VoxelFile))
				return null;
			else
				return model;
		}
		else
			return null;
	}
}
