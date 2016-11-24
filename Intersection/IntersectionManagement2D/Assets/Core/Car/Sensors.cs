#define _DEBUG_

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
	//for debugging distances TODO remove it
	public Vector2[] ds;

	public enum SensorDirection{LEFT = 0, FRONT = 1, RIGHT = 2 };
	public float radius = 3.0f;
	public DistanceStruct[] distances = new DistanceStruct[3];
	public float[] deltas;
	public float rightDelta, rightDist;

	private CircleCollider2D col;
	private Vector3 frontL, frontR, L, R, rightHand;
	private Vector3 other;
	private float alpha, beta, gamma, x, y, rightHandAngle; 
	//alpha= angle between the two front vectors
	//beta= angle between the two external ones
	//gamma= angle between my object front and vector connecting my object to the other one
	private bool triggered = false;
	private Collider2D otherCollider = null, tmpCol;
	private RaycastHit2D hitF, hitFR, hitFL, hitR;
	private float defaultID = -1f;
	private float height_offset, width_offset;
	private float minDist;


	void Start ()
	{
        distances = new DistanceStruct[] { new DistanceStruct(defaultID, radius),
			new DistanceStruct(defaultID, radius),
			new DistanceStruct(defaultID, radius) 
		};
		deltas = new float[]{radius,radius,radius};
		rightDelta = radius;
		rightDist = radius;
        col = gameObject.AddComponent<CircleCollider2D>();
	    col.radius = radius;
        col.isTrigger = true;

		// NB angles are in radiants
		alpha = Mathf.PI / 6;
		beta = Mathf.PI;
		rightHandAngle = Mathf.PI / 4;

		height_offset = GetComponent<BoxCollider2D>().size.y / 2f;
		width_offset = (GetComponent<BoxCollider2D>().size.x / 2f);

		//TODO remove
		ds = new Vector2[3];
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
		if (collider.gameObject.GetComponent<TrafficLight>()){
			if(collider.gameObject.GetComponent<TrafficLight>().isRed()){
				updateDistance((int) SensorDirection.FRONT, -1, Vector3.Distance(transform.position, collider.gameObject.transform.position));
			} else {
				updateDistance((int) SensorDirection.FRONT, -1, radius);
			}
			return;
		}else if (collider == collider.gameObject.GetComponent<CircleCollider2D> () || collider.gameObject.layer != SceneVars.streetLayer) {
			return;
		}

		other = collider.gameObject.transform.position - transform.position;

		x = transform.up.x;
		y = transform.up.y;
		frontR = new Vector3 (x * Mathf.Cos (alpha / 2.0f) + y * Mathf.Sin (alpha / 2.0f), -x * Mathf.Sin (alpha / 2.0f) + y * Mathf.Cos (alpha / 2.0f), 0);
		frontL = new Vector3 (x * Mathf.Cos (- alpha / 2.0f) + y * Mathf.Sin (- alpha / 2.0f), -x * Mathf.Sin (- alpha / 2.0f) + y * Mathf.Cos (- alpha / 2.0f), 0);
		rightHand = new Vector3 (x * Mathf.Cos (rightHandAngle) + y * Mathf.Sin (rightHandAngle), -x * Mathf.Sin (rightHandAngle) + y * Mathf.Cos (rightHandAngle), 0);

#if _DEBUG_
//		R = new Vector3(x * Mathf.Cos(beta/2.0f) + y * Mathf.Sin(beta/2.0f), -x*Mathf.Sin (beta/2.0f) + y*Mathf.Cos (beta/2.0f), 0);
//		L = new Vector3 (x * Mathf.Cos (- beta / 2.0f) + y * Mathf.Sin (- beta / 2.0f), -x * Mathf.Sin (- beta / 2.0f) + y * Mathf.Cos (- beta / 2.0f), 0);
		Debug.DrawRay( transform.position + (transform.up * height_offset), transform.up*(radius-height_offset));
		Debug.DrawRay( transform.position + (transform.up * height_offset) + (transform.right * width_offset), frontR*(radius-height_offset));
		Debug.DrawRay( transform.position + (transform.up * height_offset) - (transform.right * width_offset) , frontL*(radius-height_offset));
		Debug.DrawRay( transform.position + (transform.right * width_offset), rightHand*(radius), Color.green);
//		Debug.DrawRay( transform.position, R * radius);
//		Debug.DrawRay( transform.position, L * radius);
#endif

		//logic with raycasting
		//raycast front
		hitF = Physics2D.Raycast (transform.position + (transform.up * height_offset), transform.up, radius-height_offset, LayerMask.GetMask("Street"));
		hitFR = Physics2D.Raycast (transform.position + (transform.up * height_offset) + (transform.right * width_offset),frontR.normalized, radius-height_offset, LayerMask.GetMask("Street"));
		hitFL = Physics2D.Raycast (transform.position + (transform.up * height_offset) - (transform.right * width_offset),frontL.normalized, radius-height_offset, LayerMask.GetMask("Street"));

		if( (hitF.collider != null && hitF.collider != col) || (hitFR.collider != null && hitFL.collider != col) || (hitFL.collider != null && hitF.collider != col) ){
			minDist = radius;
			if(hitF.collider!=null && hitF.distance < minDist){
				minDist = hitF.distance;
				tmpCol = hitF.collider;
			}
			if(hitFR.collider!=null && hitFR.distance < minDist){
				minDist = hitFR.distance;
				tmpCol = hitFR.collider;
			}
			if(hitFL.collider!=null && hitFL.distance < minDist){
				minDist = hitFL.distance;
				tmpCol = hitFL.collider;
			}
			updateDistance ((int)SensorDirection.FRONT, tmpCol.gameObject.GetComponent<CarModel> ().GetID (), minDist);
			otherCollider = tmpCol;
		}	else { //not in front
			//use old one for the side
			//logic with conjunction center-center 
			gamma = Mathf.Acos (Vector3.Dot (transform.up.normalized, other.normalized));
					if (gamma <= beta / 2) {
						if (gamma <= alpha / 2) { 
						//there is a corner case in which a front car can fit into the rays, if parallel
//							Debug.DrawRay( transform.position, other, Color.green);
							updateDistance ((int)SensorDirection.FRONT, (float)collider.gameObject.GetComponent<CarModel> ().GetID (), other.magnitude);
							otherCollider = collider;
							triggered = true;
						} else {
//							Debug.DrawRay( transform.position, other, Color.yellow);
							if (Direction.Side(transform.up, other) > 0)
								updateDistance ((int)SensorDirection.RIGHT, (float)collider.gameObject.GetComponent<CarModel> ().GetID (), other.magnitude);
							else
								updateDistance ((int)SensorDirection.LEFT, (float)collider.gameObject.GetComponent<CarModel> ().GetID (), other.magnitude);
						}
					} else
						resetDistances (collider.gameObject.GetComponent<CarModel> ().GetID ());
		}
		hitR = Physics2D.Raycast(transform.position + (transform.right * width_offset), rightHand.normalized, radius, LayerMask.GetMask("Street"));
		if (hitR.collider != null && hitR.collider != col) {
//			Debug.Log ("sensor " + GetComponent<CarModel> ().GetID () + " dist to " + hitR.collider.GetComponent<CarModel> ().GetID () + " = " + hitR.distance );
			rightDelta = hitR.distance - rightDist;
			rightDist = hitR.distance;
		} else {
			rightDist = radius;
			rightDelta = 0;
		}
	}

	void updateDistance(int index, float id, float newdist){

		//remove same care records in other positions
		for (int i = 0; i<distances.Length; i++) {
			if(i==index)
				continue;
			if(distances[i].id == id){
				deltas[i] = 0;
				distances[i].dist = radius;
			}
		}

		if(id == distances[index].id || newdist < distances[index].dist){
//			newdist += -GetComponent<BoxCollider2D>().size.y;
			deltas[index] = newdist - distances[index].dist;
			distances[index].id = id;
			distances[index].dist = newdist;
		}

	}
	
