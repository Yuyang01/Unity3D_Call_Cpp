using UnityEngine;
using System.Collections;

public class gamepadManager : MonoBehaviour 
{
	public float _speed = 1.0f ;

	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		float valH = Input.GetAxis ("Horizontal") ;
		float valV = Input.GetAxis ("Vertical") ;

		transform.Translate (transform.right * valH * _speed * Time.deltaTime,Space.World);
		transform.Translate (transform.forward * valV * _speed * Time.deltaTime,Space.World);
	}
}
