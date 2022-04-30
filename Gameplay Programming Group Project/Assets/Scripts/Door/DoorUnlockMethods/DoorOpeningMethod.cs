using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpeningMethod : MonoBehaviour
{
    public enum DoorUnlockingMethod
    {
        NONE = 0,
        BUTTON = 1,
        KEY = 2,
        TRIGGER = 3
    }

    [System.NonSerialized]
    public DoorUnlockingMethod method = DoorUnlockingMethod.NONE;
    [System.NonSerialized]
    public DoorAnimator animator;
    bool locked = true;

    public void Start()
    {
        if (method != DoorUnlockingMethod.NONE)
        {
            animator = GetComponent<DoorAnimator>();
        }
    }
    virtual public bool UnlockRequirements()
    {
        return false;
    }

    public bool Unlock()
    {
        if(locked)
        {
            locked = false;
            animator.Open();
            return true;
        }
        return false;
        //Animator stuff here
        
    }

    public bool Lock()
    {
        if (!locked)
        {
            locked = true;
            animator.Close();
            return true;
        }
        return false;
    }

    void Update()
    {
        if(UnlockRequirements())
        {
            Unlock();
        }
        else
        {
            Lock();
        }

        AdditionalUpdate();
    }

    public virtual void AdditionalUpdate()
    {

    }
}
