
using UnityEngine;
using System.Collections.Generic;
using System.Runtime.InteropServices;

/// <summary>
/// Main entry point for dialog with iiVR methods
/// Declare interfaces with Dll
/// Should be put on the room gameobject
/// </summary>
public class iiVRUnityInterface : MonoBehaviour
{
    /*
	 * General methods 
	 */

    [DllImport("iiVRForUnity", CallingConvention = CallingConvention.Cdecl)]
    private static extern void iiVRUpdate();

    [DllImport("iiVRForUnity", CallingConvention = CallingConvention.Cdecl)]
    private static extern bool initImmersive(string _xmlImmersiveFile, string _xmlUserFile, float rightEyeDist, float leftEyeDist);

    /*
	 * config methods
	 */

    /*
	 * communication methods
	 */
    [DllImport("iiVRForUnity", CallingConvention = CallingConvention.Cdecl)]
    public static extern int getProcessCount();

    [DllImport("iiVRForUnity", CallingConvention = CallingConvention.Cdecl)]
    public static extern int getProcessId();

    /*
	 * Renderer methods 
	 */
    [DllImport("iiVRForUnity", CallingConvention = CallingConvention.Cdecl)]
    public static extern bool getViewCoordinates(uint _viewId, out int _x, out int _y, out int _width, out int _height);

    [DllImport("iiVRForUnity", CallingConvention = CallingConvention.Cdecl)]
    public static extern bool getCamViewport(uint _viewId, uint _screenId, uint _camId, out double _x, out double _y, out double _width, out double _height);

    [DllImport("iiVRForUnity", CallingConvention = CallingConvention.Cdecl)]
    public static extern bool getCamProjectionMatrix(uint _viewId, uint _screenId, uint _camId, double _near, double _far,
                                                     out double _mat00, out double _mat01, out double _mat02, out double _mat03,
                                                     out double _mat10, out double _mat11, out double _mat12, out double _mat13,
                                                     out double _mat20, out double _mat21, out double _mat22, out double _mat23,
                                                     out double _mat30, out double _mat31, out double _mat32, out double _mat33);

    [DllImport("iiVRForUnity", CallingConvention = CallingConvention.Cdecl)]
    public static extern bool getCamViewMatrix(uint _viewId, uint _screenId, uint _camId,
                                               out double _mat00, out double _mat01, out double _mat02, out double _mat03,
                                               out double _mat10, out double _mat11, out double _mat12, out double _mat13,
                                               out double _mat20, out double _mat21, out double _mat22, out double _mat23,
                                               out double _mat30, out double _mat31, out double _mat32, out double _mat33);

    [DllImport("iiVRForUnity", CallingConvention = CallingConvention.Cdecl)]
    public static extern bool getCamPos(uint _viewId, uint _screenId, uint _camId,
                                           out double _x, out double _y, out double _z);

    [DllImport("iiVRForUnity", CallingConvention = CallingConvention.Cdecl)]
    public static extern bool getCamViewUp(uint _viewId, uint _screenId, uint _camId,
                                               out double _x, out double _y, out double _z);

    [DllImport("iiVRForUnity", CallingConvention = CallingConvention.Cdecl)]
    public static extern bool getCamViewDir(uint _viewId, uint _screenId, uint _camId,
                                           out double _x, out double _y, out double _z);

    [DllImport("iiVRForUnity", CallingConvention = CallingConvention.Cdecl)]
    public static extern bool setRoomCoordiantes(double _posX, double _posY, double _posZ,
                                                 double _oriX, double _oriY, double _oriZ, double _oriW);

    [DllImport("iiVRForUnity", CallingConvention = CallingConvention.Cdecl)]
    public static extern void getRoomCoordiantes(out double _posX, out double _posY, out double _posZ,
                                                 out double _oriX, out double _oriY, out double _oriZ, out double _oriW);

    [DllImport("iiVRForUnity", CallingConvention = CallingConvention.Cdecl)]
    public static extern bool setHeadCoordiantes(double _posX, double _posY, double _posZ,
                                                 double _oriX, double _oriY, double _oriZ, double _oriW);

    [DllImport("iiVRForUnity", CallingConvention = CallingConvention.Cdecl)]
    public static extern void getHeadOffset(out double _x, out double _y, out double _z);

    [DllImport("iiVRForUnity", CallingConvention = CallingConvention.Cdecl)]
    public static extern void setHeadOffset(double _x, double _y, double _z);

    [DllImport("iiVRForUnity", CallingConvention = CallingConvention.Cdecl)]
    public static extern void getEyeOffset(uint _viewId, uint _screenId, uint _camId, out double _x, out double _y, out double _z);

    [DllImport("iiVRForUnity", CallingConvention = CallingConvention.Cdecl)]
    public static extern bool getCamProjFrustum(uint _viewId, uint _screenId, uint _camId,
                                                out double _near, out double _far, out double _left, out double _right, out double _top, out double _bottom);



    /*
	 * Device methods
	 */
    [DllImport("iiVRForUnity", CallingConvention = CallingConvention.Cdecl)]
    public static extern int getDeviceNbButtons(string _nameButton);

    [DllImport("iiVRForUnity", CallingConvention = CallingConvention.Cdecl)]
    public static extern bool getDeviceButton(string _nameButton, int _buttonId);

    [DllImport("iiVRForUnity", CallingConvention = CallingConvention.Cdecl)]
    public static extern int getDeviceNbAnalogs(string _nameAnalog);

    [DllImport("iiVRForUnity", CallingConvention = CallingConvention.Cdecl)]
    public static extern double getDeviceAnalog(string _nameButton, int _analogId);

