
using UnityEngine;
using System.Collections;

public class Car : MonoBehaviour {
	
	Transform transform;
	Rigidbody2D rigidbody2D;
	ReferencePoint origin, destination;
	public float power = 5;
    public float _mass = 1;
	public float maxspeed = 5;
	public float turnpower = 2;
	public float friction = 1;
	public Vector2 curspeed;
	public Sensors sensors;

	public void setCar(ReferencePoint _origin, ReferencePoint _destination){
		origin = _origin;
		destination = _destination;
		rigidbody2D = GetComponent<Rigidbody2D>();
        rigidbody2D.mass = _mass;
        rigidbody2D.drag = friction;
		transform = GetComponent<Transform>();
		sensors = gameObject.AddComponent<Sensors> ();
	}

	public void adjustSpeed(){
//		float delta = 0.1f;
		curspeed = new Vector2(rigidbody2D.velocity.x, rigidbody2D.velocity.y);

		if (curspeed.magnitude > maxspeed)
		{
			curspeed = curspeed.normalized;
			curspeed *= maxspeed;

		}
//		if (curspeed.magnitude > 0 && Mathf.Abs (transform.up.x - curspeed.x) < delta && Mathf.Abs (transform.up.y - curspeed.y) < delta) {
//			apply_brake();
//			Debug.Log ("curspeed: " + curspeed.normalized + " transform: " + transform.up.normalized );
//			
//		}
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
        if (rigidbody2D.velocity.magnitude > 0.01)
        {
            rigidbody2D.drag += 0.3f;
        }
//        Debug.Log(rigidbody2D.drag);
    }

    public void unapply_brake()
    {
		rigidbody2D.drag = friction;
    }

	public void turnLeft(){
		if (rigidbody2D.velocity.magnitude > 0)
			transform.Rotate(Vector3.forward * turnpower);
	}
	public void turnRight(){
		if (rigidbody2D.velocity.magnitude > 0)
			transform.Rotate(Vector3.forward * -turnpower);
	}
	
	public void slowDown(){
		rigidbody2D.drag = friction * 2;
	}
	
    void reset_drag()
    {
        if (rigidbody2D.drag < friction) rigidbody2D.drag = friction;
    }
}

