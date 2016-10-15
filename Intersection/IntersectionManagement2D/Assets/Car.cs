using UnityEngine;
using System.Collections;

public class Car : MonoBehaviour {

	ReferencePoint origin, destination;
	float velocity, steeringAngle, heading, wheelBase;
	Vector2 position, frontWheel, backWheel;
	Transform transform;

	public Car(ReferencePoint _origin, ReferencePoint _destination, Transform _transform){
		transform = _transform;
		origin = _origin;
		destination = _destination;
		velocity = 0;
		steeringAngle = 0;
		wheelBase = 5;
		position = findPosition(_origin, _destination);
		heading = findHeading (_origin, _destination);
		frontWheel = position + wheelBase / 2 * new Vector2 (Mathf.Cos(heading), Mathf.Sin(heading));
		backWheel = position - wheelBase / 2 * new Vector2 (Mathf.Cos(heading), Mathf.Sin(heading));

		updatePosition ();
	}

	Vector2 findPosition(ReferencePoint o, ReferencePoint d){
		//place the car somewhere, give it a position, also place the gameobject
		return new Vector2 (0, 0);
	}
	float findHeading(ReferencePoint o, ReferencePoint d){
		//find direction where the car is facing, also rotate game object accordingly
		return 90;
	}

	public void stop(){
		velocity = 0;
	}

	public float getVelocity(){
		return velocity;
	}
			
	public void turn(float angle){
		steeringAngle += angle;
	}

	public void accelerate(float speed){
		velocity += speed;
	}


	public void updatePosition(){
		//update game object position
		float newheading = heading;
		if (velocity != 0) {
			backWheel += velocity * new Vector2(Mathf.Cos(heading), Mathf.Sin(heading)); //*dt time between frames
			frontWheel += velocity * new Vector2(Mathf.Cos(heading + steeringAngle), Mathf.Sin(heading + steeringAngle)); //*dt
			
			position = (frontWheel + backWheel) / 2;
			newheading = Mathf.Atan2( frontWheel.y - backWheel.y , frontWheel.x - backWheel.x );

		}

		transform.position = position;
		if (newheading != heading){
			heading = newheading;
			transform.Rotate( Vector3.forward * heading);
		}
	}
	
}
