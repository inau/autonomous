//
//  main.cpp
//  Intersection
//
//  Created by Martino Secchi on 19/09/16.
//  Copyright © 2016 Martino Secchi. All rights reserved.
//

#include <iostream>

#include "car.h"

using namespace std;

int main(int argc, const char * argv[]) {
    // insert code here...
    cout << "Hello, World!\n";
    
    Car car = Car(North, South);
    cout << car.getDestination() << '\n';
    
    return 0;
}
