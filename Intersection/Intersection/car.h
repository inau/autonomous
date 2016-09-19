//
//  car.h
//  Intersection
//
//  Created by Martino Secchi on 19/09/16.
//  Copyright © 2016 Martino Secchi. All rights reserved.
//

#ifndef car_h
#define car_h


#endif /* car_h */

#include "intersection.h"
#include "sensors.h"
#include <math.h>

class Vector2{
public:
    float x,y;
    Vector2(float _x, float _y){
        x = _x;
        y = _y;
    }
    Vector2(){
        x = 0;
        y = 0;
    }
    Vector2 operator+ (const Vector2 &v){ return Vector2(x + v.x, y + v.y); }
    Vector2 operator* (const Vector2 &v){ return Vector2(x * v.x, y * v.y); }
    Vector2 operator* (const float &v) const { return Vector2(x *v, y * v);}
};

class Car {
    
    ReferencePoint origin, destination;
    Sensors sensors;
    float velocity, steeringAngle, heading, wheelBase;
    Vector2 position, frontWheel, backWheel;
    
public:
    Car( ReferencePoint _origin, ReferencePoint _destination){
        origin = _origin;
        destination = _destination;
        velocity = 0.0;
        steeringAngle = 0.0;
        wheelBase = 5.0;
//        these will depend on origin and destination
        heading = 0.0;
        position = Vector2(0,0);
        frontWheel = Vector2(0,0);
        backWheel = Vector2(0,0);
//        frontWheel = position + (Vector2( cos(heading) , sin(heading) ) * (float) wheelBase/2);
//        backWheel = position - (Vector2( cos(heading) , sin(heading) ) * (float) wheelBase/2);
    }
    ReferencePoint getOrigin(){ return origin;}
    ReferencePoint getDestination() {return destination;}
    
    void acclerate(){
        
    }
    void decelerate(){
    
    }
    void turn(){
        
    }
    void stop(){
    
    }
    void start(){
    
    }
};


