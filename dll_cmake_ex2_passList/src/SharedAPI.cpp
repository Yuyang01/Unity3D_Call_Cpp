
//#define EIGEN_USE_MKL_ALL
//#define EIGEN_VECTORIZE_SSE4_2

#define EIGEN_USE_BLAS
//#define EIGEN_USE_LAPACKE

#include <iostream>
#include "SharedAPI.h"

#include <math.h>
#include <Eigen/Core>
#include <Eigen/QR>

using namespace std;
using namespace Eigen;

// predifined data typeb
typedef Matrix<double, 6, 1> Vector6d;


// function to be used
Vector6d minimumJerk(Vector6d states, double T );
Vector4d getOneStates(Vector6d param, double currentTime);
MatrixXd getAllStates(Vector6d param, double T, int numOfDiscretization);
double findTimeBiSection(Vector2d SearchRange, VectorXd constraints, Vector6d states);
bool checkConstraints(Vector3d constraints, MatrixXd allStates);


double optimizer(double states0[6],double constraints0[3], double searchRange0[2], \
	                  int numOfDiscretization,\
	                  double t[], \
	                  double x[], \
	                  double v[],\
	                  double a[],\
	                  double j[])
{
   
    Vector6d states = Map< Vector6d > (states0);
    Vector3d constraints = Map< Vector3d > (constraints0);
    Vector2d searchRange = Map< Vector2d > (searchRange0);
    // find the optimal time with all the conditions meet
    double optT= findTimeBiSection(searchRange,constraints, states);
     
     // the parameters for analytical solutions
    auto param=minimumJerk(states, optT);
    
    // get all states through the time
    auto allStates=getAllStates(param, optT, numOfDiscretization);
       
     for(int i=0;i<numOfDiscretization;i++){
      
      t[i]=allStates(0,i);
      x[i]=allStates(1,i);
      v[i]=allStates(2,i);
      a[i]=allStates(3,i);
      j[i]=allStates(4,i);
      
      }
      
      
    return optT;
}




double findTimeBiSection(Vector2d SearchRange, VectorXd constraints, Vector6d states){
    
    double middle;
    for(int i=0; i<16;i++){
      // get all states at time a
      auto param1=minimumJerk(states, SearchRange(0));
      auto allStates1=getAllStates(param1, SearchRange(0), (SearchRange(0)<5)?80:180);
      bool meet1=checkConstraints(constraints,allStates1);
    
      // get all states at time b
      auto param2=minimumJerk(states, SearchRange(1));
      auto allStates2=getAllStates(param2, SearchRange(1), (SearchRange(1)<5)?80:300);
      bool meet2=checkConstraints(constraints,allStates2);
    
      assert(meet1!=meet2);
      // get all states at middle time (a+b)/2
      middle=0.5*(SearchRange(0)+SearchRange(1));
      auto paramMiddle=minimumJerk(states, middle);
      auto allStatesMiddle=getAllStates(paramMiddle, middle, (middle<5)?80:240);
      bool meetMiddle=checkConstraints(constraints,allStatesMiddle);
    
      double keepedSearch=( meetMiddle==meet2 )? SearchRange(0):SearchRange(1);
      SearchRange(0)=keepedSearch;
      SearchRange(1)=middle;
      if(keepedSearch<=middle){
          SearchRange(0)=keepedSearch;
          SearchRange(1)=middle;
         }
      else{
           SearchRange(0)=middle;
           SearchRange(1)=keepedSearch;		  
		  }
     //cout<<SearchRange<<endl;
        
    }
    
    return middle;
}


bool checkConstraints(Vector3d constraints, MatrixXd allStates){
    
    auto maxStates=allStates.rowwise().maxCoeff().bottomRows(3).cwiseAbs();
    bool speedMeet=(maxStates(0)<=constraints(0));
    bool accelerationMeet=(maxStates(1)<=constraints(1));
    bool jerkMeet=(maxStates(2)<=constraints(2));
    
    return ( (speedMeet &&  accelerationMeet) && jerkMeet)? true:false ;
}



MatrixXd getAllStates(Vector6d param, double T, int numOfDiscretization){
	
	
	auto discretizedTime=VectorXd::LinSpaced(numOfDiscretization,0,T);
	//cout<<discretizedTime.size()<<endl;
	
    MatrixXd  motionProfile(5,numOfDiscretization);
    for (int i=0;i<numOfDiscretization;i++){
        motionProfile(0,i)=discretizedTime(i);
        motionProfile.col(i).bottomRows(4)=getOneStates(param, discretizedTime(i));
    }
    
	return motionProfile;

}

Vector4d getOneStates(Vector6d param, double currentTime){
	
	Vector4d states;
	states(0)= param(0) + \
	        param(1)*pow(currentTime,1)+ \
	        param(2)*pow(currentTime,2)+ \
	        param(3)*pow(currentTime,3)+\
	        param(4)*pow(currentTime,4)+ \
	        param(5)*pow(currentTime,5) ; 
	states(1)= 0* param(0) + \
	        1*param(1) + \
	        2*param(2)*pow(currentTime,1)+ \
	        3*param(3)*pow(currentTime,2)+\
	        4*param(4)*pow(currentTime,3)+ \
	        5*param(5)*pow(currentTime,4) ;
	states(2)= 0*param(1) + \
	        2*1*param(2)*pow(currentTime,0)+ \
	        3*2*param(3)*pow(currentTime,1)+\
	        4*3*param(4)*pow(currentTime,2)+ \
	        5*4*param(5)*pow(currentTime,3) ;
    states(3)= 0*param(1) + \
              2*1*0*param(2)*pow(currentTime,0)+ \
              3*2*1*param(3)*pow(currentTime,0)+\
            4*3*2*param(4)*pow(currentTime,1)+ \
            5*4*3*param(5)*pow(currentTime,2) ;
    
    return states;
	}



Vector6d minimumJerk(Vector6d states, double T ){
	
	double T2=T*T;
	double T3=T2*T;
	double T4=T3*T;
	double T5=T4*T;
	
	Vector6d param;
    Matrix3d A;
	Vector3d b;
	
	param(0)=states(0);
	param(1)=states(1);
	param(2)=0.5*states(2);
	
	A<<  T3,   T4,    T5,
	     3*T2, 4*T3,  5*T4,
	     6*T,  12*T2, 20*T3;
	b<< states(3)- param(0)- param(1)*T  -param(2)*T2,
	     states(4)- param(1)- 2*param(2)*T,
	     states(5)- 2*param(2);
	           
	//cout<<A<<endl;
	//cout<<b<<endl;    
		
	param.bottomRows(3)= A.colPivHouseholderQr().solve(b);
	
	return param;
}





