using UnityEngine;
using System.Collections;
using System;

public class TrafficLight : MonoBehaviour {
	private SpriteRenderer sr;
	private CircleCollider2D ccol;
//	private BoxCollider2D bcol;
	private bool canRed = true;
	private int origin;
//	private CarModel fakeCar;

	public void setOrigin(int i){
		origin = i;
	}

	void Start(){

//		fakeCar = gameObject.AddComponent<CarModel> (); //need this so the cars will stop
//		fakeCar.ID = -1; 

//		bcol = gameObject.AddComponent<BoxCollider2D> ();
//		bcol.isTrigger = true;
//		bcol.size = new Vector2(1,1);
		ccol = gameObject.AddComponent<CircleCollider2D> ();
		ccol.isTrigger = true;
		ccol.radius = 1;
		sr = gameObject.AddComponent<SpriteRenderer>();
		sr.sprite = UnityEditor.AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/Background.psd");
		sr.color = Color.green;
		gameObject.transform.position = Graph.findNodeFrom(origin, IntersectionSize.streetLength - 2);
//		gameObject.SetActive(false);
	}

	public void setRed(){
		if (canRed) {
			sr.color = Color.red;
//			gameObject.layer = SceneVars.streetLayer;
		}
	}
	public void setGreen(){
			sr.color = Color.green;
//			gameObject.layer = 0;
	}

	public bool isRed(){
		return sr.color == Color.red;
	}

	public bool canTurnRed(){
		return canRed;
	}

	public void onTriggerEnter2D(Collider2D other){
		if (canRed && other.gameObject.layer == SceneVars.streetLayer)
			canRed = false;
	}
	public void onTriggerStay2D(Collider2D other){
		if (canRed && other.gameObject.layer == SceneVars.streetLayer) {
			canRed = false;
		}
//		if (!isRed () && other.gameObject.layer == SceneVars.streetLayer) {
//			if (other.gameObject.GetComponent<Sensors>().distances[1].id == -1){ //front 
//				other.gameObject.GetComponent<Sensors>().distances[1].id = 3;
//				other.gameObject.GetComponent<Sensors>().deltas[1] = 0;
//			}
//		}
	}
	public void onTriggerExit2D(Collider2D other){
		if (!canRed) {
			canRed = true;
		}
	}

}
