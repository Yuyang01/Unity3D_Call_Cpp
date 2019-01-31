using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class offsetCalibrationTarget : MonoBehaviour
{
    public Transform _ref;
    public Transform _target;
    public Transform _eyeLeft;
    public Transform _eyeRight;

    bool _useLeft = true;
    float _scaleTarget = 1.1f;
	
	// Update is called once per frame
	void LateUpdate ()
    {
        // Compute the position of the sphere
        Vector3 posTarget = new Vector3();
        if (_useLeft)
        {
            posTarget = _ref.localPosition - _eyeLeft.localPosition;
        }
        else
        {
            posTarget = _ref.localPosition - _eyeRight.localPosition;
        }

        posTarget += _ref.localPosition;

        _target.localPosition = posTarget;

        // Compute the rayon of the sphere
        float diametreRef = _ref.localScale.x * 2.0f * _scaleTarget;
        _target.localScale = new Vector3(diametreRef, diametreRef, diametreRef);
    }
}
