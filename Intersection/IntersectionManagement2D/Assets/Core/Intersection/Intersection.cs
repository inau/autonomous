
using UnityEngine;
using System.Collections;

//sets up the intersection, origins, destinations

public class Intersection : MonoBehaviour{
	public bool trafficLightMode = false;
	public static Vector3 center = new Vector3(0,0,0);
	private int ORIGINS = 4, DESTINATIONS = 4;
	private int group = 0;
	private GameObject[] origins, destinations, trafficLights;
	public Graph graph;
	static private int cCount = 0;
	static private int aCount = 0;
	static private int oCount = 0;
	static private int lCount = 0;
	public int collisions;
	public int arrived;
	public int created;
	public int lost;

	public int TrafficLight_RATE = 500;
	private int lightRate; //number of frames

	static public void increaseCollisions(){
		cCount++;
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
		SpriteRenderer sr;
		collisions = Intersection.cCount;
		arrived = Intersection.aCount;
		created = Intersection.oCount;
		lost = Intersection.lCount;
		graph = new Graph ();
		trafficLights = new GameObject[ORIGINS];

		for (int i = 0; i < ORIGINS; i++) {
			origins[i] = new GameObject("origin "+(i+1));
			origins[i].AddComponent<OriginPoint>();
			origins[i].GetComponent<OriginPoint>().setOrigin((ReferencePoint) i);
			sr = origins[i].AddComponent<SpriteRenderer>();
			sr.sprite = UnityEditor.AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/Background.psd");
			sr.color = Color.cyan;
			origins[i].transform.position = CoordinatesTranslator.translateOrigin(i);
		}

		for (int i = 0; i < DESTINATIONS; i++) {
			destinations[i] = new GameObject("destination "+ (i+1));
			destinations[i].AddComponent<DestinationPoint>();
			sr = destinations[i].AddComponent<SpriteRenderer>();
			sr.sprite = UnityEditor.AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/Background.psd");
			sr.color = Color.red;
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
	void Update(){
		collisions = Intersection.cCount / 2;
		arrived = Intersection.aCount;
		created = Intersection.oCount;
		lost = Intersection.lCount;

		if (trafficLightMode) {
			if (lightRate == TrafficLight_RATE) {
				if (trafficLights [group].GetComponent<TrafficLight> ().canTurnRed () && trafficLights [group + 1].GetComponent<TrafficLight> ().canTurnRed ()) {
					trafficLights [group].GetComponent<TrafficLight> ().setRed ();
					trafficLights [group + 1].GetComponent<TrafficLight> ().setRed ();
					group = group == 0 ? 2 : 0;
					trafficLights [group].GetComponent<TrafficLight> ().setGreen ();
					trafficLights [group + 1].GetComponent<TrafficLight> ().setGreen ();
					lightRate = 0;
				} else {
					Debug.Log ("cant turn red yet");
					return;
				}
			}
			lightRate++;
		}
	}


}