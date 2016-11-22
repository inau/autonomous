using UnityEngine;
using System.Collections;

public class CleanupCars : MonoBehaviour {
    public float width, height;
    BoxCollider2D bounds;

	// Use this for initialization
	void Start () {
        bounds = gameObject.AddComponent<BoxCollider2D>();
        bounds.isTrigger = true;
        bounds.size = new Vector2(width,height);
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerExit2D(Collider2D collider)
    {
        Destroy(collider.gameObject);
		Intersection.increaseLost ();
    }

}
