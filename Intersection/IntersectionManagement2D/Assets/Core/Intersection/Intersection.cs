
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;

//sets up the intersection, origins, destinations

public class Intersection : MonoBehaviour{

	static public Vector3 center = new Vector3(0,0,0);
	static private int cCount = 0;
	static private int aCount = 0;
	static private int oCount = 0;
	static private int lCount = 0;
	static private int pCount = 0;
	static private int dCount = 0;
	static private List<Vector2> presAtcol = new List<Vector2>();
	static private List<Vector2> presAtdead = new List<Vector2>();
	static private bool collUpdate = true;

	private BoxCollider2D bcol;
	private int ORIGINS = 4, DESTINATIONS = 4;
	private int group = 0;
	private GameObject[] origins, destinations, trafficLights;
	private int lightRate; //number of frames
	private int DEADLOCK = 500, deadCtr=0, precpres=0;
	private bool sameDeadlock = false;
	private int TrafficLight_RATE = 1200;//20secs -> * 3  ~ 1 m;

	public bool writeResults = false;
	public bool trafficLightMode = false;
	public bool canTurn = true;
	public float time;
	public int trafficLightDuration;
	public float trafficLightOffset = 3;
	public Graph graph;
	public int collisions;
	public int arrived;
	public int created;
	public int lost;
	public int present;
	public int deadlocks;
	public float spawnRate;
	public int spawnProbability;
	public float carMaxSpeed;
	public float carAcceleration;
	public Vector2[] presentAtCollision;
	public Vector2[] presentAtDeadlock;


	static public void increaseCollisions(){
		cCount++;
		if (collUpdate) {
			presAtcol.Add (new Vector2(pCount,Time.realtimeSinceStartup));
		}
		collUpdate = !collUpdate;
	}
	static public void increaseArrived(){
		aCount++;
	}
	static public void increaseCreated(){
		oCount++;
	}
	static public void increaseLost(){
		lCount++;
	}

	void Start(){
		origins = new GameObject[ORIGINS];
		destinations = new GameObject[DESTINATIONS];
//		SpriteRenderer sr;
		collisions = Intersection.cCount;
		arrived = Intersection.aCount;
		created = Intersection.oCount;
		lost = Intersection.lCount;
		graph = new Graph ();
		trafficLights = new GameObject[ORIGINS];

		GameObject go = Instantiate(Resources.Load("prefab/CarPrefab")) as GameObject;
		carMaxSpeed = RealWorldMeasure.translateSpeedKmh(go.GetComponent<CarModel>().maxspeed);
		carAcceleration = go.GetComponent<CarModel>().acceleration;
		Destroy (go);

		bcol = gameObject.AddComponent<BoxCollider2D> ();
		int width = IntersectionSize.laneOffset * 2 * 2;
		bcol.size = new Vector2 (width, width);
		bcol.isTrigger = true;

		for (int i = 0; i < ORIGINS; i++) {
			origins[i] = new GameObject("origin "+(i+1));
			origins[i].AddComponent<OriginPoint>();
			origins[i].GetComponent<OriginPoint>().setOrigin((ReferencePoint) i);
			origins[i].GetComponent<OriginPoint>().setTurn(canTurn);
//			sr = origins[i].AddComponent<SpriteRenderer>();
//			sr.sprite = UnityEditor.AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/Background.psd");
//			sr.color = Color.cyan;
			origins[i].transform.position = CoordinatesTranslator.translateOrigin(i);

		}
//		spawnRate = origins [0].GetComponent<OriginPoint> ().RATE;
		spawnProbability = origins [0].GetComponent<OriginPoint> ().PROB;

		for (int i = 0; i < DESTINATIONS; i++) {
			destinations[i] = new GameObject("destination "+ (i+1));
			destinations[i].AddComponent<DestinationPoint>();
//			sr = destinations[i].AddComponent<SpriteRenderer>();
//			sr.sprite = UnityEditor.AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/Background.psd");
//			sr.color = Color.red;
			destinations[i].transform.position = CoordinatesTranslator.translateDestination(i);
		}

		if (trafficLightMode) { 
			for (int i = 0; i < ORIGINS; i++) {
				trafficLights [i] = new GameObject ("trafficLight" + (i + 1));
				trafficLights [i].AddComponent<TrafficLight> ();
				trafficLights [i].GetComponent<TrafficLight> ().setOrigin (i);
			}

			lightRate = TrafficLight_RATE;
		}
	}