    [DllImport("iiVRForUnity", CallingConvention = CallingConvention.Cdecl)]
    public static extern int getDeviceNbTrackers(string _nameTracker);

    [DllImport("iiVRForUnity", CallingConvention = CallingConvention.Cdecl)]
    public static extern void getDeviceTracker(string _nameTracker, int _trackerId,
                                               out double posX, out double posY, out double posZ,
                                               out double oriX, out double oriY, out double oriZ, out double oriW);

    [DllImport("iiVRForUnity", CallingConvention = CallingConvention.Cdecl)]
    public static extern bool getDeviceFingerTrackingHandDatas(bool _leftHand,
                                                               out double posX, out double posY, out double posZ,
                                                               out double oriX, out double oriY, out double oriZ, out double oriW);

    [DllImport("iiVRForUnity", CallingConvention = CallingConvention.Cdecl)]
    public static extern bool getDeviceFingerTrackingFingerDatas(bool _leftHand, int fingerId,
                                                                   out double posX, out double posY, out double posZ,
                                                                   out double oriX, out double oriY, out double oriZ, out double oriW,
                                                                   out double joint1Angle, out double joint1Dist,
                                                                   out double joint2Angle, out double joint2Dist,
                                                                   out double joint3Angle, out double joint3Dist);

    [DllImport("iiVRForUnity", CallingConvention = CallingConvention.Cdecl)]
    public static extern void setDeviceFingerVibrations(int _handId, int fingerId, double depth , double strength);

    /*

    */
    /// <summary>
    /// path to the room config file
    /// </summary>
    public string _immersiveFile = "defaultConfig.xml";
    /// <summary>
    ///  path to the user config file
    /// </summary>
	public string _userFile = "defaultUser.xml";
    /// <summary>
    /// For debug while user config file management is not implemented yet
    /// </summary>
    public Vector2 _eyeOffset = new Vector2(0.032f, 0.032f);

    /// <summary>
    /// True if we are on the main thread
    /// </summary>
    public bool _isRoot;

    /// <summary>
    /// the id of the thread, 0 if root
    /// </summary>
	int _processId;

    /// <summary>
    /// to avoid computation on first update function
    /// </summary>
	bool firstFrame = true;

    /// <summary>
    /// All the devices which could be updated by iiVR
    /// Fill at runtime with with all corresponding children
    /// </summary>
	List<deviceManager> _devices = new List<deviceManager>();
    /// <summary>
    /// All cameras managed by iiVR
    /// Fill at runtime with with all corresponding children
    /// </summary>
	List<cameraManager> _cams = new List<cameraManager>();

    /// <summary>
    /// Init values
    /// Find cameras and devices and fill data structure with
    /// Then call iiVR init functions
    /// </summary>
    void init()
    {
        cameraManager[] cams = transform.GetComponentsInChildren<cameraManager>();
        for (int i = 0; i < cams.Length; i++)
        {
            _cams.Add(cams[i]);
        }

        deviceManager[] devices = transform.GetComponentsInChildren<deviceManager>();
        for (int i = 0; i < devices.Length; i++)
        {
            _devices.Add(devices[i]);
        }

        initImmersive(_immersiveFile, _userFile, _eyeOffset.x, _eyeOffset.y);

        _processId = getProcessId();

        if (_processId == 0)
        {
            _isRoot = true;
            Debug.Log("On Master");
        }
        else
        {
            _isRoot = false;
            Debug.Log("On slave " + _processId);
        }

        netSynchManager netManager = GetComponent<netSynchManager>();
        if (netManager)
        {
            netManager.init(_isRoot, _processId);
        }
    }

    /// <summary>
    /// Mono : Call when the gameobject is created
    /// 
    /// </summary>
    void Awake()
    {
        init();
    }

    /// <summary>
    /// Mono : Call each frame
    /// </summary>
    void Update()
    {
        if (firstFrame)
        {
            firstFrame = false;
        }
        else
        {
            if (_isRoot)
            {
                //Debug.Log("On Master " + _processId);
                // update datas we want to synch with other thread
                setRoomCoordiantes(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z,
                                   transform.localRotation.x, transform.localRotation.y, transform.localRotation.z, transform.localRotation.w);
            }

            // 
            iiVRUpdate();

            updateDevices();

            if (!_isRoot)
            {
                //Debug.Log("On Slave " + _processId);
                // Get data we want to be synch by root
                double posX = 0.0;
                double posY = 0.0;
                double posZ = 0.0;
                double rotX = 0.0;
                double rotY = 0.0;
                double rotZ = 0.0;
                double rotW = 0.0;
                getRoomCoordiantes(out posX, out posY, out posZ, out rotX, out rotY, out rotZ, out rotW);

                transform.localPosition = new Vector3((float)posX, (float)posY, (float)posZ);
                transform.localRotation = new Quaternion((float)rotX, (float)rotY, (float)rotZ, (float)rotW);
            }

            updateCameras();
        }
    }

    /// <summary>
    /// Mono : called each time a frame just before rendering cameras
    /// </summary>
    void LateUpdate()
    {
        lateUpdateCameras();
    }

    /// <summary>
    /// call special update of devices, in ordrer to don't manage scripts order
    /// </summary>
	void updateDevices()
    {
        for (int i = 0; i < _devices.Count; i++)
        {
            _devices[i].UpdateDevice(_isRoot);
        }
    }

    void updateCameras()
    {
        for (int i = 0; i < _cams.Count; i++)
        {
            _cams[i].UpdateCam();
        }
    }

    void lateUpdateCameras()
    {
        for (int i = 0; i < _cams.Count; i++)
        {
            _cams[i].LateUpdateCam();
        }
    }
}