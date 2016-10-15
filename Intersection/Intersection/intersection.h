//
//  intersection.h
//  Intersection
//
//  Created by Martino Secchi on 19/09/16.
//  Copyright © 2016 Martino Secchi. All rights reserved.
//

#ifndef intersection_h
#define intersection_h


#endif /* intersection_h */

#include <iostream>
#include <string>

enum ReferencePoint{ North, South, East, West};

std::ostream& operator<<(std::ostream& out, const ReferencePoint value){
    const char* s = 0;
#define PROCESS_VAL(p) case(p): s = #p; break;
    switch(value){
            PROCESS_VAL(North);
            PROCESS_VAL(South);
            PROCESS_VAL(East);
            PROCESS_VAL(West);
    }
#undef PROCESS_VAL
    
    return out << s;
}
