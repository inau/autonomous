using UnityEngine;
using System.Collections;
using System;

public class CarModel : MonoBehaviour, ICar {
	static private class id_gen {
		static int current = 0;
		static public int nextid(){
			return current++;
		}
	}

	public int ID;
    public float acceleration;
    public float maxspeed;
    public float turnpower;
    public float mass;
    public float friction;
    private Sensors sensors;

	private float angDrag = 10;

    private Rigidbody2D rb;
    private Vector2 curspeed;

	void Awake(){
		ID = id_gen.nextid ();
	}

    // Use this for initialization
    void Start () {
        rb = gameObject.GetComponent<Rigidbody2D>();
        rb.mass = this.mass;
		rb.angularDrag = angDrag;
        sensors = gameObject.GetComponent<Sensors>();
	}

	public int GetID(){
		return ID;
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

	public void slowbrake(){
		if (curspeed.magnitude > 0)
			rb.velocity *= 0.95f;
	}

    public void brake()
    {
        if (curspeed.magnitude > 0)
			rb.velocity *= 0.5f;
//            rb.AddForce( (-transform.up) * acceleration );
    }

    public void turnLeft()
    {
        if (curspeed.magnitude > 0.1)
        {
			rb.angularDrag = 0;
            transform.Rotate(Vector3.forward * turnpower);
            rb.AddForce(new Vector2(transform.right.x, transform.right.y) * -turnpower); //centripetal force
			rb.angularDrag = angDrag;
        }
    }

    public void turnRight()
    {
        if (curspeed.magnitude > 0.1)
        {
			rb.angularDrag = 0;
            transform.Rotate(Vector3.forward * -turnpower);
            rb.AddForce(new Vector2(transform.right.x, transform.right.y) * turnpower); //centripetal force
			rb.angularDrag = angDrag;
        }
    }

    public Sensors GetSensors()
    {
        return sensors;
    }
}