	IEnumerator turnTrafficLight(){
		trafficLights [group].GetComponent<TrafficLight> ().setRed ();
		trafficLights [group + 1].GetComponent<TrafficLight> ().setRed ();
		group = group == 0 ? 2 : 0;
		yield return new WaitForSeconds (trafficLightOffset);
		trafficLights [group].GetComponent<TrafficLight> ().setGreen ();
		trafficLights [group + 1].GetComponent<TrafficLight> ().setGreen ();
		yield return null;
	}


	void Update(){
		trafficLightDuration = (int) (TrafficLight_RATE * Time.deltaTime);
		spawnRate = (float) (origins [0].GetComponent<OriginPoint> ().RATE * Time.deltaTime);
		collisions = Intersection.cCount / 2;
		arrived = Intersection.aCount;
		created = Intersection.oCount;
		lost = Intersection.lCount;
		precpres = present;
		present = Intersection.pCount;
		deadlocks = Intersection.dCount;
		time = Time.realtimeSinceStartup;
		presentAtCollision = Intersection.presAtcol.ToArray ();
		presentAtDeadlock = Intersection.presAtdead.ToArray ();

		if (trafficLightMode) {
			if (lightRate == TrafficLight_RATE) {
				if (trafficLights [group].GetComponent<TrafficLight> ().canTurnRed () && trafficLights [group + 1].GetComponent<TrafficLight> ().canTurnRed ()) {
					StartCoroutine("turnTrafficLight");
					lightRate = 0;
				} else {
					Debug.Log ("cant turn red yet");
					return;
				}
			}
			lightRate++;
		}

		if (sameDeadlock && deadCtr == 0 && Intersection.presAtdead.Count > 0) {
			if (present != Intersection.presAtdead[Intersection.presAtdead.Count-1].x){ //same deadlock as before
				sameDeadlock = false;
			}else {
//				sameDeadlock = true; //still true
				return;
			}
		}
		if (present == precpres && present != 0) {
			deadCtr++;
		} else {
			deadCtr =0;
		}
		if (deadCtr == DEADLOCK) {
			Intersection.dCount++;
			Intersection.presAtdead.Add(new Vector2(present, Time.realtimeSinceStartup));
			deadCtr=0;
			sameDeadlock = true;
		}

	}

	public void OnTriggerEnter2D(Collider2D collider){
		if(collider.gameObject.GetComponent<CarModel>() && collider!= collider.gameObject.GetComponent<CircleCollider2D>() )
			Intersection.pCount++;
	}

	public void OnTriggerExit2D(Collider2D collider){
		if(collider.gameObject.GetComponent<CarModel>() && collider!= collider.gameObject.GetComponent<CircleCollider2D>() )
			Intersection.pCount--;
	}

	void OnApplicationQuit(){

		if (!writeResults)
			return;

//		Application.CaptureScreenshot ("TestResults/screenshot.png"); //too slow, cars are already destroyed
		string path = "TestResults/test" + DateTime.Now.ToString("ddMMHHmm") + ".txt";
		// This text is added only once to the file.
		if (!File.Exists(path)) 
		{
			// Create a file to write to.
			using (StreamWriter sw = File.CreateText(path)) 
			{
				sw.WriteLine("trafficLightMode:" + trafficLightMode);
				sw.WriteLine("canTurn:" + canTurn);
				sw.WriteLine("time:" + time);
				sw.WriteLine("trafficLightDuration:" + trafficLightDuration);
				sw.WriteLine("trafficLightOffset:" + trafficLightOffset);
	            sw.WriteLine("collisions:" + collisions);
				sw.WriteLine("arrived:" + arrived);
	            sw.WriteLine("created:"+created);
				sw.WriteLine("lost:"+lost);
	            sw.WriteLine("present:"+present);
				sw.WriteLine("deadlocks:"+deadlocks);
	            sw.WriteLine("spawnRate:"+spawnRate);
				sw.WriteLine("spawnProbability:"+spawnProbability);
	            sw.WriteLine("carMaxSpeed:"+carMaxSpeed);
				sw.WriteLine("carAcceleration:"+carAcceleration);
				sw.WriteLine();
				sw.WriteLine("#collisions: cars present in intersection and time:");
				for(int i=0; i< presentAtCollision.Length; i++)
					sw.WriteLine(presentAtCollision[i].x + "       " + presentAtCollision[i].y);
				sw.WriteLine();
				sw.WriteLine("#deadlocks: cars present in intersection and time:");
				for(int i=0; i< presentAtDeadlock.Length; i++)
					sw.WriteLine(presentAtDeadlock[i].x + "    " + presentAtDeadlock[i].y );

			}	
		}
	}
}