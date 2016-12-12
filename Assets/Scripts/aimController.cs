using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class aimController : MonoBehaviour {
    public bool is_enabled = true;
    public bool is_ineractable = false;

    private float z_rot = 0.0f;

	// Use this for initialization
	void Start () {
        // is_ineractive = true;
	}
	
	// Update is called once per frame
	void Update () {
        if (is_ineractable)
            gameObject.GetComponent<Image>().color = Color.black;
        else
            gameObject.GetComponent<Image>().color = Color.white;
    }
}
