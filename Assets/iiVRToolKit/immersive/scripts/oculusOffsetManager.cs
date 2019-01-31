
//#define _OCULUS_SDK_USED
 #undef _OCULUS_SDK_USED

using UnityEngine;

/*
manager for replace room with sensor physical postion
Must be used with the package OculusUtilities
if the package is not present, disable the value _OCULUS_SDK_USED
*/

public class oculusOffsetManager : MonoBehaviour
{
    public Vector3 _theoricalPosForSensor = new Vector3(0.0f, 0.96f, 0.8f);
    
    public GameObject _sensor = null;
    public GameObject _sensorRootModel = null;
    public GameObject _theoricaleOriginInSensorRef = null;
    public GameObject _iiVRRoom = null;

    public bool _debugDrawing = true;

    int _initFrame = 30; // While those first frame, we will call init function

    // Use this for initialization
    void Start()
    {
        if (!_sensor)
        {
            Debug.LogError("Sensor not specified");
        }

        if (!_sensorRootModel)
        {
            Debug.LogError("Sensor Root Model not specified");
        }

        if (!_theoricaleOriginInSensorRef)
        {
            Debug.LogError("theorical Origin not specified");
        }

        if (!_iiVRRoom)
        {
            Debug.LogError("iiVr room not specified");
        }

        enableRenderers(_debugDrawing);
    }

    // Update is called once per frame
    void Update()
    {
        if (_initFrame > 0)
        {
            _initFrame--;
            reinitRoomPositionRelativeToOptitrackOrigin();
        }
    }

    void reinitRoomPositionRelativeToOptitrackOrigin()
    {
        // In a first time manage the sensor pos and orientation

        Vector3 posSensor;
        Quaternion rotSensor;
#if _OCULUS_SDK_USED
        posSensor = OVRManager.tracker.GetPose().position;
        rotSensor = OVRManager.tracker.GetPose().orientation;
#else
        posSensor = Vector3.zero;
        rotSensor = Quaternion.identity;
#endif
        _sensor.transform.localPosition = posSensor;

        // Convert to euler
        Vector3 eulerSensor = rotSensor.eulerAngles;

        // Dispatch in 2 rotation

        // For rotation in oculus ref
        Vector3 eulerBaseSensor = new Vector3(0.0f, rotSensor.eulerAngles.y - 180.0f, 0.0f);
        _sensor.transform.localEulerAngles = eulerBaseSensor;

        //For drawing the sensor correctly
        _sensorRootModel.transform.localEulerAngles = new Vector3(eulerSensor.x, 180.0f, 0.0f);

        // Move the theoricale origin to his place
        _theoricaleOriginInSensorRef.transform.localPosition = -_theoricalPosForSensor;

        // We know where the sensor must be on the room, but he could be somewere else
        // so we have to move room oculus in order to match sensor position on oculus and optitrack origins

        // rotate the room
        Vector3 rotEulerRoomOculus = new Vector3(0.0f, -eulerBaseSensor.y, 0.0f);

        // translate the room
        Vector3 posRoomOculus = _theoricalPosForSensor - posSensor;

        // Apply computations
        transform.localPosition = posRoomOculus;
        transform.localEulerAngles = rotEulerRoomOculus;

        // Now we compute the diff between iiVR Origin and theorical origin
        Vector3 finalOffset = _iiVRRoom.transform.position - _theoricaleOriginInSensorRef.transform.position;
        transform.position = transform.position + finalOffset;
    }

    void enableRenderers(bool visible)
    {
        MeshRenderer[] rendererList = _iiVRRoom.GetComponentsInChildren<MeshRenderer>();
        for (int i = 0; i < rendererList.Length; i++)
        {
            rendererList[i].enabled = visible;
        }
    }
}