//	void OnTriggerEnter2D( Collider2D other )
//	{
//		if (other.gameObject.layer != SceneVars.streetLayer) {
//			return;
//		}
//		triggered = true;
//		otherCollider = other;
//
//	}

	void Update(){
		//debugging distances
		for (int i=0; i<distances.Length; i++) {
			ds[i].x=distances[i].id;
			ds[i].y=distances[i].dist;
		}

		radius = 1 + GetComponent<Rigidbody2D> ().velocity.magnitude;
		if (radius > 3) {
			radius = 3;
		}
		col.radius = radius;

		if (triggered && !otherCollider) {
			//the other collider was destroyed!
			// need to reset distance for its ID
			//NB otherCollider is set only for front 
			resetDistances();
//			Debug.Log("someone was destroyed!");
			triggered = false;
		}
	}

	void OnTriggerExit2D(Collider2D collider){

		//TODO when a car is in my collider and gets destroyed, this is not triggered

		if (collider.gameObject.layer != SceneVars.streetLayer) {
			return;
		}
		resetDistances (collider.gameObject.GetComponent<CarModel> ().GetID ());
		rightDist = radius;
		rightDelta = 0;

		triggered = false;
		otherCollider = null;
	}

	void resetDistances(int id){
		for (int i = 0; i < distances.Length; i++) {
			if (distances[i].id == (float) id){
				deltas[i] = 0;
				distances[i].dist = radius;
			}
		}
	}

	void resetDistances(){
		for (int i = 0; i < distances.Length; i++) {
			deltas[i] = 0;
			distances[i].dist = radius;
		}
	}

	void OnCollisionEnter2D(Collision2D collider){

		if (collider.gameObject.layer == SceneVars.streetLayer) {
			Intersection.increaseCollisions ();
//			//hard reset
//			for (int i = 0; i < distances.Length; i++){
//				deltas[i] = -distances[i].dist;
//				distances[i].id = collider.gameObject.GetComponent<CarModel> ().GetID ();
//				distances[i].dist = 0;
//			} //NB canceled by some other reset calls, it doesn really impact
		} else if (collider.gameObject.GetComponent<DestinationPoint> ()) {
			if (collider.gameObject.GetComponent<DestinationPoint>().transform.position.Equals(CoordinatesTranslator.translateDestination(GetComponent<ReactiveController>().destination)))
				Intersection.increaseArrived ();
			else
				Intersection.increaseLost();
		}
	}

}
