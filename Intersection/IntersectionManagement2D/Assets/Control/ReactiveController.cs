using UnityEngine;
using System.Collections;

public class ReactiveController : MonoBehaviour {

    private CarModel car;
    private Sensors data;
    private ReferencePoint origin, destination;
    private float[] prev = { 0f, 0f, 0f };

    void Start()
    {
        car = gameObject.GetComponent<CarModel>();
        data = car.GetSensors();
    }


    void FixedUpdate()
    {
        if (data == null) data = car.GetSensors();
        car.AdjustSpeed();

        //Reactive logic
        float[] dists = data.getDistances();
        if ( prev[0] == -1 ) prev = dists;

        //new val
        var dfront = dists[(int)Sensors.SensorDirection.FRONT] - prev[(int)Sensors.SensorDirection.FRONT];
        if (dfront >= 0)
        {
            //increasing distance to front
            car.accelerate();
        } 
        else if(dfront < 0 )
        {
            //decreasing distance to front
            car.decelerate();
        }

        //update distances for next iteration
        prev = dists;

        // emergency brake
        if (dists[(int)Sensors.SensorDirection.FRONT] < .40f) car.brake();

    }

    public void setRoute(ReferencePoint origin, ReferencePoint destination)
    {
        this.origin = origin;
        this.destination = destination;
    }

}
