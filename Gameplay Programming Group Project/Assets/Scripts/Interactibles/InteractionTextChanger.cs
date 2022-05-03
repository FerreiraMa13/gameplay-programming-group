using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InteractionTextChanger : MonoBehaviour
{
    public InteractionReceiver receiver;

    private bool valid;
    public bool do_once;
    private bool done;

    public CustomTriggerText text_trigger;
    public TextMeshProUGUI textToChange;

    private void Awake()
    {
        if(receiver == null)
        {
            receiver = GetComponent<InteractionReceiver>();
        }
        if(textToChange == null)
        {
            valid = false;
        }
    }
    private void Update()
    {
        if(valid && !done)
        {
            textToChange.enabled = false;
            text_trigger.DisableAll();

            if(do_once)
            {
                done = true;
            }
        }
    }
}
