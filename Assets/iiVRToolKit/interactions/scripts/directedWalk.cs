using UnityEngine;
using System.Collections;

public class directedWalk : MonoBehaviour 
{
	// This scipt has to be placed on the room entity

	public GameObject _entityHead = null;		// The input entity for head orientation
	public GameObject _room = null;		// The input entity for head orientation
	public float _directedSpeed = 10.0f;			// The speed of the redircetion 

	public float _directedThreeshold = 10.0f;	// Angle in degrees where no redirection is allowed
	public float _valueAtThreeshold = 1.0f;	// reference for angle when we are at threeshold value (initial speed)

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(_entityHead)
		{
			Quaternion localRot = _entityHead.transform.localRotation;

			Vector3 refForward = new Vector3(0.0f,0.0f,1.0f);
			Vector3 viewDir = localRot * refForward ;

			float eulerY = Mathf.Atan2(viewDir.x,viewDir.z);
			eulerY = Mathf.Rad2Deg * eulerY;

			float rotateValue = 0.0f;

			while(eulerY < -180.0f)
			{
				eulerY += 360.0f;
			}

			while(eulerY > 180.0f)
			{
				eulerY -= 360.0f;
			}

			if(eulerY > 0.0f)
			{
				rotateValue =   ( eulerY * eulerY  )*  ( _valueAtThreeshold / ( _directedThreeshold * _directedThreeshold ) ) * _directedSpeed * Time.deltaTime;
			}
			else if(eulerY < 0.0f)
			{
				rotateValue =   - ( eulerY * eulerY  )*  ( _valueAtThreeshold / ( _directedThreeshold * _directedThreeshold ) ) * _directedSpeed * Time.deltaTime;
			}
			else
			{
				rotateValue = 0.0f;
			}

			_room.transform.localEulerAngles = new Vector3 (_room.transform.localEulerAngles.x,_room.transform.localEulerAngles.y + rotateValue,_room.transform.localEulerAngles.z);
		}
	}
}
