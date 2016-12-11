using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Node")]
public class Node : ScriptableObject {
	[Multiline] public string key;
	public AudioClip clip;
	public Transform originalObj;
	public Transform clearObj;
	public bool isSecretAgent;
	public Node[] associations;
}
