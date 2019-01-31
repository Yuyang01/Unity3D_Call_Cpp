
#pragma once
//#define EXPORT_API __declspec(dllexport)

extern "C"{
	double optimizer(double states0[6],double constraints0[3], double searchRange0[2], \
	                  int numOfDiscretization,\
	                  double t[], \
	                  double x[], \
	                  double v[], \
	                  double a[], \
	                  double j[]);
	}


