using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlobAnimator : MonoBehaviour {
	
	// Use this for initialization
	void Start () {
	}
	private float time;
	// Update is called once per frame
	void Update ()
	{
		time += Time.deltaTime;
		GetComponent<MeshRenderer>().material.SetTextureOffset("_DispTex",new Vector2(Mathf.Sin(time) + 0.5f,Mathf.Sin(time) + 0.5f));
	}
}
