using UnityEngine;
using System.Collections;
using System;

public class CarModel : MonoBehaviour, ICar {
    public float acceleration;
    public float maxspeed;
    public float turnpower;
    public float mass;
    public float friction;
    private Sensors sensors;

    private Rigidbody2D rb;
    private Vector2 curspeed;

    // Use this for initialization
    void Start () {
        rb = gameObject.GetComponent<Rigidbody2D>();
        rb.mass = this.mass;
        sensors = gameObject.GetComponent<Sensors>();
	}
	
	// Should be called every frame to max out speed
	public void AdjustSpeed () {

        curspeed = new Vector2(rb.velocity.x, rb.velocity.y);
        if (curspeed.magnitude > maxspeed)
        {
            curspeed = curspeed.normalized * maxspeed;
        }
    }

    public void accelerate()
    {
        rb.AddForce(transform.up * acceleration);
    }

    public void decelerate()
    {
        rb.AddForce( (-transform.up) * acceleration );
    }

    public void brake()
    {
		Debug.Log ("brake");

        if (curspeed.magnitude > 0)
			rb.velocity *= 0.5f;//0.95f;
//            rb.AddForce( (-transform.up) * acceleration );
    }

    public void turnLeft()
    {
        if (curspeed.magnitude > 0.1)
        {
            transform.Rotate(Vector3.forward * turnpower);
            rb.AddForce(new Vector2(transform.right.x, transform.right.y) * -turnpower); //centripetal force
        }
    }

    public void turnRight()
    {
        if (curspeed.magnitude > 0.1)
        {

            transform.Rotate(Vector3.forward * -turnpower);
            rb.AddForce(new Vector2(transform.right.x, transform.right.y) * turnpower); //centripetal force
        }
    }

    public Sensors GetSensors()
    {
        return sensors;
    }
}
