#define _DEBUG_

using System;
using UnityEngine;
using System.Collections.Generic;


public class Edge{
	public ReferencePoint leads;
	public Node from;
	public Node to;
	public Edge(Node _from, Node _to){
		from = _from;
		to = _to;
	}
	public Edge(Node _from, Node _to, ReferencePoint _leads){
		from = _from;
		to = _to;
		leads = _leads;
	}
	public Edge(Node _from, Node _to, int _leads){
		from = _from;
		to = _to;
		leads = (ReferencePoint) _leads;
	}
}

public class Node{
	public Vector3 position;
	public Dictionary<ReferencePoint, Edge> edges;//key represents in which direction it leads
	
	public Node(Vector3 pos){
		edges = new Dictionary<ReferencePoint, Edge>(); 
		position = pos;
	}
	public void AddEdge(ReferencePoint s, Edge e){ 
		if (edges.ContainsKey (s))
			return;
		edges.Add (s, e);
	}
	public void AddEdge(int s, Edge e){
		AddEdge ((ReferencePoint)s, e);
	}
}

public class Graph{

	public Dictionary<Vector3, Node> nodes;

	public Graph(){
		int i, j, direction = 1;
		Vector3 pos;
		nodes = new Dictionary<Vector3, Node> ();
		Node node, prev;// center = new Node (Intersection.center);
//		nodes.Add (Intersection.center, center);

		for (j = 0; j < 4; j++){
			direction = -direction; //N=0,E=2 -1, going "down" on y,x, S=1,W=3 1, going "up" on y,x
			pos = CoordinatesTranslator.translateOrigin(j);
			node = null;
			for (i = 0; i <= IntersectionSize.streetLength * 2; i++) {
				prev = node;
				node = new Node(pos);
				if (!nodes.ContainsKey(pos))
					nodes.Add(pos, node);
				else
					node = nodes[pos];
				if (prev != null)
					prev.AddEdge( j-direction, new Edge(prev, node, j-direction) );
				if (j < 2)//N,S
					pos.y = pos.y + direction;
				else //E,W
					pos.x = pos.x + direction;
			}
		}
		//all the nodes created,
		//need to connect the central nodes of the intersection
		//right turns
		prev = nodes [findNodeFrom (ReferencePoint.North, IntersectionSize.streetLength - 2)];
		node = nodes [findNodeTo (ReferencePoint.West, IntersectionSize.streetLength + 2)];
		prev.AddEdge( ReferencePoint.West, new Edge(prev, node, ReferencePoint.West));

	    prev = nodes [findNodeFrom (ReferencePoint.South, IntersectionSize.streetLength - 2)];
		node = nodes [findNodeTo (ReferencePoint.East, IntersectionSize.streetLength + 2)];
		prev.AddEdge( ReferencePoint.East, new Edge(prev, node, ReferencePoint.East));

		prev = nodes [findNodeFrom (ReferencePoint.East, IntersectionSize.streetLength - 2)];
		node = nodes [findNodeTo (ReferencePoint.North, IntersectionSize.streetLength + 2)];
		prev.AddEdge( ReferencePoint.North, new Edge(prev, node, ReferencePoint.North));

		prev = nodes [findNodeFrom (ReferencePoint.West, IntersectionSize.streetLength - 2)];
		node = nodes [findNodeTo (ReferencePoint.South, IntersectionSize.streetLength + 2)];
		prev.AddEdge( ReferencePoint.South, new Edge(prev, node, ReferencePoint.South));

		//left turns

		//centers
		Node centerW = new Node (Intersection.center);
		centerW.position.x += -0.5f;
		nodes.Add (centerW.position, centerW);

		Node centerS = new Node (Intersection.center);
		centerS.position.y += -0.5f;
		nodes.Add (centerS.position, centerS);

		Node centerE = new Node (Intersection.center);
		centerE.position.x += 0.5f;
		nodes.Add (centerE.position, centerE);

		Node centerN = new Node (Intersection.center);
		centerN.position.y += 0.5f;
		nodes.Add (centerN.position, centerN);

		prev = nodes [findNodeFrom (ReferencePoint.North, IntersectionSize.streetLength - 2)];
		node = nodes [findNodeTo (ReferencePoint.East, IntersectionSize.streetLength + 2)];
		prev.AddEdge (ReferencePoint.East, new Edge (prev, centerN, ReferencePoint.East));
		centerN.AddEdge (ReferencePoint.East, new Edge (centerN, centerE, ReferencePoint.East));
		centerE.AddEdge (ReferencePoint.East, new Edge (centerE, node, ReferencePoint.East));
		
		prev = nodes [findNodeFrom (ReferencePoint.West, IntersectionSize.streetLength - 2)];
		node = nodes [findNodeTo (ReferencePoint.North, IntersectionSize.streetLength + 2)];
		prev.AddEdge( ReferencePoint.North, new Edge(prev, centerW, ReferencePoint.North));
		centerW.AddEdge( ReferencePoint.North, new Edge(centerW, centerN, ReferencePoint.North));
		centerN.AddEdge( ReferencePoint.North, new Edge(centerN, node, ReferencePoint.North));
		
		
		prev = nodes [findNodeFrom (ReferencePoint.South, IntersectionSize.streetLength - 2)];
		node = nodes [findNodeTo (ReferencePoint.West, IntersectionSize.streetLength + 2)];
		prev.AddEdge( ReferencePoint.West, new Edge(prev, centerS, ReferencePoint.West));
		centerS.AddEdge (ReferencePoint.West, new Edge (centerS, centerW, ReferencePoint.West));
		centerW.AddEdge (ReferencePoint.West, new Edge (centerW, node, ReferencePoint.West));
		
		prev = nodes [findNodeFrom (ReferencePoint.East, IntersectionSize.streetLength - 2)];
		node = nodes [findNodeTo (ReferencePoint.South, IntersectionSize.streetLength + 2)];
		prev.AddEdge( ReferencePoint.South, new Edge(prev, centerE, ReferencePoint.South));
		centerE.AddEdge( ReferencePoint.South, new Edge(centerE, centerS, ReferencePoint.South));
		centerS.AddEdge( ReferencePoint.South, new Edge(centerS, node, ReferencePoint.South));

		//old one
//		prev = nodes [findNodeFrom (ReferencePoint.North, IntersectionSize.streetLength - 1)];
//		node = nodes [findNodeTo (ReferencePoint.East, IntersectionSize.streetLength + 1)];
////		prev.AddEdge( ReferencePoint.East, new Edge(prev, node, ReferencePoint.East));
////		prev.AddEdge (ReferencePoint.North, new Edge (prev, center, ReferencePoint.North));//U turn
//		prev.AddEdge (ReferencePoint.East, new Edge (prev, centerW, ReferencePoint.East));
////		centerW.AddEdge (ReferencePoint.East, new Edge (centerW, node, ReferencePoint.East));
//		centerW.AddEdge (ReferencePoint.East, new Edge (centerW, centerS, ReferencePoint.East));
//		centerS.AddEdge (ReferencePoint.East, new Edge (centerS, node, ReferencePoint.East));
//
//		prev = nodes [findNodeFrom (ReferencePoint.West, IntersectionSize.streetLength - 1)];
//		node = nodes [findNodeTo (ReferencePoint.North, IntersectionSize.streetLength + 1)];
//		//		prev.AddEdge( ReferencePoint.North, new Edge(prev, node, ReferencePoint.North));
//		//		prev.AddEdge (ReferencePoint.West, new Edge (prev, center, ReferencePoint.West));//U turn
//		prev.AddEdge( ReferencePoint.North, new Edge(prev, centerS, ReferencePoint.North));
////		centerS.AddEdge( ReferencePoint.North, new Edge(centerS, node, ReferencePoint.North));
//		centerS.AddEdge( ReferencePoint.North, new Edge(centerS, centerE, ReferencePoint.North));
//		centerE.AddEdge( ReferencePoint.North, new Edge(centerE, node, ReferencePoint.North));
//
//
//		prev = nodes [findNodeFrom (ReferencePoint.South, IntersectionSize.streetLength - 1)];
//		node = nodes [findNodeTo (ReferencePoint.West, IntersectionSize.streetLength + 1)];
////		prev.AddEdge( ReferencePoint.West, new Edge(prev, node, ReferencePoint.West));
////		prev.AddEdge (ReferencePoint.South, new Edge (prev, center, ReferencePoint.South));//U turn
//		prev.AddEdge( ReferencePoint.West, new Edge(prev, centerE, ReferencePoint.West));
////		centerE.AddEdge( ReferencePoint.West, new Edge(centerE, node, ReferencePoint.West));
//		centerE.AddEdge (ReferencePoint.West, new Edge (centerE, centerN, ReferencePoint.West));
//		centerN.AddEdge (ReferencePoint.West, new Edge (centerN, node, ReferencePoint.West));
//
//		prev = nodes [findNodeFrom (ReferencePoint.East, IntersectionSize.streetLength - 1)];
//		node = nodes [findNodeTo (ReferencePoint.South, IntersectionSize.streetLength + 1)];
////		prev.AddEdge( ReferencePoint.South, new Edge(prev, node, ReferencePoint.South));
////		prev.AddEdge (ReferencePoint.East, new Edge (prev, center, ReferencePoint.East));//U turn
//		prev.AddEdge( ReferencePoint.South, new Edge(prev, centerN, ReferencePoint.South));
////		centerN.AddEdge( ReferencePoint.South, new Edge(centerN, node, ReferencePoint.South));
//		centerN.AddEdge( ReferencePoint.South, new Edge(centerN, centerW, ReferencePoint.South));
//		centerW.AddEdge( ReferencePoint.South, new Edge(centerW, node, ReferencePoint.South));



#if _DEBUG_
		//graphical representation for testing:
		debugShowGraph ();
#endif

	}

