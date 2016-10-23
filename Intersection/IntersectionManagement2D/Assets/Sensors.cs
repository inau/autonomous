using UnityEngine;
using System.Collections;

public class Sensors : MonoBehaviour
{

    private CircleCollider2D col;

	// Use this for initialization
	void Start ()
	{	    
	    col = gameObject.AddComponent<CircleCollider2D>();
	    col.radius = 4;
	    //var rb = GetComponent<Rigidbody2D>();
	    //rb.isKinematic = true;
        col.isTrigger = true;
	}
	
	// Update is called once per frame
	void Update () {
        
        
        	
	}

    public void OnCollisionEnter2D(Collision2D collison)
    {
        Debug.Log("Someone got VERY close..\n");
    }

    public void OnTriggerEnter2D(Collider2D collider)
    {
        Debug.Log("Someone got close..\n");
    }

    public void OnTriggerStay2D(Collider2D collider)
    {
        Debug.Log("Someone stays close..\n");
    }

    public void OnTriggerExit2D(Collider2D collider)
    {
        Debug.Log("Someone left..\n");
    }
}
