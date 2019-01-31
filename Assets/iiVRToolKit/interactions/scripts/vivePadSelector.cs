
//#define _STEAMVR_SDK_USED
#undef _STEAMVR_SDK_USED

using UnityEngine;

// Work with the script from vive api
#if _STEAMVR_SDK_USED
[RequireComponent(typeof(SteamVR_TrackedObject))]
public class vivePadSelector : MonoBehaviour
{
    /*
     * Attributes
     */
    // The id of the tracked objet (Left or right pad)
    SteamVR_TrackedObject.EIndex _currentViveIndex = SteamVR_TrackedObject.EIndex.None;

    // iiVR component
    selectionManager _currentSelectedObject = null;
    
    // to disable when the pad is used for other task, where we don't want to select
    bool _enable = true;

    /*
     * Mono
     */
    // Physx callback
    private void OnTriggerEnter(Collider other)
    {
        if (_enable)
        {
            if (_currentSelectedObject)
            {
                // do nothing we already have an object potentially selectable
            }
            else
            {
                _currentSelectedObject = other.gameObject.GetComponent<selectionManager>();
                if (_currentSelectedObject)
                {
                    _currentSelectedObject.onHilight();
                    SteamVR_Controller.Input((int)_currentViveIndex).TriggerHapticPulse(2000);
                }
            }
        }
    }

    // Physx callback
    private void OnTriggerExit(Collider other)
    {
        if (_enable)
        {
            if (_currentSelectedObject)
            {
                _currentSelectedObject.offHilight();
                _currentSelectedObject = null;
            }
        }
    }
    
    private void Start()
    {
        _currentViveIndex  = GetComponent<SteamVR_TrackedObject>().index;
    }
    
    private void Update()
    {
        if (_enable)
        {
            // If click on trigger, check if we are on a recognize object, and call is selection method
            if (SteamVR_Controller.Input((int)_currentViveIndex).GetPressDown(SteamVR_Controller.ButtonMask.Trigger))
            {
                if (_currentSelectedObject)
                {
                    if (_currentSelectedObject.onSelection(gameObject))
                        _currentSelectedObject = null;
                }
            }
        }
    }

    /*
     * Interfaces
     */
    public void setSelectionEnable(bool enable)
    {
        _enable = enable;
        if (_enable)
        {

        }
        else
        {
            // Check if there is an object manage
            if (_currentSelectedObject)
            {
                _currentSelectedObject.offHilight();
                _currentSelectedObject = null;
            }
        }
    }

    /*
     * Internals
     */
}

#endif