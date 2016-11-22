
using UnityEngine;
using System.Collections;

//sets up the intersection, origins, destinations

public class Intersection : MonoBehaviour{

	public static Vector3 center = new Vector3(0,0,0);
	private int ORIGINS = 4, DESTINATIONS = 4;
	private GameObject[] origins, destinations;
	public Graph graph;
	static private int cCount = 0;
	static private int aCount = 0;
	static private int oCount = 0;
	static private int lCount = 0;
	public int collisions;
	public int arrived;
	public int created;
	public int lost;

//	public int RATE = 15;
//	private int spawnRate; //number of frames

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

//		return;

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

//		spawnRate = 0;
	}
	void Update(){
		collisions = Intersection.cCount / 2;
		arrived = Intersection.aCount;
		created = Intersection.oCount;
		lost = Intersection.lCount;
	}
//		if (spawnRate == 0) {
//
//			origins[(int) Random.Range(0, ORIGINS)].GetComponent<OriginPoint>().CreateCar();
//			
//		}
//		spawnRate++;
//		if (spawnRate >= RATE)
//			spawnRate = 0;
//	}

}