
using UnityEngine;

public class configScenario : MonoBehaviour
{
    public UnityEngine.UI.InputField _headX;
    public UnityEngine.UI.InputField _headY;
    public UnityEngine.UI.InputField _headZ;

    double _offsetX = 0.0;
    double _offsetY = 0.0;
    double _offsetZ = 0.0;

    bool _toUpdate = false;

    // Use this for initialization
    void Start ()
    {
        iiVRUnityInterface.getHeadOffset(out _offsetX, out _offsetY, out _offsetZ);

        _headX.text = _offsetX.ToString("F4");
        _headY.text = _offsetY.ToString("F4");
        _headZ.text = _offsetZ.ToString("F4");
    }

    void Update()
    {
        if (_toUpdate)
        {
            iiVRUnityInterface.setHeadOffset(_offsetX, _offsetY, _offsetZ);
            _toUpdate = false;
        }
    }

    public void onChangeHeadX(string val)
    {
        _offsetX = double.Parse(val);
        _toUpdate = true;
    }
    public void onChangeHeadY(string val)
    {
        _offsetY = double.Parse(val);
        _toUpdate = true;
    }
    public void onChangeHeadZ(string val)
    {
        _offsetZ = double.Parse(val);
        _toUpdate = true;
    }

    public Vector3 getHeadValue()
    {
        return new Vector3((float)_offsetX, (float)_offsetY, (float)_offsetZ);
    }

    public void setHeadValue(Vector3 offset)
    {
        _offsetX = offset.x;
        _offsetY = offset.y;
        _offsetZ = offset.z;
        _toUpdate = true;
    }
}
