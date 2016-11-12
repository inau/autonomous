using UnityEngine;
using System.Collections;

public interface ICar {
    void AdjustSpeed();
    void accelerate();
    void decelerate();
    void brake();
    void turnLeft();
    void turnRight();
	void slowbrake();
	int GetID();

    Sensors GetSensors();
}
