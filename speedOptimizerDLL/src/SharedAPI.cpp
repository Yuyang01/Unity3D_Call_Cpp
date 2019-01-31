
//#define EIGEN_USE_MKL_ALL
//#define EIGEN_VECTORIZE_SSE4_2

//#define EIGEN_USE_BLAS
//#define EIGEN_USE_LAPACKE

#include "SharedAPI.hpp"
#include <iostream>
#include <math.h>
#include <Eigen/Core>
#include <Eigen/QR>



using namespace std;
using namespace Eigen;

// predifined data typeb
typedef Matrix<double, 6, 1> Vector6d;


Vector6d minimumJerk(Vector6d states, double T );
Vector4d getOneStates(Vector6d param, double currentTime);
MatrixXd getAllStates(Vector6d param, double T, int numOfDiscretization);
double findTimeBiSection(Vector2d SearchRange, VectorXd constraints, Vector6d states);
bool checkConstraints(Vector3d constraints, MatrixXd allStates);

int main()
{
    auto start =clock();
    double duration;
	// Initial value for boundary condition
	double x0=0, dx0=0, ddx0=0;
	double xT=1,dxT=0, ddxT=0;
	int numOfDiscretization=11;
	
	// This is the boundary condition
	Vector6d states;
	states(0)=x0;
	states(1)=dx0;
	states(2)=ddx0;
	states(3)=xT;
	states(4)=dxT;
	states(5)=ddxT;
    
    // This is the constraints
    Vector3d constraints;
    //speed constraints
    constraints(0)=1.5;
    
    //acceleration constraints
    constraints(1)=1.5;
    
    //jerk constraints
    constraints(2)=1;
    
    // find optimized time with bisection method: the optimization means that
    // the solution should follow the constraints
    
    Vector2d SearchRange;
    SearchRange(0)=60;SearchRange(1)=0.1;
    
    
    double optT= findTimeBiSection(SearchRange,constraints, states);
    //cout<<optT<<endl;
    // the parameters for analytical solutions
    auto param=minimumJerk(states, optT);
    
	// get all states through the time
    auto allStates=getAllStates(param, optT, numOfDiscretization);
	cout<<allStates.row(0)<<endl;
  

    
    duration = ( clock() - start ) / (double) CLOCKS_PER_SEC;
    cout<<"printf: "<< duration <<'\n';
	return 0;
		
}

double findTimeBiSection(Vector2d SearchRange, VectorXd constraints, Vector6d states){
    
    double middle;
    for(int i=0; i<15;i++){
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

