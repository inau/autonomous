
using UnityEngine;
using System.Collections;

//spawns the cars

public class OriginPoint : MonoBehaviour{

	public int RATE = 15;
	public int PROB = 15; // %
	private int spawnRate; //number of frames
	
	ReferencePoint origin;
	GameObject newCar;
	SpriteRenderer sr;
	Rigidbody2D rb2d;
	Texture2D txt;
	Car car;

	void Start(){
		spawnRate = Random.Range(0,RATE);
	}

	void Update(){
		if (spawnRate%RATE == 0) {
			if (Random.Range (0, 100) < PROB) {
				CreateCar ();
			}
		}
		spawnRate++;
}


	public void setOrigin(ReferencePoint _origin){
		origin = _origin;
	}
	
	public void CreateCar(){
		newCar = new GameObject("Car");
		newCar.layer = SceneVars.streetLayer;
		rb2d = newCar.AddComponent<Rigidbody2D>();
		rb2d.gravityScale = 0;
		newCar.AddComponent<CarController2d>();
		car = newCar.AddComponent<Car> ();
		car.setCar (origin, (ReferencePoint) Random.Range(0,3));

		//rotation
		newCar.transform.Rotate ( CoordinatesTranslator.getInitialRotation (car.origin));

		sr = newCar.AddComponent<SpriteRenderer>();
		txt = Resources.Load ("car") as Texture2D;
		if (txt == null) {
			Debug.Log (" no texture loaded.");
			newCar.transform.localScale += new Vector3(2,2,0);
			sr.sprite = UnityEditor.AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/Background.psd");
			sr.color = Color.black;
		} else {
			sr.sprite = Sprite.Create (txt, new Rect (0, 0, 48, 88), new Vector2 ());
		}
		newCar.AddComponent<BoxCollider2D>();
		newCar.transform.position = this.transform.position;

	}
	
}
