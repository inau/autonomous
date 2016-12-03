
using UnityEngine;

public enum ReferencePoint{ North=0, South, East, West };

public class ReferencePointUtil{
	static public ReferencePoint getOpposite(ReferencePoint o){
		switch (o) {
		case ReferencePoint.East:
			return ReferencePoint.West;
		case ReferencePoint.North:
			return ReferencePoint.South;
		case ReferencePoint.South:
			return ReferencePoint.North;
		case ReferencePoint.West:
			return ReferencePoint.East;
		default:
			return ReferencePoint.North;
		}
	}
	static public ReferencePoint getRandomDest(ReferencePoint origin){ //no U turns
		ReferencePoint d = (ReferencePoint) Random.Range (0, 3.999f);
		while (d == origin) {
			d = (ReferencePoint) Random.Range (0, 3.999f);
		}
		return d;
	}
}

public class SceneVars{
	 public static int streetLayer = 8;
}

public class IntersectionSize{
	static public int laneOffset = 1;
	static public int streetLength = 10;
}


public class CoordinatesTranslator {
	public static Vector3 translateOrigin( ReferencePoint o){
		int l = IntersectionSize.laneOffset;
		int s = IntersectionSize.streetLength;
		switch (o) {
		case ReferencePoint.North:
			return new Vector3(-l,s,0)  + Intersection.center;
		case ReferencePoint.South:
			return new Vector3(l,-s,0)  + Intersection.center;
		case ReferencePoint.East:
			return new Vector3(s,l,0)  + Intersection.center;
		case ReferencePoint.West:
			return new Vector3(-s,-l,0)  + Intersection.center;
		default:
			return new Vector3(0,0,0)  + Intersection.center;
		}
	}

	public static Vector3 translateDestination( ReferencePoint d){
		int l = IntersectionSize.laneOffset;
		int s = IntersectionSize.streetLength;
		switch (d) {
		case ReferencePoint.North:
			return new Vector3(l,s,0) + Intersection.center;
		case ReferencePoint.South:
			return new Vector3(-l,-s,0)  + Intersection.center;
		case ReferencePoint.East:
			return new Vector3(s,-l,0)  + Intersection.center;
		case ReferencePoint.West:
			return new Vector3(-s,l,0)  + Intersection.center;
		default:
			return new Vector3(0,0,0)  + Intersection.center;
		}
	}

	public static Vector3 getInitialRotation(ReferencePoint o){
		switch (o) {
		case ReferencePoint.South:
			return new Vector3(0,0,0);
		case ReferencePoint.North:
			return new Vector3(0,0,180);
		case ReferencePoint.West:
			return new Vector3(0,0,270);
		case ReferencePoint.East:
			return new Vector3(0,0,90);
		default:
			return new Vector3(0,0,0);
		}
	}

	public static Vector3 translateOrigin(int o){
		return translateOrigin ((ReferencePoint) o);
	}

	public static Vector3 translateDestination(int d){
		return translateDestination ((ReferencePoint) d);
	}

	static public ReferencePoint findClosestOrigin(Vector3 myPos){
		Vector3 closest = new Vector3();
		float dist = IntersectionSize.streetLength;
		for (int i=0; i<4; i++) {
			if (Vector3.Distance (myPos, CoordinatesTranslator.translateOrigin (i)) < dist) {
				dist = Vector3.Distance (myPos, CoordinatesTranslator.translateOrigin (i));
				closest = CoordinatesTranslator.translateOrigin (i);
			}
		}
		return ReverseTranslate(closest);
	}
	static public ReferencePoint ReverseTranslate(Vector3 pos){
		for (int i=0; i<4; i++) {
			if(translateOrigin(i)==pos) return (ReferencePoint) i;
			if (translateDestination(i) == pos) return (ReferencePoint) i;
		}
		return ReferencePoint.North;
	}
}

public class Direction{
	public static float Side(Vector2 a, Vector2 b){
		float dot = a.x*-b.y + a.y*b.x;
//		if (dot > 0)//b on the right of a
//		else if(dot < 0)//b on the left of a
//		else  //b parallel/antiparallel to a
		return dot;
	}

	public static float Side(Vector3 a, Vector3 b){
		float dot = a.x*-b.y + a.y*b.x;
		return dot;
	}
}

public class RealWorldMeasure{
	static public float carLengthU = 0.8f;//unity
	static public float carLengthR = 4.4f;//meters
	static public float translateRatio = carLengthR / carLengthU; //5.5
	static public float translateSpeedKmh(float speed){
		return (speed * translateRatio) * 3.6f;
	}
	static public float translateDistanceM(float dist){
		return dist * translateRatio;
	}
}
