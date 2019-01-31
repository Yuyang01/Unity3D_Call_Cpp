using UnityEngine;

/*
 * This is an abstract class
 * you should override the function onSelction
 * this function could be called directly by a gamepad script
 * when there is an evenement associated
 * this script should be put on a game object with a collider and a rigid body
 */

[RequireComponent(typeof(Rigidbody))]
public abstract class selectionManager : MonoBehaviour
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="pad">the vive pad root</param>
    /// <returns>true if by selection we disable the current object, false else</returns>
    public abstract bool onSelection(GameObject pad);

    public virtual void onHilight()
    {
        HighLightManager hMgr = GetComponent<HighLightManager>();
        if(hMgr != null)
            hMgr.setHighLight(true);
    }

    public virtual void offHilight()
    {
        HighLightManager hMgr = GetComponent<HighLightManager>();
        if (hMgr != null)
            hMgr.setHighLight(false);
    }
}
