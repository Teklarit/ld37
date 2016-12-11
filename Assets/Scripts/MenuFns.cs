using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuFns : MonoBehaviour {

	public void LoadScene(int level)
    {
        Application.LoadLevel(level);
    }

    public void Exit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void Credits()
    {
        
    }
}
