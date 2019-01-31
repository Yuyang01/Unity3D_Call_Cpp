//#define _STEAMVR_SDK_USED
#undef _STEAMVR_SDK_USED

using UnityEngine;

// Work with the script from vive api
#if _STEAMVR_SDK_USED
public class replaceRoomViveManager : MonoBehaviour
{
    /*
     * Attributes
     */
    Vector3 _roomPos;
    float _roomOriUp;

    public SteamVR_TrackedObject _rightPad = null;
    public GameObject _rootCalibration;

    // for saving in a new register
    public string _applicationName = "";

    /*
     * Monos
     */
	// Use this for initialization
	private void Start ()
    {
        loadConfig();
        transform.localPosition = _roomPos;
        transform.localEulerAngles = new Vector3(0.0f, _roomOriUp, 0.0f);

        _rootCalibration.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            _rootCalibration.SetActive(!_rootCalibration.activeSelf);
        }

        if (_rootCalibration.activeSelf)
        {
            // We are in calibration
            if(SteamVR_Controller.Input((int)_rightPad.index).GetHairTrigger() )
            {
                Vector3 posPad = _rightPad.transform.position;
                Vector3 diffPos = _rootCalibration.transform.position - posPad;
                diffPos.y = 0.0f;

                transform.position += diffPos;

                float rotPad = _rightPad.transform.eulerAngles.y;
                float diffRot = _rootCalibration.transform.eulerAngles.y - rotPad;

                transform.eulerAngles = new Vector3(0.0f, transform.eulerAngles.y + diffRot, 0.0f);
            }
        }
    }

    private void OnApplicationQuit()
    {
        _roomPos = transform.localPosition;
        _roomOriUp = transform.localEulerAngles.y;
        saveConfig();
    }

    /*
     * Internals
     */
    void loadConfig()
    {
        float roomPosX  = PlayerPrefs.GetFloat(_applicationName+"roomPosX");
        float roomPosY = PlayerPrefs.GetFloat(_applicationName+"roomPosY");
        float roomPosZ = PlayerPrefs.GetFloat(_applicationName+"roomPosZ");
        _roomPos = new Vector3(roomPosX,roomPosY,roomPosZ);
        _roomOriUp = PlayerPrefs.GetFloat(_applicationName+"roomOriUp");
    }

    void saveConfig()
    {
        PlayerPrefs.SetFloat(_applicationName+"roomPosX", _roomPos.x);
        PlayerPrefs.SetFloat(_applicationName+"roomPosY", _roomPos.y);
        PlayerPrefs.SetFloat(_applicationName+"roomPosZ", _roomPos.z);
        PlayerPrefs.SetFloat(_applicationName+"roomOriUp", _roomOriUp);
    }
}
    
#endif