
using UnityEngine;
using System.Collections;

public class Car2 : MonoBehaviour {
	
	Transform transform;
	Rigidbody2D rigidbody2D;
	ReferencePoint origin, destination;
	public float power = 5;
	public float maxspeed = 10;
	public float turnpower = 2;
	public float friction = 3;
	public Vector2 curspeed ;
	
	public Car2(ReferencePoint _origin, ReferencePoint _destination, Transform _transform, Rigidbody2D _rigidbody2d){
		origin = _origin;
		destination = _destination;
		rigidbody2D = _rigidbody2d;
		transform = _transform;
	}
	public void adjustSpeed(){
		curspeed = new Vector2(rigidbody2D.velocity.x, rigidbody2D.velocity.y);

		if (curspeed.magnitude > maxspeed)
		{
			curspeed = curspeed.normalized;
			curspeed *= maxspeed;
		}
		Debug.Log ("speed magnitude: " + curspeed.magnitude);
	}
	public void accelerate(){
		rigidbody2D.AddForce(transform.up * (power + curspeed.magnitude));
		rigidbody2D.drag = friction;
	}
	public void decelerate(){
		rigidbody2D.AddForce((- transform.up)  * (power/2 * curspeed.magnitude));
		rigidbody2D.drag = friction;
	}
	public void turnLeft(){
		transform.Rotate(Vector3.forward * turnpower);
	}
	public void turnRight(){
		transform.Rotate(Vector3.forward * -turnpower);
	}
	
	public void slowDown(){
		rigidbody2D.drag = friction * 2;
	}
	
}

