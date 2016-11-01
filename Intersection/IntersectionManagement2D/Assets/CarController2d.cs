using UnityEngine;
using System.Collections;

public class CarController2d : MonoBehaviour {

	private Car car;

	void Start () {
		car = gameObject.GetComponent<Car> ();
	}
	
	
	void FixedUpdate()
	{
		car.adjustSpeed ();
		car.accelerate();

	}

}
