
using UnityEngine;

public enum ReferencePoint{ North=0, South, East, West };

public class SceneVars{
	 public static int streetLayer = 8;
}

public class CoordinatesTranslator {
	public static Vector3 translateOrigin( ReferencePoint o){
		switch (o) {
		case ReferencePoint.North:
			return new Vector3(-1,10,0);
		case ReferencePoint.South:
			return new Vector3(1,-10,0);
		case ReferencePoint.East:
			return new Vector3(10,1,0);
		case ReferencePoint.West:
			return new Vector3(-10,-1,0);
		default:
			return new Vector3(0,0,0);
		}
	}

	public static Vector3 translateDestination( ReferencePoint d){
		switch (d) {
		case ReferencePoint.North:
			return new Vector3(1,10,0);
		case ReferencePoint.South:
			return new Vector3(-1,-10,0);
		case ReferencePoint.East:
			return new Vector3(10,-1,0);
		case ReferencePoint.West:
			return new Vector3(-10,1,0);
		default:
			return new Vector3(0,0,0);
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