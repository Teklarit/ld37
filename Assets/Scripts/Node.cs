using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour {
	public AudioClip voiceClip;
	public AudioClip objectClip;
	public Transform originalObj;
	public Transform clearObj;
	public bool isSecretAgent;
	public bool visited;
	public Node[] associations;
}
