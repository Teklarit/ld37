using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeWalker : MonoBehaviour {
	private Stack<Node> history;
	private int secretAgent = 0;
	public LayerMask interactibleLayerMask;

	private void GoNode(Node node) {
		if (!node.visited) {
			node.visited = true;
			if (node.isSecretAgent) secretAgent += 1;
		}
		history.Push(node);
		// reveal node
	}

	private void Return() {
		var node = history.Pop();
		// reveal node etc etc
	}

	// Use this for initialization
	void Start () {
		history = new Stack<Node>();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown("Fire")) {
			RaycastHit hit;
			if (Physics.Raycast(transform.position, transform.forward, out hit, 8.0f, interactibleLayerMask))
			{
				Collider hit_collider = hit.collider;
				print("Get collider, game_object: " + hit_collider.gameObject);
				print("Found an object - distance: " + hit.distance);
			}
		}		
	}
}
