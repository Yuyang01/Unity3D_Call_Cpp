using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tactileFingerManager : MonoBehaviour
{
    // 0 left, 1 right
    public int _handId = 0;

    // 0 thumb , 1 index , 2 major
    public int _fingerId = 0;

    // Between 0.0 and 1.0
    public float _intensityVibration = 0.5f;
    
    bool _vibration = false;
    bool _toUpdate = false;

    bool _isOnRoot = true;

    private void Start()
    {
        if (iiVRUnityInterface.getProcessId() == 0)
            _isOnRoot = true;
        else
            _isOnRoot = false;

        if (!_isOnRoot)
        {
            Collider col = GetComponent<Collider>();
            if (col)
                col.enabled = false;
        }
    }

    private void Update()
    {
        if (!_isOnRoot)
            return;

        if (_toUpdate)
        {
            _toUpdate = false;

            if (_vibration)
            {
                iiVRUnityInterface.setDeviceFingerVibrations(_handId, _fingerId, _intensityVibration,1.0);
            }
            else
            {
                iiVRUnityInterface.setDeviceFingerVibrations(_handId, _fingerId, 0.0, 1.0);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        _vibration = true;
        _toUpdate = true;
    }

    private void OnTriggerExit(Collider other)
    {
        _vibration = false;
        _toUpdate = true;
    }
}
