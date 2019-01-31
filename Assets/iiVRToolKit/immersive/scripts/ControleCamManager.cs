
using UnityEngine;

public class ControleCamManager : cameraManager 
{
    // here we just have to manage the viewport to fill the screen

    // Update is called once per frame
    public override void UpdateCam()
    {
		uint vId = (uint)_viewId;
		uint sId = (uint)_screenId;
		uint cId = (uint)_camId;

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

        iiVRUnityInterface.getCamViewport(vId, sId , cId ,out  _viewportX,out _viewportY,out _viewportL,out _viewportH);
    }

    public override void LateUpdateCam()
    {
		Camera cam = GetComponent<Camera>();
		if (cam) 
		{
			Rect newViewportRect = new Rect ((float)_viewportX, (float)_viewportY, (float)_viewportL, (float) _viewportH);
			cam.rect = newViewportRect;
        }
	}
}
