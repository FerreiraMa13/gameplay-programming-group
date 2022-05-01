using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerBehaviour : MonoBehaviour
{

    [System.NonSerialized] public bool trigger = false;
    [System.NonSerialized] public int numb_of_triggers = 0;
    [System.NonSerialized] public int numb_of_doors = 0;
    /// <summary>
    /// Makes it so that leaving the trigger will activate it again.
    /// </summary>
    public bool pulseTrigger = false;
    public bool onExitOnly = false;
    public bool oneTime = false;

    private bool onEnterTriggered = false;
    private float onEnterTimer = 0;
    public float onEnterDelay = 0;

    private bool onExitTriggered = false;
    private float onExitTimer = 0;
    public float onExitDelay = 0;

    
    /*public List<TriggerDoor> triggerDoors;*/
    /*public TriggerDoor triggerDoor = null;*/
    bool triggered = false;

    private void Update()
    {
        if(numb_of_triggers <= 0)
        {
            trigger = false;
        }

        if(onEnterTriggered)
        {
            if(onEnterTimer > 0)
            {
                onEnterTimer -= Time.deltaTime;
            }
            else 
            {
                onEnterTriggered = false;
                EnterTrigger();
            }
        }
        else if(onExitTriggered)
        {
            if(onExitTimer > 0)
            {
                onExitTimer -= Time.deltaTime;
            }
            else
            {
                onExitTriggered = false;
                ExitTrigger();
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player" && !triggered && !onExitOnly)
        {
            if(onEnterDelay > 0)
            {
                onEnterTimer = onEnterDelay;
                onEnterTriggered = true;

                if(onExitTriggered)
                {
                    onExitTriggered = false;
                }
            }
            else
            {
                EnterTrigger();
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player" && !triggered)
        {
            if(onExitDelay > 0)
            {
                onExitTimer = onExitDelay;
                onExitTriggered = true;

                if(onEnterTriggered)
                {
                    onEnterTriggered = false;
                }
            }
            else
            {
                ExitTrigger();
            }
        }
    }
    private void EnterTrigger()
    {
        trigger = true;
        numb_of_triggers = numb_of_doors;
        if (oneTime)
        {
            triggered = true;
        }
    }
    private void ExitTrigger()
    {
        if (pulseTrigger || onExitOnly)
        {
            trigger = true;
            numb_of_triggers = numb_of_doors;
        }
        else
        {
            numb_of_triggers = 0;
            trigger = false;
        }

        if(oneTime)
        {
            triggered = true;
        }
    }
}
