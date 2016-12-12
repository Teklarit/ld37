using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonHelps : MonoBehaviour {
    public Text first_text;
    public Text second_text;

    private int font_change = 2;
    private float alpha_off = 0.2f;

    public void point_enter()
    {
        var cmp_text = gameObject.GetComponent<Text>();
        cmp_text.fontSize += font_change;
        if (cmp_text == first_text)
            second_text.GetComponent<CanvasRenderer>().SetAlpha(alpha_off);
        else
            first_text.GetComponent<CanvasRenderer>().SetAlpha(alpha_off);
    }

    public void point_exit()
    {
        var cmp_text = gameObject.GetComponent<Text>();
        cmp_text.fontSize -= font_change;
        if (cmp_text == first_text)
            second_text.GetComponent<CanvasRenderer>().SetAlpha(1.0f);
        else
            first_text.GetComponent<CanvasRenderer>().SetAlpha(1.0f);

    }
}
