using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonHelps : MonoBehaviour {
    public void point_enter()
    {
        gameObject.GetComponent<Text>().fontSize += 4;
        gameObject.GetComponent<CanvasRenderer>().SetAlpha(0.5f);
    }

    public void point_exit()
    {
        gameObject.GetComponent<Text>().fontSize -= 4;
        gameObject.GetComponent<CanvasRenderer>().SetAlpha(1.0f);
    }
}
