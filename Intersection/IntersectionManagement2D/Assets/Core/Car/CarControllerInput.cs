using UnityEngine;
using System.Collections;

public class CarControllerInput : MonoBehaviour {
	
	private ICar car;
	
	void Start () {
		car = gameObject.GetComponent<ICar> ();
		//car.setCar (ReferencePoint.South, ReferencePoint.North);
	}
	
	
	void FixedUpdate()
	{
		car.AdjustSpeed ();
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
			//car.unapply_brake();
		}
		if(Input.GetKey(KeyCode.Space))
		{
			car.brake();
		}
		
		
	}
	
}

