using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IngameMenuFns : MonoBehaviour {
    public bool is_active;
    public Canvas canvas;

	// Use this for initialization
	void Start () {
        is_active = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Escape))
        {
            is_active = !is_active;
        }

        canvas.enabled = is_active;
	}

    public void BackToGameButton ()
    {
        is_active = false;
    }

    public void ExitToMenu()
    {
		SceneManager.LoadScene(0);
    }
}
