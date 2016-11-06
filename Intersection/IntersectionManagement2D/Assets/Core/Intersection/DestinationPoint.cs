using UnityEngine;
using System.Collections;

public class DestinationPoint : MonoBehaviour{

	//destroys cars that reach
	
	void Start(){
		gameObject.AddComponent<CircleCollider2D> ();

	}
	
	void Update(){
		
	}

	void OnCollisionEnter2D (Collision2D col){
		Destroy (col.gameObject);
	}
	
}
