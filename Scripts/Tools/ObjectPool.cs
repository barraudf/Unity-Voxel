using UnityEngine;
using System.Collections.Generic;

public class ObjectPool : MonoBehaviour
{
	public int PoolSize = 5;
	public bool PrePopulate = false;
	public bool CanGrow = true;
	public GameObject Prefab;

	private List<GameObject> _Instances;

	void Awake()
	{
		_Instances = new List<GameObject>(PoolSize);

		if (PrePopulate)
			PrePolpulate();
	}

	public void PrePolpulate()
	{
		for (int i = 0; i < PoolSize; i++)
		{
			GameObject clone = CreateInstance();
			clone.SetActive(false);
		}
	}

	public GameObject NextObject()
	{
		for (int i = 0; i < _Instances.Count; i++)
		{
			if (!_Instances[i].activeSelf)
			{
				_Instances[i].SetActive(true);
				return _Instances[i];
			}
		}

		if (CanGrow)
			return CreateInstance();
		else
		{
			Debug.LogError("No chunk available in pool");
			return null;
		}
	}

	private GameObject CreateInstance()
	{
		GameObject clone = GameObject.Instantiate(Prefab, Vector3.zero, Quaternion.identity) as GameObject;
		clone.transform.parent = transform;
		_Instances.Add(clone);
		return clone;
	}
}
