using UnityEngine;
using System.Collections;

public class CarControllerInput : MonoBehaviour {
	
	private Car car;
	
	void Start () {
		car = gameObject.AddComponent<Car> ();
		car.setCar (ReferencePoint.South, ReferencePoint.North);
	}
	
	
	void FixedUpdate()
	{
		car.adjustSpeed ();
		if (Input.GetKey(KeyCode.UpArrow))
		{
			car.accelerate();
		}
		if (Input.GetKey(KeyCode.DownArrow))
		{
			car.decelerate();
		}
		if (Input.GetKey(KeyCode.LeftArrow))
		{
			car.turnLeft();
		}
		if (Input.GetKey(KeyCode.RightArrow))
		{
			car.turnRight();
		}
		if (Input.GetKeyUp(KeyCode.Space))
		{
			car.unapply_brake();
		}
		if(Input.GetKey(KeyCode.Space))
		{
			car.apply_brake();
		}
		
		
	}
	
}

