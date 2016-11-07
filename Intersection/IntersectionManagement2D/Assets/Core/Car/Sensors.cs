#define _DEBUG_

using UnityEngine;
using System.Collections;

public class Sensors : MonoBehaviour
{
    public enum SensorDirection{LEFT = 0, FRONT = 1, RIGHT = 2 };
    private CircleCollider2D col;
	public float radius = 1.5f;
    public float side_range = 1.0f;
    public float front_range = 1.5f;
    private float[] distances;

	// Use this for initialization
	void Start ()
	{
        distances = new float[] { side_range, front_range, side_range };
        col = gameObject.AddComponent<CircleCollider2D>();
	    col.radius = radius;
	    //var rb = GetComponent<Rigidbody2D>();
	    //rb.isKinematic = true;
        col.isTrigger = true;
	}

    //left, front and right
    public float[] getDistances()
    {
        return distances;
    }

    public void OnTriggerStay2D(Collider2D collider)
    {
        float width_offset = (GetComponent<BoxCollider2D>().size.x / 2f) + 0.2f;
        float height_offset = GetComponent<BoxCollider2D>().size.y / 2f;

        Vector3 origin = gameObject.transform.position;
        Vector3 left = (origin + gameObject.transform.right * width_offset),
                right = (origin - gameObject.transform.right * width_offset);

		if (collider == collider.gameObject.GetComponent<CircleCollider2D>()){
			return;
		}
#if _DEBUG_
        Debug.DrawRay (origin + (gameObject.transform.up * height_offset), gameObject.transform.up * front_range);
		Debug.DrawRay (left, (gameObject.transform.up + gameObject.transform.right * side_range));
		Debug.DrawRay (right, (gameObject.transform.up - gameObject.transform.right * side_range));
#endif

        RaycastHit2D l,f,r;
        int streetlayer = LayerMask.GetMask("Street");
        if (f = Physics2D.Raycast(origin + (transform.up * height_offset), gameObject.transform.up, front_range, streetlayer))
        {
            distances[(int)SensorDirection.FRONT] = Vector2.Distance(f.point, origin + (transform.up * height_offset));
        }
        else distances[(int)SensorDirection.FRONT] = front_range;
        if (r = Physics2D.Raycast(right, gameObject.transform.up + gameObject.transform.right, side_range, streetlayer))
        {
            distances[(int)SensorDirection.RIGHT] = Vector2.Distance(r.point, right);
        }
        else distances[(int)SensorDirection.RIGHT] = side_range;
        
        if (l = Physics2D.Raycast(left, gameObject.transform.up - gameObject.transform.right, side_range, streetlayer))
        {
            distances[(int)SensorDirection.LEFT] = Vector2.Distance(l.point, left);
        }
        else distances[(int)SensorDirection.LEFT] = side_range;
        
    }

}
