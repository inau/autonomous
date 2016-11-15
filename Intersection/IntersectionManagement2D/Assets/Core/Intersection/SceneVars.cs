
using UnityEngine;

public enum ReferencePoint{ North=0, South, East, West };

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
}