using UnityEngine;
using System.Collections;

public abstract class ModelManager : MonoBehaviour
{
	public const string ROOT_BODY_GAMEOBJECT = "Body";
	public RaceData RaceData;

	protected Transform BodyRoot;
	private ModelDataFile mdf;

	protected void Start ()
	{
		BodyRoot = transform.Find(ROOT_BODY_GAMEOBJECT);

		if (!BodyRoot)
			Debug.LogErrorFormat("GameObject {0} has no child named {1}", gameObject.name, ROOT_BODY_GAMEOBJECT);
		else
		{
			LoadResources();
			BuildMeshes();
			UnloadResources();
		}
	}
	
	public virtual void BuildMeshes()
	{
		if (RaceData == null)
			Debug.LogErrorFormat("{0}'s RaceData has not been initialized", transform.name);
	}

	public virtual void LoadResources()
	{
		mdf = new ModelDataFile();
	}

	public virtual void UnloadResources()
	{
		mdf.Unload();
	}

	protected void SetMesh(string goName, string modelName)
	{
		Transform child = transform.Find(goName);
		if (child != null)
		{
			MeshFilter meshFilter = child.gameObject.GetComponent<MeshFilter>();

			if(!meshFilter)
			{
				Debug.LogErrorFormat("GameObject {0} has no MeshFilterComponent", child.name);
				return;
			}

			if(string.IsNullOrEmpty(modelName))
			{
				meshFilter.mesh.Clear();
				return;
			}

			ModelData md = mdf.Load(modelName);

			if (md != null)
			{
				Mesh[] meshes = md.LoadMesh();
				foreach (Mesh mesh in meshes)
				{
					meshFilter.mesh = mesh;
					break;
				}
			}
			else
			{
				Debug.LogErrorFormat("Error loading ModelData for \"{0}\'", modelName);
			}
		}
		else
		{
			Debug.LogErrorFormat("GameObject {0} has no child named {1}", gameObject.name, goName);
		}
	}
}
