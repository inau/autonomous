using UnityEngine;
using System.Collections;
using System;

public class TrafficLight : MonoBehaviour {
	private SpriteRenderer sr;
	private CircleCollider2D ccol;
//	private BoxCollider2D bcol;
	private int origin;
	private int present = 0;

	public void setOrigin(int i){
		origin = i;
	}

	void Start(){

//		bcol = gameObject.AddComponent<BoxCollider2D> ();
//		bcol.isTrigger = true;
//		if (origin == 0 || origin == 1)
//			bcol.size = new Vector2 (1, 0.1f);
//		else
//			bcol.size = new Vector2 (0.1f, 1);
		ccol = gameObject.AddComponent<CircleCollider2D> ();
		ccol.isTrigger = true;
		ccol.radius = 1.5f;
		sr = gameObject.AddComponent<SpriteRenderer>();
		sr.sprite = UnityEditor.AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/Background.psd");
		sr.color = Color.green;
		gameObject.transform.position = Graph.findNodeFrom(origin, IntersectionSize.streetLength - 2);
	}

	public void setRed(){
		if (present<=0) {
			sr.color = Color.red;
		}
	}
	public void setGreen(){
			sr.color = Color.green;
	}

	public bool isRed(){
		return sr.color == Color.red;
	}

	public bool canTurnRed(){
		return present<=0;
	}

	public void onTriggerEnter2D(Collider2D other){
		if (other.gameObject.layer == SceneVars.streetLayer) {
			present++;
		}
	}

	public void onTriggerExit2D(Collider2D other){
		present--;
	}

}
