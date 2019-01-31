using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class roomLightManager : MonoBehaviour
{
    public void UpdateRoom()
    {
        iiVRLightInterface.setRoomCoordiantes(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z,
            transform.localRotation.x, transform.localRotation.y, transform.localRotation.z, transform.localRotation.w);
    }
}
