
using UnityEngine;
using System.Collections;

//spawns the cars

public class OriginPoint : MonoBehaviour{

	GameObject newCar;
	SpriteRenderer sr;
	Rigidbody2D rb2d;
	Texture2D txt;
	Car car;

	void Start(){

	}
	
	public void CreateCar(int COUNT){
		newCar = new GameObject("Car " + COUNT++ );
		rb2d = newCar.AddComponent<Rigidbody2D>();
		rb2d.gravityScale = 0;
		newCar.AddComponent<CarController2d>();
		car = newCar.AddComponent<Car> ();
		car.setCar ((ReferencePoint) Random.Range(0, 3), (ReferencePoint) Random.Range(0,3));

		//rotation



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
