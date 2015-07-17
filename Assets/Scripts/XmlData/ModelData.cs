using UnityEngine;
using System.Xml;

public class ModelData : XmlItem
{
	/// <summary>
	/// Path of the vox file. The default behavior of the class is to load an Asset
	/// in which case the path is relative to the Resources folder and don't contains the extension.
	/// The extension has to be .bytes so that Unity knows it is a binary file
	/// </summary>
	public string VoxelFile = string.Empty;

	/// <summary>
	/// Size of voxel for the model (in Unity space unit)
	/// </summary>
	public float SizePerVoxel = 1.0f;

	/// <summary>
	/// Specify the origin's coordinates of the model
	/// </summary>
	public Vector3 MeshOrigin;

	/// <summary>
	/// The name of the material to use, if it's not the default one
	/// </summary>
	public string Material;

	private bool MeshOriginSet = false;

	/// <summary>
	/// Load one child <see cref="XmlElement"/>
	/// </summary>
	/// <param name="node">the node</param>
	/// <returns>True if the data could be loaded, false otherwise</returns>
	protected override bool LoadData(XmlNode node)
	{
		switch (node.Name)
		{
			case "VoxelFile":
				VoxelFile = node.InnerText;
				break;
			case "SizePerVoxel":
				TryParseFloat(node.InnerText, out SizePerVoxel);
				break;
			case "MeshOrigin":
				string[] temp = node.InnerText.Replace(" ", "").Split(',');
				float x, y, z;
				if (!TryParseFloat(temp[0], out x)
					|| !TryParseFloat(temp[1], out y)
					|| !TryParseFloat(temp[2], out z))
					return false;

				MeshOriginSet = true;
				MeshOrigin = new Vector3(x, y, z);
				break;
			case "Material":
				Material = node.InnerText;
				break;
		}

		return true;
	}

	/// <summary>
	/// Generate mesh(es) from the vox model
	/// </summary>
	/// <returns>One or more meshes if the vox file was successfully loaded or an empty array otherwise</returns>
	public Mesh[] LoadMesh()
	{
		Mesh[] meshes = new Mesh[] { };
		byte[] voxData = LoadVox();

		if (voxData.Length > 0)
		{
			MVMainChunk v = MVImporter.LoadVOXFromData(voxData);

			v.palatte[0] = new Color(1f, 1f, 0.4f, 0.5f);
			v.palatte[1] = new Color(0f, 0f, 0.8f, 0.5f);

			if (v != null)
			{
				if (MeshOriginSet)
					meshes = MVImporter.CreateMeshesFromChunk(v.voxelChunk, v.palatte, SizePerVoxel, MeshOrigin);
				else
					meshes = MVImporter.CreateMeshesFromChunk(v.voxelChunk, v.palatte, SizePerVoxel);
			}
		}

		return meshes;
	}

	/// <summary>
	/// Load the data contained in <see cref="VoxelFile"/>
	/// </summary>
	/// <returns>data</returns>
	private byte[] LoadVox()
	{
		byte[] data;

		TextAsset asset = Resources.Load<TextAsset>(VoxelFile);
		if (!asset)
		{
			Debug.LogErrorFormat("Unable to load Model file \"{0}\", with ModelData ID={1}", VoxelFile, ID);
			data = new byte[] { };
		}
		else
		{
			data = asset.bytes;
		}

		return data;
	}
}
