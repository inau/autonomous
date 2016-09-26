using UnityEngine;
using System.Collections;

public class Controller : MonoBehaviour {

	public Car car;

	// Use this for initialization
	void Start () {
		car = new Car (ReferencePoint.South, ReferencePoint.North, this.transform);
	}
	
	// Update is called once per frame
	void Update () {
		//some rudimental friction
		if (car.getVelocity () >= 0.5F)
			car.accelerate (-0.5F);
		else if (car.getVelocity () < 0.5F && car.getVelocity () >= 0)
			car.stop ();
		else
			car.accelerate (0.5F);

		//user input
		if (Input.GetKeyDown (KeyCode.LeftArrow))
			car.turn (20);
		else if (Input.GetKeyDown (KeyCode.RightArrow))
			car.turn (-20);
		else if (Input.GetKeyDown (KeyCode.UpArrow))
			car.accelerate (5);
		else if (Input.GetKeyDown (KeyCode.DownArrow))
			car.accelerate (-5);
		car.updatePosition ();
	}
}
