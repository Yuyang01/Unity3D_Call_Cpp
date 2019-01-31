
using UnityEngine;

public class cameraManager : MonoBehaviour
{
    public int _viewId = 0;
    public int _screenId = 0;
    public int _camId = 0;

    protected double _viewportX = 0;
    protected double _viewportY = 0;
    protected double _viewportH = 1;
    protected double _viewportL = 1;

    Vector3 _dirLookAt;
    Vector3 _upLookAt;
    Vector3 _camPos;

    //
    double _frustumNear = 0.01;
    double _frustumFar = 0.01;
    double _frustumRight = 0.01;
    double _frustumLeft = 0.01;
    double _frustumTop = 0.01;
    double _frustumBottom = 0.01;

    protected bool _camChecked = false;
    protected bool _enabledCam = true;

    // Update is called once per frame
    public virtual void UpdateCam()
    {
        // cast id to exchange with dll
        uint vId = (uint)_viewId;
        uint sId = (uint)_screenId;
        uint cId = (uint)_camId;

        // Check if the camera view id is the same as the process, there is one process per view
        // Doing this in update and not on start to be sure init is done done yet in iiVRInterface script
        if (!_camChecked)
        {
            _camChecked = true;
            if (iiVRUnityInterface.getProcessId() == _viewId)
            {
                _enabledCam = true;
            }
            else
            {
                _enabledCam = false;
            }
        }

        if (!_enabledCam)
        {
            GetComponent<Camera>().enabled = false;
            return;
        }

        // Value to get data from dll
        double xDir;
        double yDir;
        double zDir;

        double xUp;
        double yUp;
        double zUp;

        double xCam;
        double yCam;
        double zCam;

        // Call dll 
        iiVRUnityInterface.getCamPos(vId, sId, cId, out xCam, out yCam, out zCam);
        iiVRUnityInterface.getCamViewDir(vId, sId, cId, out xDir, out yDir, out zDir);
        iiVRUnityInterface.getCamViewUp(vId, sId, cId, out xUp, out yUp, out zUp);

        iiVRUnityInterface.getCamProjFrustum(vId, sId, cId,
                                                out _frustumNear,
                                                out _frustumFar,
                                                out _frustumLeft,
                                                out _frustumRight,
                                                out _frustumTop,
                                                out _frustumBottom);
        iiVRUnityInterface.getCamViewport(vId, sId, cId,
                                            out _viewportX, out _viewportY,
                                            out _viewportL, out _viewportH);

        // Update data
        _upLookAt = new Vector3((float)xUp, (float)yUp, (float)zUp);
        _camPos = new Vector3((float)xCam, (float)yCam, (float)zCam);
        _dirLookAt = _camPos + new Vector3((float)xDir, (float)yDir, (float)zDir);

        // Update view matrix
        transform.position = _camPos;
        transform.LookAt(_dirLookAt, _upLookAt);
    }

    // Update is called once per frame
    public virtual void LateUpdateCam()
    {
        Camera cam = GetComponent<Camera>();
        if (cam)
        {
            // Update viewport
            Rect newViewportRect = new Rect((float)_viewportX, (float)_viewportY, (float)_viewportL, (float)_viewportH);
            cam.rect = newViewportRect;

            Matrix4x4 m = matricesTools.PerspectiveOffCenter((float)_frustumLeft, (float)_frustumRight, (float)_frustumBottom, (float)_frustumTop, (float)_frustumNear, (float)_frustumFar);

            cam.projectionMatrix = m;
        }
        else
        {
            Debug.LogError("Cannot find camera");
        }
    }
}
