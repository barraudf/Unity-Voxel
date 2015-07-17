using UnityEngine;
using System.Collections;

public class MeshChanger : MonoBehaviour
{

	public GameObject Character;

	// Use this for initialization
	void Start()
	{
		RaceDataFile rdf = new RaceDataFile();
		RaceData humanRace = rdf.Load("Human");

		GameObject go = humanRace.InstantiateCharacter(Vector3.zero);

		if(go != null)
		{
			GameObject target = go.transform.Find("Body/Chest/Head/CameraTarget").gameObject;
			Camera.main.GetComponent<ThirdPersonCamera>().TargetLookAt = target.transform;
		}

		rdf.Unload();
	}
}
