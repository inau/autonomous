using UnityEngine;
using System.Collections;

public class ReactiveController : MonoBehaviour {

    private CarModel car;
    private Sensors data;
    private ReferencePoint origin, destination;
    private float[] prev;
	private float[] dists;
	private float dfront, dleft, dright;

    void Start()
    {
        car = gameObject.GetComponent<CarModel>();
        data = car.GetSensors();
		prev = null;
    }


    void FixedUpdate()
    {
        if (data == null) data = car.GetSensors();
        car.AdjustSpeed();
		if (prev == null) prev = data.getDistances();

        //Reactive logic
        dists = data.getDistances();

		Debug.Log ( (dists[0] == prev[0]) + " " + (dists[1] == prev[1]) + " " + (dists[2]==prev[2]) );

        //new val
        dfront = dists[(int)Sensors.SensorDirection.FRONT] - prev[(int)Sensors.SensorDirection.FRONT];
		dleft = dists[(int)Sensors.SensorDirection.LEFT] - prev[(int)Sensors.SensorDirection.LEFT];
		dright = dists[(int)Sensors.SensorDirection.RIGHT] - prev[(int)Sensors.SensorDirection.RIGHT];

//		Debug.Log (dfront + " " + dleft + " " + dright);
		if (dfront < 0 || dleft < 0 || dright < 0){
//			car.decelerate();
			Debug.Log(" too close! ");
			car.brake();
		} else if ( dfront >=0 ){
			car.accelerate();
		}

        //update distances for next iteration
        //prev = dists;
		prev [0] = dists [0];
		prev [1] = dists [1];
		prev [2] = dists [2];

        // emergency brake
        if (dists[(int)Sensors.SensorDirection.FRONT] < 1.5f) car.brake();

    }

    public void setRoute(ReferencePoint origin, ReferencePoint destination)
    {
        this.origin = origin;
        this.destination = destination;
    }

}
