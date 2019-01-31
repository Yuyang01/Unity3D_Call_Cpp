using UnityEngine;
using System.Collections;

public class trackerLightManager : deviceLightManager
{
    public string _deviceName = "TRACKER";
    public int _trackerId = -1;

    // Update is called once per frame
    public override void UpdateDevice()
    {
        //if (_nbTrackers > 0 && _trackerId < _nbTrackers) 
        {
            double posX = 0.0;
            double posY = 0.0;
            double posZ = 0.0;
            double oriX = 0.0;
            double oriY = 0.0;
            double oriZ = 0.0;
            double oriW = 0.0;

            iiVRLightInterface.getDeviceTracker(_deviceName, _trackerId, out posX, out posY, out posZ, out oriX, out oriY, out oriZ, out oriW);

            Vector3 posDevice = new Vector3((float)posX, (float)posY, (float)posZ);
            Quaternion rotDevice = new Quaternion((float)oriX, (float)oriY, (float)oriZ, (float)oriW);

            transform.localPosition = posDevice;
            transform.localRotation = rotDevice;
        }
    }
}