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
	//defaults in prefab
	public int ID;
    public float acceleration; 
    public float maxspeed; 
    public float turnpower;
    public float mass;
    public float friction;

    private Sensors sensors;

//	private float g = 9.8f;//gravitational acc
//	private float mu = 0.7f;//coeff friction brakes

	private float angDrag = 10;

    private Rigidbody2D rb;

	void Awake(){
		ID = id_gen.nextid ();
	}

    // Use this for initialization
    void Start () {
        rb = gameObject.GetComponent<Rigidbody2D>();
		if (rb) {
			rb.mass = this.mass;
			rb.angularDrag = angDrag;
		}
        sensors = gameObject.GetComponent<Sensors>();
	}

	public int GetID(){
		return ID;
	}
	
	// Should be called every frame to max out speed
	public void AdjustSpeed () {

        if (rb.velocity.magnitude > maxspeed)
        {
            rb.velocity = rb.velocity.normalized * maxspeed;
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
		if (rb.velocity.magnitude > 0)
			rb.velocity *= 0.96f;
	}

    public void brake()
    {
        if (rb.velocity.magnitude > 0)
			rb.velocity *=0.7f;
    }

    public void turnLeft()
    {
        if (rb.velocity.magnitude > 0)
        {
			rb.angularDrag = 0;
            transform.Rotate(Vector3.forward * turnpower);
            rb.AddForce(new Vector2(transform.right.x, transform.right.y) * -turnpower); //centripetal force
			rb.angularDrag = angDrag;
        }
    }

    public void turnRight()
    {
        if (rb.velocity.magnitude > 0)
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
