using System;
using UnityEngine;
using System.Collections.Generic;

public class Graph{


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

	public Dictionary<Vector3, Node> nodes;

	public Graph(){
		int i, j, direction = 1;
		Vector3 pos;
		nodes = new Dictionary<Vector3, Node> ();
		Node node, prev;

		for (j = 0; j < 4; j++){
			direction = -direction; //N=0,E=2 -1, going "down", S=1,W=3 1, going "up"
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
					node.AddEdge( j-direction, new Edge(prev, node, j-direction) );
				if (j < 2)//N,S
					pos.y = pos.y + direction;
				else //E,W
					pos.x = pos.x + direction;
				
			}
		}

		Debug.Log("created " + nodes.Count + " nodes"  );

		//graphical representation for testing:
		GameObject o;
		SpriteRenderer sr;
		i = 0;
		foreach (Vector3 k in nodes.Keys) {
			o = new GameObject("node "+(i+1));
			sr = o.AddComponent<SpriteRenderer>();
			sr.sprite = UnityEditor.AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/Background.psd");
			sr.color = Color.green;
			o.transform.position = k;
			i++;
			if (nodes[k].edges.Count == 0)
				Debug.Log("node has no edges");
			foreach(Edge e in nodes[k].edges.Values){
				if (e.from != null){
					Debug.DrawLine(e.from.position, e.to.position, Color.red, 1000f);
					Debug.Log("line drawn " + e.from.position + "-" + e.to.position );
				}else
					Debug.Log("from node is null");

			}
		}
	}
		

}


