using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory_Condition : Condition
{
    private bool valid = false;
    public bool active = true;
    public List<Inventory> checkInventories;
    public List<string> requiredItemNames;
    public List<int> requiredItemQuantities;

    private void Awake()
    {
        if(requiredItemNames == null ||
            requiredItemQuantities == null)
        {
            valid = false;
        }
        else if(requiredItemNames.Count == requiredItemQuantities.Count)
        {
            valid = true;
        }
    }
    protected override bool CheckRequirments()
    {
        if (valid && active)
        {
            foreach(Inventory inventory in checkInventories)
            {
                bool inventory_check = true;
                for(int i = 0; i < requiredItemQuantities.Count; i++)
                {
                    if(!inventory.CheckForObject(requiredItemNames[i], requiredItemQuantities[i]))
                    {
                        inventory_check = false;
                        break;
                    }
                }
                if(!inventory_check)
                {
                    return false;
                }
            }
        }
        return true;
    }
}
