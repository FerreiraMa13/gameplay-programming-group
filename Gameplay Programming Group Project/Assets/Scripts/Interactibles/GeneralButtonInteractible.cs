using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralButtonInteractible : HitObject
{
    [System.NonSerialized]
    public bool pressed = false;
    public InteractionReceiver[] interactibles;

    public override void OnHitBehaviour()
    {
        gotPressed();
    }
    public void gotPressed()
    {
        player.interact = false;
        ButtonLogic();
    }
    private void ButtonLogic()
    {
        pressed = !pressed;
        foreach(InteractionReceiver receiver in interactibles)
        {
            receiver.Signal(pressed);
        }
    }
}

