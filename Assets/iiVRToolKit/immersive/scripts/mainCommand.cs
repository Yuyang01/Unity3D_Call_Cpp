
using UnityEngine;
using System.Collections;

public class mainCommand : MonoBehaviour 
{
	public GameObject _roomEntity = null;

    Vector3 _startRoomPos;
	Quaternion _startRoomOri;

	public UnityEngine.UI.Text _framerateText = null;

	void Start()
	{
		if(_roomEntity)
		{
			_startRoomPos = _roomEntity.transform.localPosition;
			_startRoomOri = _roomEntity.transform.localRotation;
		}
	}

	// Update is called once per frame
	void Update () 
	{
		if (Input.GetKeyDown (KeyCode.Escape)) 
		{
			Application.Quit ();
		}

		if (_framerateText) 
		{
			float FPS = 1.0f / Time.deltaTime;
			_framerateText.text = "FrameRate : " + FPS.ToString ("F0") + " FPS";
		}
	}

	public void resetPosition()
	{
		if (_roomEntity) 
		{
			_roomEntity.transform.localPosition = _startRoomPos;
			_roomEntity.transform.localRotation = _startRoomOri;
		}
	}

	public void quitButton()
	{
		Application.Quit ();
	}
}
