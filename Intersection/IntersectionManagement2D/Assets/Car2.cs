
using UnityEngine;
using System.Collections;

public class Car2 : MonoBehaviour {
	
	Transform transform;
	Rigidbody2D rigidbody2D;
	ReferencePoint origin, destination;
	public float power = 5;
    public float _mass = 1;
	public float maxspeed = 5;
	public float turnpower = 2;
	public float friction = 1;
	public Vector2 curspeed;
	
	public Car2(ReferencePoint _origin, ReferencePoint _destination, Transform _transform, Rigidbody2D _rigidbody2d){
		origin = _origin;
		destination = _destination;
		rigidbody2D = _rigidbody2d;
        rigidbody2D.mass = _mass;
        rigidbody2D.drag = friction;
		transform = _transform;

	}
	public void adjustSpeed(){
		curspeed = new Vector2(rigidbody2D.velocity.x, rigidbody2D.velocity.y);

		if (curspeed.magnitude > maxspeed)
		{
			curspeed = curspeed.normalized;
			curspeed *= maxspeed;
		}
		//Debug.Log ("speed magnitude: " + curspeed.magnitude);
	}
	public void accelerate(){
        reset_drag();
        rigidbody2D.AddForce(transform.up * ((2*power/3)*(curspeed.magnitude+1)));
	}
	public void decelerate(){
        reset_drag();
		rigidbody2D.AddForce((- transform.up)*(power/3 * (curspeed.magnitude+1)));
    }
    public void apply_brake()
    {
        var dir = rigidbody2D.velocity.normalized;
        if (rigidbody2D.velocity.magnitude > 0.01)
        {
            rigidbody2D.drag += 0.3f;
        }
        Debug.Log(rigidbody2D.drag);
    }

    public void unapply_brake()
    {
        reset_drag();
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
	
    void reset_drag()
    {
        if (rigidbody2D.drag != friction) rigidbody2D.drag = friction;
    }
}

