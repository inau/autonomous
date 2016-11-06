using UnityEngine;
using System.Collections;

public class ReactiveController : MonoBehaviour {

    private CarModel car;
    private Sensors data;
    private ReferencePoint origin, destination;

    void Start()
    {
        car = gameObject.GetComponent<CarModel>();
        data = gameObject.GetComponent<Sensors>();
    }


    void FixedUpdate()
    {
        if (data == null) car.GetSensors();
        car.AdjustSpeed();

        //Reactive logic
        float[] dists = data.getDistances();
        //   Debug.Log(dists[0] + " " + dists[1] + " " + dists[2]);
        if (dists[(int)Sensors.SensorDirection.FRONT] < 1.40f ||
            dists[(int)Sensors.SensorDirection.LEFT] < .30f ||
            dists[(int)Sensors.SensorDirection.RIGHT] < .30f) { }
        //car.brake();
        else car.accelerate();

    }

    public void setRoute(ReferencePoint origin, ReferencePoint destination)
    {
        this.origin = origin;
        this.destination = destination;
    }

}
