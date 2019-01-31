
using UnityEngine;

public class roomManager : MonoBehaviour 
{
	public void UpdateRoom () 
	{
		iiVRUnityInterface.setRoomCoordiantes (transform.localPosition.x,transform.localPosition.y,transform.localPosition.z,
			transform.localRotation.x,transform.localRotation.y,transform.localRotation.z,transform.localRotation.w);
	}
}
