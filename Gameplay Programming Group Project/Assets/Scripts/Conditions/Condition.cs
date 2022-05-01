using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Condition : MonoBehaviour
{
    public bool ongoing = false;
    private bool was_met = false;

    public bool CheckCondition()
    {
        if(ongoing)
        {
            return CheckRequirments();
        }
        else
        {
            if(!was_met)
            {
                was_met = CheckRequirments();
            }

            return was_met;
        }
    }
    virtual protected bool CheckRequirments()
    {
        return false;
    }
}
