using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiMessage : MonoBehaviour {
    public Text message;
    public Image background_image;
    public Canvas canvas;

    private bool is_showed;
    private bool is_showed_cache;
    private string in_str = "";
    // private string out_str = ""; // TODO: remove, use message.text !
    private float time_counter = 0.0f;
    public float fade_starting_delay = 1.0f;
    public float fade_closing_delay = 1.0f;
    public float print_delay = 0.05f; // print 1 char (time)
    public float print_space_delay = 0.05f;
    public float post_delay = 1.0f; // show full text (time) // UNUSED NOW!
    // TODO: space - show full message:w
    // TODO: check space, use other delay!

    private void Start()
    {
        reset_fade();
    }

    private void LateUpdate()
    {
        time_counter += Time.deltaTime;
        if (in_str.Length == 0)
        {
            /*if (time_counter > post_delay)
                is_showed = false;*/
        }
        else
        {
            is_showed = true;
            while (true)
            {
                if (in_str.Length == 0)
                    break;
                char c = in_str[0];
                float delay = (c == ' ' ? print_space_delay : print_delay); // We can use longer delay and other sound
                if (time_counter >= delay)
                {
                    message.text += c.ToString();
                    in_str = in_str.Substring(1);
                    time_counter -= delay;
                }
                else
                    break;
            }
        }

        if (in_str.Length == 0 && Input.GetKeyDown("space"))
        {
            is_showed = false;
        }

        if (Input.GetKeyDown("space") && in_str.Length > 0) // in_str.Length - защита от постоянного скидывания time_counter с задержкой
        {
            message.text += in_str;
            in_str = "";
            time_counter = 0;
        }

        check_fade();
    }

    void check_fade()
    {
        if (is_showed != is_showed_cache)
        {
            float fade_delay = is_showed ? fade_starting_delay : fade_closing_delay;
            float fade_value = is_showed ? 0.85f : 0.0f;

            is_showed_cache = is_showed;
            background_image.CrossFadeAlpha(fade_value, fade_delay, true);
            message.CrossFadeAlpha(fade_value, fade_delay, true);
        }
    }

    void reset_fade()
    {
        message.text = "";
        is_showed = is_showed_cache = false;
        background_image.CrossFadeAlpha(0.0f, 0.0f, true);
        message.CrossFadeAlpha(0.0f, 0.0f, true);
    }

    public void show_message(string s)
    {
        reset_fade();
        time_counter = -fade_starting_delay; // 0;
        in_str = s;
    }
}