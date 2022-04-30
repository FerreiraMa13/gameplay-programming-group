using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonDoor : DoorOpeningMethod
{
    public bool independent = false;
    public ButtonBehaviour button = null;

    private void Awake()
    {
        method = DoorUnlockingMethod.BUTTON;
        if (!independent)
        {
            button = GetComponentInChildren<ButtonBehaviour>();
        }
    }

    public override bool UnlockRequirements()
    {
        return button.pressed;
    }
    public override void AdditionalUpdate()
    {
        if(animator.stationary)
        {
            button.DoorFinishedMovement();
        }
    }
}
