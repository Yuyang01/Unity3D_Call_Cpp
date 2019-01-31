using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fingerTrackingManager : deviceManager
{

    public string _deviceName = "FINGERTRACKING";
    public bool _leftHand = true;

    public int _nbFingers = 3;

    Transform _finger01;
    Transform _finger02;
    Transform _finger03;
    Transform _finger04;
    Transform _finger05;

    private void Start()
    {
        _finger01 = transform.Find("finger01Tip");
        _finger02 = transform.Find("finger02Tip");
        _finger03 = transform.Find("finger03Tip");
        _finger04 = transform.Find("finger04Tip");
        _finger05 = transform.Find("finger05Tip");
    }


    // Update is called once per frame
    public override void UpdateDevice(bool isRoot)
    {
        // Check if we are on root thread
        if (isRoot)
        {
            // Yes : we will ask iiVR datas
            // No : pos will be updated by netSynchManager

            // Manage hand root data
            {
                double posX = 0.0;
                double posY = 0.0;
                double posZ = 0.0;
                double oriX = 0.0;
                double oriY = 0.0;
                double oriZ = 0.0;
                double oriW = 0.0;

                // get hand data
                iiVRUnityInterface.getDeviceFingerTrackingHandDatas(_leftHand, out posX, out posY, out posZ, out oriX, out oriY, out oriZ, out oriW);

                // Set to the transfrom
                Vector3 posDevice = new Vector3((float)posX, (float)posY, (float)posZ);
                Quaternion rotDevice = new Quaternion((float)oriX, (float)oriY, (float)oriZ, (float)oriW);
                transform.localPosition = posDevice;
                transform.localRotation = rotDevice;
            }

            // Manage fingers
            for (int i = 0; i < _nbFingers; i++)
            {
                double posX = 0.0;
                double posY = 0.0;
                double posZ = 0.0;
                double oriX = 0.0;
                double oriY = 0.0;
                double oriZ = 0.0;
                double oriW = 0.0;

                double jAngle1 = 0.0f;
                double jAngle2 = 0.0f;
                double jAngle3 = 0.0f;

                double jDist1 = 0.0f;
                double jDist2 = 0.0f;
                double jDist3 = 0.0f;

                iiVRUnityInterface.getDeviceFingerTrackingFingerDatas(_leftHand, i,
                                                                       out posX, out posY, out posZ,
                                                                       out oriX, out oriY, out oriZ, out oriW,
                                                                       out jAngle1, out jDist1,
                                                                       out jAngle2, out jDist2,
                                                                       out jAngle3, out jDist3);

                Vector3 pos = new Vector3((float)posX, (float)posY, (float)posZ);
                Quaternion rot = new Quaternion((float)oriX, (float)oriY, (float)oriZ, (float)oriW);

                Transform workingFinger = null;
                switch (i)
                {
                    case 0:
                        workingFinger = _finger01;
                        break;
                    case 1:
                        workingFinger = _finger02;
                        break;
                    case 2:
                        workingFinger = _finger03;
                        break;
                    case 3:
                        workingFinger = _finger04;
                        break;
                    case 4:
                        workingFinger = _finger05;

                        break;
                }

                workingFinger.localPosition = pos;
                workingFinger.localRotation = rot;
            }
        }
    }
}