	private void debugShowGraph(){
//		Debug.Log("created " + nodes.Count + " nodes"  );
		GameObject o;
		SpriteRenderer sr;
		int i = 0;
		foreach (Vector3 k in nodes.Keys) {
			o = new GameObject("node "+i);
			sr = o.AddComponent<SpriteRenderer>();
			sr.sprite = UnityEditor.AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/Background.psd");
			sr.color = Color.green;
			o.transform.position = k;
			i++;
			//			if (nodes[k].edges.Count == 0)
			//				Debug.Log("node has no edges");
			foreach(Edge e in nodes[k].edges.Values){
				if (e.from != null){
					Debug.DrawLine(e.from.position, e.to.position, Color.red, 1000f);
					//					Debug.Log("line drawn " + e.from.position + "-" + e.to.position );
				}else
					Debug.Log("from node is null");
				
			}
		}
	}

	static public Vector3 findNodeTo(ReferencePoint to, int at){ 
		Vector3 pos = new Vector3 (0, 0, 0);
		if (at < 0 || at > IntersectionSize.streetLength * 2) {
			Debug.Log("wrong position requested");
			return pos;
		}
		switch (to){
		case ReferencePoint.North:
			pos = CoordinatesTranslator.translateOrigin(ReferencePoint.South);
			pos.y += at;
			break;
		case ReferencePoint.South:
			pos = CoordinatesTranslator.translateOrigin(ReferencePoint.North);
			pos.y += -at;
			break;
		case ReferencePoint.East:
			pos = CoordinatesTranslator.translateOrigin(ReferencePoint.West);
			pos.x += at;
			break;
		case ReferencePoint.West:
			pos = CoordinatesTranslator.translateOrigin(ReferencePoint.East);
			pos.x += -at;
			break;
		}
		return pos;
	}
	static public Vector3 findNodeFrom(ReferencePoint from, int at){
		switch (from) {
		case ReferencePoint.North:
			return findNodeTo(ReferencePoint.South, at);
		case ReferencePoint.South:
			return findNodeTo(ReferencePoint.North, at);
		case ReferencePoint.East:
			return findNodeTo(ReferencePoint.West, at);
		case ReferencePoint.West:
			return findNodeTo(ReferencePoint.East, at);
		default:
			return findNodeTo(from, IntersectionSize.streetLength+3);//out of bounds
		}
	}
	static public Vector3 findNodeTo(int to, int at){
		return findNodeTo ((ReferencePoint) to, at);
	}
	static public Vector3 findNodeFrom(int from, int at){
		return findNodeFrom ((ReferencePoint)from, at);
	}
	public Node findNext(Node from, ReferencePoint o, ReferencePoint dest){
		Node ret=null;
		if (from.edges.ContainsKey (dest)) {
			ret= from.edges [dest].to;
		} else {
			//go straight
			if (from.edges.ContainsKey(ReferencePointUtil.getOpposite(o)))
				ret = from.edges[ReferencePointUtil.getOpposite(o)].to;
		}
//		Debug.Log ("next node for pos " + from.position + " going to " + dest + " is " + ret.position);
		return ret;
	}

}


