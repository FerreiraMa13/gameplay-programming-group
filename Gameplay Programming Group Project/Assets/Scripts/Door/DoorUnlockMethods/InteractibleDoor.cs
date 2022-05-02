using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractibleDoor : DoorOpeningMethod
{
    public InteractionReceiver receiver = null;
    public bool trigger_once = true;

    private void Awake()
    {
        method = DoorUnlockingMethod.INTERACTIBLE;
        if(receiver == null) 
        {
            receiver = GetComponent<InteractionReceiver>();
        }
    }

    public override bool UnlockRequirements()
    {
        bool hasTriggered = receiver.signal;
        if (hasTriggered && !(animator.status == DoorAnimator.DoorStatus.OPENING || animator.status == DoorAnimator.DoorStatus.CLOSING))
        {
            /*trigger.trigger = false;*/
            if (trigger_once)
            {
                trigger_once = false;
            }
        }
        else if (!hasTriggered)
        {
            if (!trigger_once)
            {
                trigger_once = true;
            }
        }
        return hasTriggered;
    }
}
