using UnityEngine;
using System;

public class ReactiveController : MonoBehaviour{

	public ReferencePoint origin, destination;

	private CarModel car;
	private Sensors data;
	private DistanceStruct[] dists;
	private float[] deltas;
	private float dfront, dleft, dright;
	private Graph graph;
	private Node currentNode, nextNode;
//	private bool state = true, prevstate; //true: moving false: breaking
//	private int waitTime=0, INTERVAL = 100;
	
	void Start()
	{
		GameObject inter;
		car = gameObject.GetComponent<CarModel>();
		data = car.GetSensors();
		inter = GameObject.Find ("Intersection");
		if (inter != null)
			graph = inter.GetComponent<Intersection> ().graph;
		else {
			Debug.Log ("didn find graph");
			graph = null;
			return;
		}
		currentNode = graph.nodes[CoordinatesTranslator.translateOrigin (origin)];
		nextNode = graph.findNext (currentNode, origin, destination);
	}
	
	
	void FixedUpdate()
	{
		if (data == null)
			data = car.GetSensors ();
		car.AdjustSpeed ();

		//Reactive logic
		dists = data.getDistances ();
		deltas = data.getDeltas ();
		
		dfront = deltas [(int)Sensors.SensorDirection.FRONT];
		dleft = deltas [(int)Sensors.SensorDirection.LEFT];
		dright = deltas [(int)Sensors.SensorDirection.RIGHT];

		//wait before restarting?
//		prevstate = state;
//		state = (dfront >= -0.0001f);
//		if (waitTime > 0 || (!prevstate && state)) { //was still and now I want to move
//			//wait for some frames more
//			waitTime++;
//			if (waitTime < INTERVAL) {
//				state = false; //not yet buddy
//				car.slowbrake ();
//				Debug.Log (GetComponent<CarModel> ().GetID () + " waiting to restart ");
//				return;
//			} 
//			waitTime = 0;
//		} else {
//			waitTime = 0;
//		}
		//if traffic lights, check if red
		if (data.isRedLight ()) {
			car.slowbrake();
			return;
		}
		// emergency brake, too close
		if (dists [(int)Sensors.SensorDirection.FRONT].dist < data.radius /2) {
			car.brake ();
//			return;
		} else if (data.rightDelta < 0){
			car.brake ();
//			Debug.Log("right hand applies");
//			return;
//		//right hand rule?
//		} else if (dright < 0) {
//			car.slowbrake();
//			return;  
//			//only works with a wider angle, if the car is exactly to the right I could still pass instead of stopping in front of it
		} else if (dfront >= -0.0001f) {
			car.accelerate ();
		}else if (dfront < -0.0001f || dleft < -0.0001f || dright < 0.0001f){
			car.slowbrake();
		} 
//		} else {
//			car.brake();
//		}

		//follow next node
		if (graph!=null && nextNode!=null) {
			//check if I need to update current and next node
			if((nextNode.position - transform.position).magnitude < GetComponent<BoxCollider2D>().size.y/2){
				currentNode = nextNode;
				nextNode = graph.findNext(currentNode, origin, destination);
			}
			//follow next, turn
			if (nextNode!=null){
//				Debug.DrawLine(transform.position, nextNode.position, Color.red);
//				if (Vector3.Dot (transform.up, nextNode.position - transform.position) > 0){
				if (Direction.Side (transform.up, nextNode.position - transform.position) > 0.02f){
					car.slowbrake();
					car.turnRight();
//				}else if (Vector3.Dot(transform.up, nextNode.position - transform.position) < 0){
				} else if (Direction.Side (transform.up, nextNode.position - transform.position) < -0.02f){
					car.slowbrake();
					car.turnLeft();
				}
			}
		}
		
		
		
		
	}
	
	public void setRoute(ReferencePoint origin, ReferencePoint destination)
	{
		this.origin = origin;
		this.destination = destination;
	}

}


