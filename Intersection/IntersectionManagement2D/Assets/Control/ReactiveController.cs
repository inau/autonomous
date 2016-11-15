using UnityEngine;
using System.Collections;

public class ReactiveController : MonoBehaviour {

    private CarModel car;
    private Sensors data;
    private ReferencePoint origin, destination;
    private DistanceStruct[] dists;
	private float[] deltas;
	private float dfront, dleft, dright;

    void Start()
    {
        car = gameObject.GetComponent<CarModel>();
        data = car.GetSensors();
    }


    void FixedUpdate()
    {
		if (data == null)
			data = car.GetSensors ();
		car.AdjustSpeed ();

		//Reactive logic
		dists = data.getDistances ();
		deltas = data.getDeltas ();

		dfront = deltas [(int)Sensors.SensorDirection.FRONT];
		dleft = deltas [(int)Sensors.SensorDirection.LEFT];
		dright = deltas [(int)Sensors.SensorDirection.RIGHT];


		// emergency brake
		if (dists [(int)Sensors.SensorDirection.FRONT].dist < 1.5) {
			car.brake ();
			return;
		}
		if (dfront >= 0) {
			car.accelerate ();
			return;
		}

		if (dfront < 0 || dleft < 0 || dright < 0){
			car.slowbrake();	
//			Debug.Log(" too close! ");
//			car.brake();
		} 



    }

    public void setRoute(ReferencePoint origin, ReferencePoint destination)
    {
        this.origin = origin;
        this.destination = destination;
    }

}
