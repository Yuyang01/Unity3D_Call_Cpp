using UnityEngine;
using System.Collections;

public class cameraLightManager : MonoBehaviour
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

    // For second eye
    double _frustumNear2nd = 0.01;
    double _frustumFar2nd = 0.01;
    double _frustumRight2nd = 0.01;
    double _frustumLeft2nd = 0.01;
    double _frustumTop2nd = 0.01;
    double _frustumBottom2nd = 0.01;

    bool _camChecked = false;
    bool _enabledCam = true;

    // Update is called once per frame
    public virtual void UpdateCam()
    {
        // cast id to exchange with dll
        uint vId = (uint)_viewId;
        uint sId = (uint)_screenId;
        uint cId = (uint)_camId;

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
        iiVRLightInterface.getCamPos(vId, sId, cId, out xCam, out yCam, out zCam);
        iiVRLightInterface.getCamViewDir(vId, sId, cId, out xDir, out yDir, out zDir);
        iiVRLightInterface.getCamViewUp(vId, sId, cId, out xUp, out yUp, out zUp);

        iiVRLightInterface.getCamProjFrustum(vId, sId, cId, out _frustumNear, out _frustumFar, out _frustumLeft, out _frustumRight, out _frustumTop, out _frustumBottom);
        //iiVRLightInterface.getCamProjFrustum(vId, sId, cId + 1, out _frustumNear2nd, out _frustumFar2nd, out _frustumLeft2nd, out _frustumRight2nd, out _frustumTop2nd, out _frustumBottom2nd);

        iiVRLightInterface.getCamViewport(vId, sId, cId, out _viewportX, out _viewportY, out _viewportL, out _viewportH);

        // Update data
        _dirLookAt = transform.position + new Vector3((float)xDir, (float)yDir, (float)zDir);
        _upLookAt = new Vector3((float)xUp, (float)yUp, (float)zUp);
        _camPos = new Vector3((float)xCam, (float)yCam, (float)zCam);
    }

    void LateUpdate()
    {
        // update view matrice
        transform.LookAt(_dirLookAt, _upLookAt);
        transform.position = _camPos;

        Camera cam = GetComponent<Camera>();
        if (cam)
        {
            // Update viewport
            Rect newViewportRect = new Rect((float)_viewportX, (float)_viewportY, (float)_viewportL, (float)_viewportH);
            cam.rect = newViewportRect;

            // Update proj matrice
            Matrix4x4 m = PerspectiveOffCenter((float)_frustumLeft, (float)_frustumRight, (float)_frustumBottom, (float)_frustumTop, (float)_frustumNear, (float)_frustumFar);
            cam.projectionMatrix = m;

            //cam.projectionMatrix = _projMat;
            cam.SetStereoProjectionMatrix(Camera.StereoscopicEye.Left, m);
            cam.SetStereoProjectionMatrix(Camera.StereoscopicEye.Right, m);
        }
        else
        {
            Debug.LogError("Cannot find camera");
        }
    }

    static Matrix4x4 PerspectiveOffCenter(float left, float right, float bottom, float top, float near, float far)
    {
        // to compute proj matrices from frustrum for unity
        float x = 2.0F * near / (right - left);
        float y = 2.0F * near / (top - bottom);
        float a = (right + left) / (right - left);
        float b = (top + bottom) / (top - bottom);
        float c = -(far + near) / (far - near);
        float d = -(2.0F * far * near) / (far - near);
        float e = -1.0F;
        Matrix4x4 m = new Matrix4x4();
        m[0, 0] = x;
        m[0, 1] = 0;
        m[0, 2] = a;
        m[0, 3] = 0;
        m[1, 0] = 0;
        m[1, 1] = y;
        m[1, 2] = b;
        m[1, 3] = 0;
        m[2, 0] = 0;
        m[2, 1] = 0;
        m[2, 2] = c;
        m[2, 3] = d;
        m[3, 0] = 0;
        m[3, 1] = 0;
        m[3, 2] = e;
        m[3, 3] = 0;
        return m;
    }
}

