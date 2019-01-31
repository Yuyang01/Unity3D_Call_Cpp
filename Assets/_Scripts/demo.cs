using UnityEngine;
using System;
using System.Collections.Generic;
//using System.Linq;
//using System.Text;
using System.Runtime.InteropServices;




public class demo : MonoBehaviour {


	// example 1
	//static extern int Add(int a, int b);

	// example 2
	[DllImport ("./Assets/Plugins/libdlltest")]
	static extern double optimizer(double[] states0,double[] constraints0, double[] searchRange0, 
		                            int numOfDiscretization,
									double[] t, 
									double[] x, 
									double[] v,
									double[] a,
									double[] j);


	void Start () {

		//example 1
		//int status = Add (3, 3);
		//Debug.Log (status.ToString());


		//example 2
		double[] states = new double[]{0,0,0,2,0,0};
		double[] constraints = new double[]{ 1.5, 1.5, 1 };
		double[] searchRange = new double[]{80,0.1 };


		int numOfDiscretization = 11;
		double[] t=new double[numOfDiscretization];
		double[] x=new double[numOfDiscretization];
		double[] v=new double[numOfDiscretization];
		double[] a=new double[numOfDiscretization];
		double[] j=new double[numOfDiscretization];

		double length = optimizer(states, constraints, searchRange,numOfDiscretization,t,x,v,a,j);

		for (int i = 0; i < numOfDiscretization; i++) {
			Debug.Log (v[i]);
		}
		Debug.Log (length.ToString());

	
	}

	void Update(){
	
	
	}





}
