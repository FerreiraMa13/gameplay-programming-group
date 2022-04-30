using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorAnimator : MonoBehaviour
{
    public enum DoorOpeningAnimation
    {
        NONE = 0,
        SLIDING = 1,
        ROTATING = 2,
        VANISHING = 3
    }
    public enum DoorStatus
    {
        INVALID = 0,
        CLOSED = 1,
        OPENING = 2,
        CLOSING = 3,
        OPEN = 4
    }

    [System.NonSerialized]
    public DoorStatus status = DoorStatus.CLOSED;
    [System.NonSerialized]
    public DoorOpeningAnimation type = DoorOpeningAnimation.NONE;
    [System.NonSerialized]
    public bool stationary = false;

    private void Update()
    {
        if(type != DoorOpeningAnimation.NONE)
        {
            switch (status)
            {
                case DoorStatus.OPENING:
                    {
                        OpeningMovement();
                        break;
                    }
                case DoorStatus.CLOSING:
                    {
                        ClosingMovement();
                        break;
                    }
            }

            if((status == DoorStatus.CLOSED || status == DoorStatus.OPEN))
            {
                stationary = true;
            }
            else
            {
                stationary = false;
            }
        }
    }
    public void Open()
    {
        if(status == DoorStatus.CLOSED)
        {
            status = DoorStatus.OPENING;
        }
    }
    public void Close()
    {
        if(status == DoorStatus.OPEN)
        {
            status = DoorStatus.CLOSING;
        }
    }
    public virtual void OpeningMovement()
    {

    }
    public virtual void ClosingMovement()
    {

    }
}
