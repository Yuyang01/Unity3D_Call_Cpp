using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;


public class test : MonoBehaviour {

	[DllImport ("./Assets/Plugins/libdlltest")]
	static extern int Add(int a, int b);

	void Start () {
		
		int status = Add (2, 3);
		Debug.Log (status.ToString());
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
