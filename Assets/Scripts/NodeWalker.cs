using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NoiseBall;

public class NodeWalker : MonoBehaviour {
	private Stack<Node> history;
	private int secretAgent = 0;
	private Node currentNode;
	private Node prevNode;

	public Node startNode;
	public UiMessage UIMessage;
	public LayerMask interactibleLayer;

	private void Unreveal(Node node) {
		if (!node.originalObj) return;
		var blob = node.originalObj.FindChild("blob");
		blob.gameObject.SetActive(true);
		blob.GetComponent<NoiseBallRendererFixed>()._surfaceColor = Color.red;
		node.originalObj.GetComponent<BoxCollider>().enabled = false;
		node.clearObj.gameObject.SetActive(false);
	}

	private void Reveal(Node node) {
		node.originalObj.FindChild("blob").gameObject.SetActive(false);
		node.clearObj.gameObject.SetActive(true);
		node.originalObj.GetComponent<BoxCollider>().enabled = true;
	}

	private void Associate(Node node) {
		if (!node.originalObj) return;
		var blob = node.originalObj.FindChild("blob");
		blob.gameObject.SetActive(true);
		blob.GetComponent<NoiseBallRendererFixed>()._surfaceColor = Color.green;
		node.originalObj.GetComponent<BoxCollider>().enabled = true;
		node.clearObj.gameObject.SetActive(false);
	}

	private void GoNode(Node node) {
		if (!node.visited) {
			node.visited = true;
			if (node.isSecretAgent) secretAgent += 1;
		}
		// Hide node previous to the one we currently have, not the one we're going to
		if (prevNode) {
			Unreveal(prevNode);
		}
		// Hide all the associations for the current node that we've not used
		if (currentNode) {
			for (var i = 0; i < currentNode.associations.Length; i++) {
				Unreveal(currentNode.associations[i]);
			}
		}
		// Reveal the association that we used
		Reveal(node);
		for (var i = 0; i < node.associations.Length; i++) {
			Associate(node.associations[i]);
		}
		// Store the previous node in a history stack
		if (prevNode) history.Push(prevNode);
		prevNode = currentNode; // Set the current one as previous
		currentNode = node; // Set the new one as current

		// Do node operations (text, sound, etc)
		UIMessage.show_message(Texts.texts[node.gameObject.name]);
	}

	private void Return() {
		// Pretend we moved to a previous node
		var newCurrent = prevNode;
		prevNode = null;
		if (history.Count > 0) {
			Debug.Log(history.Peek());
			currentNode = history.Pop();
			Reveal(currentNode);
		}
		GoNode(newCurrent);
	}

	void Start () {
		history = new Stack<Node>();
		//GoNode(startNode);
	}
	
	void Update () {
		if (Input.GetButtonDown("Fire1")) {
			RaycastHit hit;
			if (Physics.Raycast(transform.position, transform.forward, out hit, 8.0f, interactibleLayer)) {
				if (hit.transform == currentNode.originalObj) return; // Don't run anything if we hit the current node
				// Find the matching association and go to that node
				for (var i = 0; i < currentNode.associations.Length; i++) {
					var association = currentNode.associations[i];
					if (!association.originalObj) continue;
					if (hit.transform == association.originalObj) { // Matching association is the one having transform we just hit
						GoNode(association);
						return;
					}
				}
				Return();
			}
		}		
	}
}
