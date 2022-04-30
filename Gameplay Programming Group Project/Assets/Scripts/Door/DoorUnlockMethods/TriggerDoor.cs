using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDoor : DoorOpeningMethod
{
    public bool independent = false;
    public TriggerBehaviour trigger= null;
    public bool trigger_once = true;

    private void Awake()
    {
        method = DoorUnlockingMethod.TRIGGER;
        if (!independent)
        {
            trigger = GetComponentInChildren<TriggerBehaviour>();
            
        }
        trigger.numb_of_doors++;
    }

    public override bool UnlockRequirements()
    {
        bool hasTriggered = trigger.trigger;
        if(hasTriggered && !(animator.status == DoorAnimator.DoorStatus.OPENING || animator.status == DoorAnimator.DoorStatus.CLOSING))
        {
            /*trigger.trigger = false;*/
            if(trigger_once)
            {
                trigger.numb_of_triggers--;
                trigger_once = false;
            }
        }
        else if(!hasTriggered)
        {
            if(!trigger_once)
            {
                trigger_once = true;
            }
        }
        return hasTriggered;
    }
}
