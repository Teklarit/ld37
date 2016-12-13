using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NoiseBall;

public class NodeWalker : MonoBehaviour {
	private Stack<Node> history;
	private int secretAgent = 0;
	private Node currentNode;
	private Node prevNode;

    public AudioClip music;
    public float music_volume = 0.2f;
    public AudioSource audio1;
    public AudioSource audio2;
    public AudioSource audio3;
	public GameCutscene cutscene;

    public Node startNode;
	public UiMessage UIMessage;
	public LayerMask interactibleLayer;
	[ColorUsage(false, true, 0, 8, 0.125f, 3)] public Color hidden;
	[ColorUsage(false, true, 0, 8, 0.125f, 3)] public Color revealed;


	private void Unreveal(Node node) {
		if (!node.originalObj) return;
		var blob = node.originalObj.FindChild("blob");
		blob.gameObject.SetActive(true);
		blob.GetComponent<NoiseBallRendererFixed>()._surfaceColor = hidden;
		node.originalObj.GetComponent<BoxCollider>().enabled = false;
		node.clearObj.gameObject.SetActive(false);
		node.state = Node.State.HIDDEN;
	}

	private void Reveal(Node node) {
		node.originalObj.FindChild("blob").gameObject.SetActive(false);
		node.clearObj.gameObject.SetActive(true);
		node.originalObj.GetComponent<BoxCollider>().enabled = true;
		node.state = Node.State.REVEALED;
	}

	private void Associate(Node node) {
		if (!node.originalObj) return;
		var blob = node.originalObj.FindChild("blob");
		blob.gameObject.SetActive(true);
		blob.GetComponent<NoiseBallRendererFixed>()._surfaceColor = revealed;
		node.originalObj.GetComponent<BoxCollider>().enabled = true;
		node.clearObj.gameObject.SetActive(false);
		node.state = Node.State.ASSOCIATED;
	}

	private void GoNode(Node node) {
		if (node.name == "burglars" && secretAgent < 7)
		{
			node.originalObj.gameObject.SetActive(false);
			cutscene.TriggerLoseSequence();
			UIMessage.gameObject.SetActive(false);
			return;
		}
		if (node.name == "burglars" && currentNode.name == "belts")
		{
			node.originalObj.gameObject.SetActive(false);
			cutscene.TriggerWinSequence();
			UIMessage.gameObject.SetActive(false);
			return;
		}
		if (node.name == "passport-reveal")
		{
			Reveal(GameObject.Find("table-painterly").GetComponent<Node>());
		}
		if (node.name == "pictures-reveal")
		{
			Unreveal(GameObject.Find("table-painterly").GetComponent<Node>());
			Reveal(GameObject.Find("table").GetComponent<Node>());
		}
		Debug.Log("---------------------------------------------");
		Debug.Log("NEW NODE: " + node);
		Debug.Log("CURRENT NODE: " + currentNode);
		Debug.Log("PREV NODE:" + prevNode);
		if (!node.visited) {
			node.visited = true;
			if (node.isSecretAgent) secretAgent += 1;
		}
		if (node.name == "poster-painting") {
			var painterly = GameObject.Find("table-painterly").GetComponent<Node>();
			var writer = GameObject.Find("table").GetComponent<Node>();
			Unreveal(writer);
			Associate(painterly);
		}
		// Hide node previous to the one we currently have, not the one we're going to
		if (prevNode) {

			// Can't hide table my stuff is on
			if (prevNode.name != "table-painterly" && prevNode.name != "table") {
				Unreveal(prevNode);
			} else
				prevNode.originalObj.GetComponent<BoxCollider>().enabled = false;

			// Hid the table first opportunity we get moving away from it
			//var zebraToProPhoto = prevNode.name == "other-pictures" && currentNode.name == "photo-zebra";
			//var awayFromPainterly = (prevNode.name == "other-pictures" && (currentNode.name == "big_box-terrarium" || currentNode.name == "photo-zebra"));
			//var awayFromWriter = (prevNode.name == "scratchpad" && currentNode.name == "camera");
			//if (zebraToProPhoto) {
				//Unreveal(GameObject.Find("table-painterly").GetComponent<Node>());
			//}
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
			if (node.associations[i] != currentNode)
				Associate(node.associations[i]);
		}
		// Store the previous node in a history stack
		if (prevNode) history.Push(prevNode);
		prevNode = currentNode; // Set the current one as previous
		currentNode = node; // Set the new one as current

		// Do node operations (text, sound, etc)
		UIMessage.show_message(Texts.texts[node.gameObject.name]);
        audio3.Stop();
        audio2.Stop();
        AudioClip obj_sound = node.objectClip;
        AudioClip voice_sound = node.voiceClip;
        if (obj_sound != null && voice_sound != null)
        {
            audio3.PlayOneShot(obj_sound, 1.0f);
            audio2.clip = voice_sound;
            audio2.PlayDelayed(1.0f);//obj_sound.length);
        }
        else if (voice_sound != null)
        {
            audio2.PlayOneShot(voice_sound, 1.0f);
        }
	}

	private void Return() {
		// Pretend we moved to a previous node
		Debug.Log("RETURNING");
		for (var i = 0; i < currentNode.associations.Length; i++) {
			Unreveal(currentNode.associations[i]);
		}
		for (var i = 0; i < prevNode.associations.Length; i++) {
			Debug.Log("UNREVEALING " + prevNode.associations[i]);
			Unreveal(prevNode.associations[i]);
		}
		var newCurrent = prevNode;
		prevNode = null;
		if (history.Count > 0) {
			Debug.Log("REVEALING: " + history.Peek());
			currentNode = history.Pop();
			Reveal(currentNode);
		} else {
			currentNode = null;
		}
		GoNode(newCurrent);
	}

	void Start () {
		history = new Stack<Node>();
		//GameObject.Find("burglars:origin").SetActive(true);
		GoNode(startNode);

        audio1.PlayOneShot(music, music_volume);
    }
	
	void Update () {
        if (audio3.isPlaying)
            audio2.volume = audio3.volume / 2.0f;
        else
            audio2.volume = 1.0f;

        if (audio2.isPlaying)
            audio1.volume = audio2.volume / 2.0f;
        else
            audio1.volume = 1.0f;

		if (Input.GetButtonDown("Fire1")) {
			RaycastHit hit;
			if (Physics.Raycast(transform.position, transform.forward, out hit, 8.0f, interactibleLayer)) {
				if (hit.transform == currentNode.originalObj) return; // Don't run anything if we hit the current node
				if (prevNode && hit.transform == prevNode.originalObj) {
					Return();
					return;
				}
				// Find the matching association and go to that node
				for (var i = 0; i < currentNode.associations.Length; i++) {
					var association = currentNode.associations[i];
					if (!association.originalObj) continue;
					if (hit.transform == association.originalObj) { // Matching association is the one having transform we just hit
						GoNode(association);
						return;
					}
				}
			}
		}		
	}
}
