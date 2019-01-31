cmake_minimum_required(VERSION 2.8.9)
project(directory_test)

set(CMAKE_MACOSX_RPATH 1)
if(NOT CMAKE_BUILD_TYPE)
  set(CMAKE_BUILD_TYPE Release)
endif()



set(CMAKE_CXX_FLAGS "-Wall -Wextra")
set(CMAKE_CXX_FLAGS_DEBUG "-g")
set(CMAKE_CXX_FLAGS_RELEASE "-O3 -march=native ")

#Bring the headers, such as Student.h into the project
include_directories(include)

set(CMAKE_CXX_STANDARD 11) # C++11...
set(CMAKE_CXX_STANDARD_REQUIRED ON) #...is required...
set(CMAKE_CXX_EXTENSIONS OFF) #...without compiler extensions like gnu++11


#add_definitions(-DEIGEN_USE_BLAS -DEIGEN_USE_LAPACKE)

find_package(Eigen3 REQUIRED)
include_directories(${EIGEN3_INCLUDE_DIR})



#However, the file(GLOB...) allows for wildcard additions:
file(GLOB SOURCES "src/*.cpp")

#Generate the shared library from the sources
add_library(dlltest SHARED ${SOURCES})
#add_executable(dlltest ${SOURCES})


