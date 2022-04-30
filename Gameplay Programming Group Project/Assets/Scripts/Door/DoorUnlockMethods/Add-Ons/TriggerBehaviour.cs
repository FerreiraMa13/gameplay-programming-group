using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerBehaviour : MonoBehaviour
{

    [System.NonSerialized]
    public bool trigger = false;
    
    public int numb_of_triggers = 0;
    
    public int numb_of_doors = 0;
    public bool independent = false;
    public bool oneTime = false;
    /// <summary>
    /// Makes it so that leaving the trigger will activate it again.
    /// </summary>
    public bool pulseTrigger = false;
    /*public List<TriggerDoor> triggerDoors;*/
    /*public TriggerDoor triggerDoor = null;*/
    bool triggered = false;
    private void Awake()
    {
        if (!independent)
        {

        }
    }
    private void Update()
    {
        if(numb_of_triggers <= 0)
        {
            trigger = false;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player" && !triggered)
        {
            trigger = true;
            numb_of_triggers = numb_of_doors;
            if(oneTime)
            {
                triggered = true;
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player" && !triggered)
        {
            if(pulseTrigger)
            {
                trigger = true;
                numb_of_triggers = numb_of_doors;
            }
            else
            {
                numb_of_triggers = 0;
                trigger = false;
            }
        }
    }
}
