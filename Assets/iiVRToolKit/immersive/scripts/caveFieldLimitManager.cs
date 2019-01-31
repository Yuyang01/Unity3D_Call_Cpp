
using UnityEngine;

using System.Collections.Generic;

/// <summary>
/// Manage the metaphore showing to user to inform him he is near screens
/// </summary>
public class caveFieldLimitManager : MonoBehaviour
{
    /// <summary>
    /// If user is near screens than this distance, we inform him. 
    /// Meters.
    /// </summary>
    public float _distanceMax = 0.4f;

    /// <summary>
    /// Position of the left screen in the cave origin. 
    /// Cave origin is the tracking origin. 
    /// Meters.
    /// </summary>
    public float _left = -1.7f;
    /// <summary>
    /// Position of the right screen in the cave origin. 
    /// Cave origin is the tracking origin. 
    /// Meters.
    /// </summary>
    public float _right = 1.7f;
    /// <summary>
    /// Position of the face screen in the cave origin. 
    /// Cave origin is the tracking origin. 
    /// Meters.
    /// </summary>
    public float _forward = 1.35f;

    /// <summary>
    /// Link to object tracked
    /// </summary>
    public List<GameObject> _checkObjects = new List<GameObject>();

    /// <summary>
    /// Room of the virtual wall that have the grid
    /// </summary>
    public GameObject _rootLimits;
    /// <summary>
    /// Link to the grif material
    /// </summary>
    public Material _materialLimits;

    /// <summary>
    /// Store if we have to see limits or no
    /// </summary>
    bool _seeLimits = false;
    /// <summary>
    /// Current alpha value for materials used
    /// </summary>
    float _alphaValue = 0.5f;
    /// <summary>
    /// the minimu value alpha could take
    /// </summary>
    public float _botAlphaLimit = 0.5f;
    /// <summary>
    /// The speed alpha value will change during time grids are activated
    /// </summary>
    public float _speedAlpha = 0.1f;

    /// <summary>
    /// The albedo color muliply to the white grid
    /// Alpha is set to the current alpha value
    /// </summary>
    Color _colorGrid = Color.red;
	
	/*
     * Monos
     */
	void Update ()
    {
        _seeLimits = false;

        for (int i = 0; i < _checkObjects.Count; i++)
        {
            if (_checkObjects[i].transform.localPosition.x < (_left + _distanceMax) ||
                _checkObjects[i].transform.localPosition.x > (_right - _distanceMax) ||
                _checkObjects[i].transform.localPosition.z > (_forward - _distanceMax)
            )
            {
                _seeLimits = true;
            }
        }

        // If we have to see limits, show it to user
        if (_seeLimits)
        {
            _rootLimits.SetActive(true);
            _colorGrid.a = _alphaValue;
            _materialLimits.color = _colorGrid;

            // Change the alpha value of material with the speed in param
            _alphaValue += _speedAlpha * Time.deltaTime;

            // check if we have to change the way of the alpha speed
            if (_speedAlpha > 0.0f)
            { 
                if (_alphaValue > 1.0f)
                {
                    _alphaValue = 1.0f;
                    _speedAlpha = -_speedAlpha;
                }
            }
            else
            {
                if (_alphaValue < _botAlphaLimit)
                {
                    _alphaValue = _botAlphaLimit;
                    _speedAlpha = -_speedAlpha;
                }
            }
        }
        else
        {
            _rootLimits.SetActive(false);
            _alphaValue = _botAlphaLimit;
        }
	}
}
