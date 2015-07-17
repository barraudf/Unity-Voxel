using UnityEngine;
using System.Collections;

public class BipedManager : ModelManager
{
	public override void BuildMeshes()
	{
		base.BuildMeshes();

		SetMesh("Body/Chest", RaceData.GetDefaultModelName("Chest"));
		SetMesh("Body/Chest/Head", RaceData.GetDefaultModelName("Head"));
		SetMesh("Body/Chest/RightShoulder", RaceData.GetDefaultModelName("Shoulder"));
		SetMesh("Body/Chest/LeftShoulder", RaceData.GetDefaultModelName("Shoulder"));
		SetMesh("Body/Chest/RightHand", RaceData.GetDefaultModelName("Hand"));
		SetMesh("Body/Chest/LeftHand", RaceData.GetDefaultModelName("Hand"));
		SetMesh("Body/LeftFoot", RaceData.GetDefaultModelName("Foot"));
		SetMesh("Body/RightFoot", RaceData.GetDefaultModelName("Foot"));
	}
}
