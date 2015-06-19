using UnityEngine;
using System.Collections.Generic;

public class ObjectPooling : MonoBehaviour
{
    public int PoolSize = 5;
	public bool PrePopulate = false;
    public bool CanGrow = true;
    public GameObject Prefab;

    private List<GameObject> Instances;

    void Start()
    {
        Instances = new List<GameObject>(PoolSize);

		if (PrePopulate)
			PrePolpulate ();
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
        foreach (GameObject instance in Instances)
        {
            if (!instance.activeSelf)
            {
                instance.SetActive(true);
                return instance;
            }
		}

        if (CanGrow)
            return CreateInstance();
        else
            return null;
	}
 
	private GameObject CreateInstance()
    {
        GameObject clone = GameObject.Instantiate(Prefab, Vector3.zero, Quaternion.identity) as GameObject;
        clone.transform.parent = transform;
        Instances.Add(clone);
		return clone;
	}
}
