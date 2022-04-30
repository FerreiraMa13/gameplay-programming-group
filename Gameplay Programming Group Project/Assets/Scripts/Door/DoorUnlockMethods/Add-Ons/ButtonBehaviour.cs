using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonBehaviour : HitObject
{
    [System.NonSerialized]
    public bool pressed = false;
    public bool independent = false;
    public ButtonDoor buttonDoor = null;
    public Cinematic_Behaviour cinematic;
    bool once_buttonLogic = false;
    bool once_punched = false;
    bool cinematic_door_ending = false;

    private void Update()
    {
        if(cinematic != null)
        {
            if(cinematic.current_state == Cinematic_Behaviour.SequenceState.STAY_B )
            {
                if(!once_buttonLogic)
                {
                    once_buttonLogic = true;
                    ButtonLogic();
                }
                if(cinematic_door_ending)
                {
                    once_punched = false;
                    cinematic.SignalSTATE(Cinematic_Behaviour.SequenceState.BA_TRANSITION);
                    cinematic_door_ending = false;
                }
            }
        }
    }
    private void Awake()
    {
        if(!independent)
        {
            buttonDoor = GetComponentInParent<ButtonDoor>();
        }
    }
    public void gotPressed()
    {
        player.interact = false;
        if (cinematic != null && !cinematic.triggered)
        {
            if(!once_punched)
            {
                once_punched = true;
                cinematic.SignalSTARTING();
            }
           /* if(!cinematic.triggered)
            {
                cinematic.SignalSTARTING();
            }
            else
            {
                ButtonLogic();
                Debug.LogError("Second Punch");
            }*/
        }
        else
        {
            ButtonLogic();
            Debug.LogError("No Punch");
        }
    }
    public override void OnHitBehaviour()
    {
        gotPressed();
    }
    private void ButtonLogic()
    {
        pressed = !pressed;
        Debug.Log(pressed);
    }
    public void DoorFinishedMovement()
    {
        cinematic_door_ending = true;
    }
}
