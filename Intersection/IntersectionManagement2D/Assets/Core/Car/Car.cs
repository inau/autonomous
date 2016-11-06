
using UnityEngine;
using System.Collections;
using System;

public class Car : MonoBehaviour, ICar {
	
	Transform transform;
	Rigidbody2D rb2D;
	public ReferencePoint origin, destination;
	public float power = 5;
    public float _mass = 1;
	public float maxspeed = 5;
	public float turnpower = 2;
	public float friction = 1;
	public Vector2 curspeed;
	public Sensors sensors;

    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        rb2D.mass = _mass;
        rb2D.drag = friction;
        transform = GetComponent<Transform>();
        sensors = gameObject.AddComponent<Sensors>();
    }


    public void setCar(ReferencePoint _origin, ReferencePoint _destination){
		origin = _origin;
		destination = _destination;
	}

	public void AdjustSpeed(){
//		float delta = 0.1f;
		curspeed = new Vector2(rb2D.velocity.x, rb2D.velocity.y);

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
        rb2D.AddForce(transform.up * ((2*power/3)*(curspeed.magnitude+1)));
	}
	public void decelerate(){
        reset_drag();
		rb2D.AddForce((- transform.up)*(power/3 * (curspeed.magnitude+1)));
    }
    public void apply_brake()
    {
        if (rb2D.velocity.magnitude > 0.01)
        {
            rb2D.drag += 0.3f;
        }
//        Debug.Log(rigidbody2D.drag);
    }

    public void unapply_brake()
    {
		rb2D.drag = friction;
    }

	public void turnLeft(){
		if (rb2D.velocity.magnitude > 0.1) {

			transform.Rotate (Vector3.forward * turnpower);
			rb2D.AddForce( new Vector2(transform.right.x, transform.right.y) * -turnpower ); //centripetal force
		}
	}
	public void turnRight(){
		if (rb2D.velocity.magnitude > 0.1) {
			transform.Rotate (Vector3.forward * -turnpower);
			rb2D.AddForce( new Vector2(transform.right.x, transform.right.y) * turnpower ); //centripetal force
		}
	}
	
	public void slowDown(){
		rb2D.drag = friction * 2;
	}
	
    void reset_drag()
    {
        if (rb2D.drag < friction) rb2D.drag = friction;
    }

    public void brake()
    {
        apply_brake();
    }

    public Sensors GetSensors()
    {
        return sensors;
    }
}

