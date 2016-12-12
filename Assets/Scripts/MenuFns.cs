using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuFns : MonoBehaviour {

	public void LoadScene(int level)
    {
		SceneManager.LoadScene(level);
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
