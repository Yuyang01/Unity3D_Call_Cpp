
using UnityEngine;

public class multiDisplayManager : MonoBehaviour
{
	// Use this for initialization
	void Start ()
    {
        for (int i = 1; i < 8; i++)
        {
            if (Display.displays.Length > i)
                Display.displays[i].Activate();
        }
    }
}
