using UnityEngine;
using System.Collections;

public class CrossHair : MonoBehaviour
{
	public Texture2D Texture;

	private Rect _Position;

	private int _LastScreenWidth, _LastScreenHeight;

	private void Start ()
	{
		UpdatePosition();
	}

	private void FixedUpdate()
	{
		if (Screen.width != _LastScreenWidth || Screen.height != _LastScreenHeight)
			UpdatePosition();
	}
	
	private void OnGUI()
	{
		GUI.DrawTexture(_Position, Texture);
	}

	private void UpdatePosition()
	{
		_Position = new Rect((Screen.width - Texture.width) / 2, (Screen.height - Texture.height) / 2, Texture.width, Texture.height);
		_LastScreenWidth = Screen.width;
		_LastScreenHeight = Screen.height;
	}
}
