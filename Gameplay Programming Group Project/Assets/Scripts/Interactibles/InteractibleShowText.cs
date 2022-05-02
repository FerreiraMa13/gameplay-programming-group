using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InteractibleShowText : MonoBehaviour
{
    public bool repeatable = false;
    public InteractionReceiver receiver;
    public List<TextMeshProUGUI> texts;
    public float text_duration = 0.5F;
    private float text_timer = 0;
    private int text_index = 0;
    private bool do_once = false;
    private bool active = false;
    private bool valid = true;

    private void Awake()
    {
        if(receiver == null)
        {
            receiver = GetComponent<InteractionReceiver>();
        }
        if(texts == null || texts.Count < 1)
        {
            valid = false;
        }
    }

    void Update()
    {
        if(valid)
        {
            if (active)
            {
                if (text_timer > 0)
                {
                    text_timer -= Time.deltaTime;
                }
                else
                {
                    texts[text_index].gameObject.SetActive(false);
                    if (text_index < texts.Count - 1)
                    {
                        text_index++;
                        text_timer = text_duration;
                        texts[text_index].gameObject.SetActive(true);
                    }
                    else
                    {
                        text_index = 0;
                        active = false;
                    }
                }
            }
            else if (!do_once && !active)
            {
                if (receiver.signal)
                {
                    active = true;
                    text_index = 0;
                    texts[text_index].gameObject.SetActive(true);
                    text_timer = text_duration;

                    if(!repeatable)
                    {
                        do_once = true;
                    }
                }
            }
        }
        
    }
}
