using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class displayManager : MonoBehaviour
{
    public int _nbDisplay = 1;

	// Use this for initialization
	void Start ()
    {
        for (int i = 1; i < _nbDisplay; i++)
        {
            if (Display.displays.Length > i)
                    Display.displays[i].Activate();
        }
	}
}
