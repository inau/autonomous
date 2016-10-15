using UnityEngine;
using System.Collections;

public class CarController2d : MonoBehaviour {

	Car2 car;
	Rigidbody2D rigidbody2D;
	bool gas;
	
	// Use this for initialization
	void Start () {
		rigidbody2D = GetComponent<Rigidbody2D>();
		car = new Car2 (ReferencePoint.South, ReferencePoint.North, this.transform, rigidbody2D);
		gas = false;
	}
	
	
	void FixedUpdate()
	{
		//gas = false;
		car.adjustSpeed ();
		if (Input.GetKey(KeyCode.UpArrow))
		{
			car.accelerate();
			gas = true;
		}
		if (Input.GetKey(KeyCode.DownArrow))
		{
			car.decelerate();
			gas = true;
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
            Debug.Log("UP");
            car.unapply_brake();
        }
        if(Input.GetKey(KeyCode.Space))
        {
            Debug.Log("DOWN");
            car.apply_brake();
        }

//        if (!gas) {
//			car.slowDown();
//		}

		
	}

}
