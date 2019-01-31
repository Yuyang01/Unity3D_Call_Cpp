
using UnityEngine;

public class flystickManager : trackerManager 
{
	public GameObject _room = null;

	public float _speedRot = 30.0f;
	public float _speedWalk = 1.0f;

    // Use this for initialization
    void Start () 
	{
		_deviceName = "FLYSTICK";
	}
	
	// Update is called once per frame
	public override void UpdateDevice (bool isRoot) 
	{
        base.UpdateDevice (isRoot);

        if (isRoot)
        {
            // We will move the room only on thefirst thread,
            // in other, this will be manage by messages
            double valAnalog01 = iiVRUnityInterface.getDeviceAnalog("FLYSTICK", 0);
            double valAnalog02 = iiVRUnityInterface.getDeviceAnalog("FLYSTICK", 1);

            bool valButton01 = iiVRUnityInterface.getDeviceButton("FLYSTICK", 0);
            bool valButton02 = iiVRUnityInterface.getDeviceButton("FLYSTICK", 1);
            bool valButton03 = iiVRUnityInterface.getDeviceButton("FLYSTICK", 2);
            bool valButton04 = iiVRUnityInterface.getDeviceButton("FLYSTICK", 3);
            bool valButton05 = iiVRUnityInterface.getDeviceButton("FLYSTICK", 4);
            bool valButton06 = iiVRUnityInterface.getDeviceButton("FLYSTICK", 5);

            if (_room)
            {
                Vector3 dirWorld = transform.forward * _speedWalk * (float)(valAnalog02) * Time.deltaTime;
                _room.transform.position = _room.transform.position + dirWorld;

                float turnValue = _speedRot * (float)valAnalog01 * Time.deltaTime;
                _room.transform.RotateAround(transform.position, new Vector3(0.0f, 1.0f, 0.0f), turnValue);
            }
        }
	}
}
