using UnityEngine;
using System.Collections;

public class Sensors : MonoBehaviour
{

    private CircleCollider2D col;
	public float radius = 1.5f;

	// Use this for initialization
	void Start ()
	{	    
	    col = gameObject.AddComponent<CircleCollider2D>();
	    col.radius = radius;
	    //var rb = GetComponent<Rigidbody2D>();
	    //rb.isKinematic = true;
        col.isTrigger = true;
	}

    public void OnTriggerEnter2D(Collider2D collider)
    {

	}

    public void OnTriggerStay2D(Collider2D collider)
    {
		float offset = 0.4f;
		Vector3 origin = gameObject.transform.position;

		Debug.DrawRay (origin, gameObject.transform.up * radius);
		Debug.DrawRay (((origin + gameObject.transform.right * ((GetComponent<BoxCollider2D>().size.x / 2f ) + 0.01f))), (gameObject.transform.up + gameObject.transform.right *offset) , Color.red);
		Debug.DrawRay (origin, (gameObject.transform.up - gameObject.transform.right * offset) * radius);
		
//		if(Physics2D.Raycast (gameObject.transform.position, gameObject.transform.up, radius))
//			Debug.Log("there is something in front of the car");
		if (Physics2D.Raycast (((origin + gameObject.transform.right * ((GetComponent<BoxCollider2D>().size.x / 2f ) + 0.01f))),
		                       (gameObject.transform.up + gameObject.transform.right *offset), 0.1f))
			Debug.Log ("there is something in the front-right of the car");
//		if (Physics2D.Raycast (gameObject.transform.position, gameObject.transform.up - gameObject.transform.right *offset, radius))
//			Debug.Log ("there is something in the front-left of the car");
    }

//    public void OnCollisionEnter2D(Collision2D collison)
//    public void OnTriggerExit2D(Collider2D collider)

}
