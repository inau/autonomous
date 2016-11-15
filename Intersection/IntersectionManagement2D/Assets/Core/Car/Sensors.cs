//#define _DEBUG_

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DistanceStruct{
	public float id;
	public float dist;
	public DistanceStruct(float _id, float _dist){
		id = _id;
		dist = _dist;
	}
}

public class Sensors : MonoBehaviour
{
	public enum SensorDirection{LEFT = 0, FRONT = 1, RIGHT = 2 };
	public float radius;
    public float side_range = 3.0f;
    public float front_range = 3.0f;

	private CircleCollider2D col;
	public DistanceStruct[] distances;
	public float[] deltas;
//	private float width_offset, height_offset;
	private Vector3 frontL, frontR, L, R;
	private Vector3 other;
	private float alpha, beta, gamma, x, y; 
	//alpha= angle between the two front vectors
	//beta= angle between the two external ones
	//gamma= angle between my object front and vector connecting my object to the other one

	void Start ()
	{
		radius = Mathf.Max (side_range, front_range);
        distances = new DistanceStruct[] { new DistanceStruct(-1f, side_range), new DistanceStruct(-1f, front_range), new DistanceStruct(-1f, side_range) };
		deltas = new float[]{side_range, front_range, side_range};
        col = gameObject.AddComponent<CircleCollider2D>();
	    col.radius = radius;
        col.isTrigger = true;
//		width_offset = (GetComponent<BoxCollider2D>().size.x / 2f) + 0.2f;
//		height_offset = GetComponent<BoxCollider2D>().size.y / 2f;
		// NB angles are in radiants
		alpha = Mathf.PI / 4;
		beta = Mathf.PI;
	}

    //left, front and right
    public DistanceStruct[] getDistances()
    {
        return distances;
    }
	public float[] getDeltas(){
		return deltas;
	}

    public void OnTriggerStay2D(Collider2D collider)
    {
        
		if (collider == collider.gameObject.GetComponent<CircleCollider2D> () || collider.gameObject.layer != SceneVars.streetLayer) {
			return;
		}

		other = collider.gameObject.transform.position - transform.position;

		#if _DEBUG_
		x = transform.up.x;
		y = transform.up.y;
		frontR = new Vector3 (x * Mathf.Cos (alpha / 2.0f) + y * Mathf.Sin (alpha / 2.0f), -x * Mathf.Sin (alpha / 2.0f) + y * Mathf.Cos (alpha / 2.0f), 0);
		R = new Vector3(x * Mathf.Cos(beta/2.0f) + y * Mathf.Sin(beta/2.0f), -x*Mathf.Sin (beta/2.0f) + y*Mathf.Cos (beta/2.0f), 0);
		frontL = new Vector3 (x * Mathf.Cos (- alpha / 2.0f) + y * Mathf.Sin (- alpha / 2.0f), -x * Mathf.Sin (- alpha / 2.0f) + y * Mathf.Cos (- alpha / 2.0f), 0);
		L = new Vector3 (x * Mathf.Cos (- beta / 2.0f) + y * Mathf.Sin (- beta / 2.0f), -x * Mathf.Sin (- beta / 2.0f) + y * Mathf.Cos (- beta / 2.0f), 0);

		Debug.DrawRay( transform.position, transform.up * radius, Color.red);
		Debug.DrawRay( transform.position, frontR * radius);
		Debug.DrawRay( transform.position, frontL * radius);
		Debug.DrawRay( transform.position, R * radius);
		Debug.DrawRay( transform.position, L * radius);
		#endif

		gamma = Mathf.Acos (Vector3.Dot (transform.up.normalized, other.normalized));

		if (gamma <= beta / 2) {
			if (gamma <= alpha / 2) {
				#if _DEBUG_
				Debug.Log ("FRONT dist: " + other.magnitude);
				Debug.DrawRay( transform.position, other, Color.green);
				#endif
				updateDistance ((int)SensorDirection.FRONT, (float)collider.gameObject.GetComponent<CarModel> ().GetID (), other.magnitude);
			} else {
				#if _DEBUG_
				Debug.DrawRay( transform.position, other, Color.yellow);
				Debug.Log ("SIDE dist: " + other.magnitude);
				#endif
				if (Vector3.Dot (transform.up, other) > 0) {
					updateDistance ((int)SensorDirection.RIGHT, (float)collider.gameObject.GetComponent<CarModel> ().GetID (), other.magnitude);
				} else {
					updateDistance ((int)SensorDirection.LEFT, (float)collider.gameObject.GetComponent<CarModel> ().GetID (), other.magnitude);
				}
			}
		} else {
			resetDistances (collider.gameObject.GetComponent<CarModel> ().GetID ());
		}
	}

	void updateDistance(int index, float id, float newdist){
		if(id == distances[index].id || newdist < distances[index].dist){
			deltas[index] = newdist - distances[index].dist;
			distances[index] = new DistanceStruct(id, newdist);
		}

	}

//		Debug.Log (distances[0] + " " + distances[1] + " " + distances[2]);


		//		front = gameObject.transform.position + (gameObject.transform.up * height_offset);
		//		left = (gameObject.transform.position + gameObject.transform.right * width_offset);
		//        right = (gameObject.transform.position - gameObject.transform.right * width_offset);
		//
//#if _DEBUG_
//        Debug.DrawRay (front, gameObject.transform.up * front_range);
//		Debug.DrawRay (left, (gameObject.transform.up + gameObject.transform.right * side_range));
//		Debug.DrawRay (right, (gameObject.transform.up - gameObject.transform.right * side_range));
//#endif       
//        streetlayer = LayerMask.GetMask("Street");
//        if (f = Physics2D.Raycast(front, gameObject.transform.up, front_range, streetlayer))
//        {
//            distances[(int)SensorDirection.FRONT] = Vector2.Distance(f.point, front);
//        }
//        else distances[(int)SensorDirection.FRONT] = front_range;
//        if (r = Physics2D.Raycast(right, gameObject.transform.up + gameObject.transform.right, side_range, streetlayer))
//        {
//            distances[(int)SensorDirection.RIGHT] = Vector2.Distance(r.point, right);
//        }
//        else distances[(int)SensorDirection.RIGHT] = side_range;
//        
//        if (l = Physics2D.Raycast(left, gameObject.transform.up - gameObject.transform.right, side_range, streetlayer))
//        {
//            distances[(int)SensorDirection.LEFT] = Vector2.Distance(l.point, left);
//        }
//        else distances[(int)SensorDirection.LEFT] = side_range;
//        
//    }

	void OnTriggerExit2D(Collider2D collider){
		if (collider.gameObject.layer != SceneVars.streetLayer) {
			return;
		}
		resetDistances (collider.gameObject.GetComponent<CarModel> ().GetID ());
	}

	void resetDistances(int id){
		for (int i = 0; i < distances.Length; i++) {
			if (distances[i].id == (float) id){
				deltas[i] = 0;
				distances[i].dist = radius;
			}
		}
	}

	void OnCollisionEnter2D(Collision2D col){
		if (col.gameObject.layer == SceneVars.streetLayer)
			Intersection.increaseCollisions ();
		else if (col.gameObject.GetComponent<DestinationPoint> ())
			Intersection.increaseArrived ();
	}

}
