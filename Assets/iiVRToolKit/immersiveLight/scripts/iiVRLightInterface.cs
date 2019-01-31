using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

public class iiVRLightInterface : MonoBehaviour
{
    /*
	 * General methods 
	 */

    [DllImport("iiVRLightForUnity", CallingConvention = CallingConvention.Cdecl)]
    private static extern void iiVRUpdate();

    [DllImport("iiVRLightForUnity", CallingConvention = CallingConvention.Cdecl)]
    private static extern bool initImmersive(string _xmlImmersiveFile, string _xmlUserFilet);

    /*
	 * Renderer methods 
	 */
    [DllImport("iiVRLightForUnity", CallingConvention = CallingConvention.Cdecl)]
    public static extern bool getViewCoordinates(uint _viewId, out int _x, out int _y, out int _width, out int _height);

    [DllImport("iiVRLightForUnity", CallingConvention = CallingConvention.Cdecl)]
    public static extern bool getCamViewport(uint _viewId, uint _screenId, uint _camId, out double _x, out double _y, out double _width, out double _height);

    [DllImport("iiVRLightForUnity", CallingConvention = CallingConvention.Cdecl)]
    public static extern bool getCamProjectionMatrix(uint _viewId, uint _screenId, uint _camId, double _near, double _far,
                                                     out double _mat00, out double _mat01, out double _mat02, out double _mat03,
                                                     out double _mat10, out double _mat11, out double _mat12, out double _mat13,
                                                     out double _mat20, out double _mat21, out double _mat22, out double _mat23,
                                                     out double _mat30, out double _mat31, out double _mat32, out double _mat33);

    [DllImport("iiVRLightForUnity", CallingConvention = CallingConvention.Cdecl)]
    public static extern bool getCamViewMatrix(uint _viewId, uint _screenId, uint _camId,
                                               out double _mat00, out double _mat01, out double _mat02, out double _mat03,
                                               out double _mat10, out double _mat11, out double _mat12, out double _mat13,
                                               out double _mat20, out double _mat21, out double _mat22, out double _mat23,
                                               out double _mat30, out double _mat31, out double _mat32, out double _mat33);

    [DllImport("iiVRLightForUnity", CallingConvention = CallingConvention.Cdecl)]
    public static extern bool getCamPos(uint _viewId, uint _screenId, uint _camId,
                                           out double _x, out double _y, out double _z);

    [DllImport("iiVRLightForUnity", CallingConvention = CallingConvention.Cdecl)]
    public static extern bool getCamViewUp(uint _viewId, uint _screenId, uint _camId,
                                               out double _x, out double _y, out double _z);

    [DllImport("iiVRLightForUnity", CallingConvention = CallingConvention.Cdecl)]
    public static extern bool getCamViewDir(uint _viewId, uint _screenId, uint _camId,
                                           out double _x, out double _y, out double _z);

    [DllImport("iiVRLightForUnity", CallingConvention = CallingConvention.Cdecl)]
    public static extern bool setRoomCoordiantes(double _posX, double _posY, double _posZ,
                                                 double _oriX, double _oriY, double _oriZ, double _oriW);

    [DllImport("iiVRLightForUnity", CallingConvention = CallingConvention.Cdecl)]
    public static extern bool setHeadCoordiantes(double _posX, double _posY, double _posZ,
                                                 double _oriX, double _oriY, double _oriZ, double _oriW);

    [DllImport("iiVRLightForUnity", CallingConvention = CallingConvention.Cdecl)]
    public static extern void getHeadOffset(out double _x, out double _y, out double _z);

    [DllImport("iiVRLightForUnity", CallingConvention = CallingConvention.Cdecl)]
    public static extern void getEyeOffset(uint _viewId, uint _screenId, uint _camId, out double _x, out double _y, out double _z);

    [DllImport("iiVRLightForUnity", CallingConvention = CallingConvention.Cdecl)]
    public static extern bool getCamProjFrustum(uint _viewId, uint _screenId, uint _camId,
                                                out double _near, out double _far, out double _left, out double _right, out double _top, out double _bottom);



    /*
	 * Device methods
	 */
    [DllImport("iiVRLightForUnity", CallingConvention = CallingConvention.Cdecl)]
    public static extern int getDeviceNbButtons(string _nameButton);

    [DllImport("iiVRLightForUnity", CallingConvention = CallingConvention.Cdecl)]
    public static extern bool getDeviceButton(string _nameButton, int _buttonId);

    [DllImport("iiVRLightForUnity", CallingConvention = CallingConvention.Cdecl)]
    public static extern int getDeviceNbAnalogs(string _nameAnalog);

    [DllImport("iiVRLightForUnity", CallingConvention = CallingConvention.Cdecl)]
    public static extern double getDeviceAnalog(string _nameButton, int _analogId);

    [DllImport("iiVRLightForUnity", CallingConvention = CallingConvention.Cdecl)]
    public static extern int getDeviceNbTrackers(string _nameTracker);

    [DllImport("iiVRLightForUnity", CallingConvention = CallingConvention.Cdecl)]
    public static extern void getDeviceTracker(string _nameTracker, int _trackerId,
                                               out double posX, out double posY, out double posZ,
                                               out double oriX, out double oriY, out double oriZ, out double oriW);

    /*

    */
    public string _immersiveFile = "defaultConfig.xml";
    public string _userFile = "defaultUser.xml";

    bool _firstFrame = true;

    public List<deviceLightManager> _devices = new List<deviceLightManager>();
    public roomLightManager _room;
    public List<cameraLightManager> _cams = new List<cameraLightManager>();

    void init()
    {
        initImmersive(_immersiveFile, _userFile);
    }

    // Use this for initialization
    void Start()
    {
        init();
    }

    // Update is called once per frame
    void Update()
    {
        if (_firstFrame)
        {
            _firstFrame = false;
        }
        else
        {
            updateDevices();

            updateRoom();

            iiVRUpdate();

            updateCams();
        }
    }

    void updateDevices()
    {
        for (int i = 0; i < _devices.Count; i++)
        {
            _devices[i].UpdateDevice();
        }
    }

    void updateRoom()
    {
        if (_room != null)
        {
            _room.UpdateRoom();
        }
    }

    void updateCams()
    {
        for (int i = 0; i < _cams.Count; i++)
        {
            _cams[i].UpdateCam();
        }
    }
}