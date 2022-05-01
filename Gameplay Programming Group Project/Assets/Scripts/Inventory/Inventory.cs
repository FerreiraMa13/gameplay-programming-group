using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private List<Inventory_Object> inventory;
    public bool AddNewObject(string object_name, int object_quantity = 1)
    {
        int index = FindIndexOf(object_name);
        if (index >= 0)
        {
            inventory[index].object_quantity += object_quantity;
        }
        else
        {
            if (object_quantity > 0)
            {
                inventory.Add(new Inventory_Object(object_name, inventory.Count, object_quantity));
            }
            else
            {
                return false;
            }
        }
        return true;
    }
    public bool AddNewObject(Inventory_Object inv_object)
    {
        int index = FindIndexOf(inv_object.object_name);
        if (index >= 0)
        {
            return false;
        }
        else
        {
            inv_object.object_id = inventory.Count;
            inventory.Add(inv_object);
        }
        return true;
    }
    public bool RemoveObject(int index)
    {
        if(index >= 0 && index < inventory.Count)
        {
            int corrections = inventory.Count - 1 - index;
            for(int i = 1; i < corrections + 1; i++)
            {
                inventory[index + i].object_id -= 1;
            }
            inventory.RemoveAt(index);
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool AddMoreQuantity(int object_id, int object_quantity = 1)
    {
        int index = FindIndexOf(object_id);
        if (index >= 0)
        {
            inventory[index].object_quantity += object_quantity;
        }
        else
        {
            return false;
        }
        return true;
    }
    public bool AddMoreQuantity(string object_name, int object_quantity = 1)
    {
        int index = FindIndexOf(object_name);
        if (index >= 0)
        {
            inventory[index].object_quantity += object_quantity;
        }
        else
        {
            return false;
        }
        return true;
    }
    public bool SetQuantity(int object_id, int object_quantity = 1)
    {
        int index = FindIndexOf(object_id);
        if (index >= 0)
        {
            inventory[index].object_quantity = object_quantity;
        }
        else
        {
            return false;
        }
        return true;
    }
    public bool SetQuantity(string object_name, int object_quantity = 1)
    {
        int index = FindIndexOf(object_name);
        if (index >= 0)
        {
            inventory[index].object_quantity = object_quantity;
        }
        else
        {
            return false;
        }
        return true;
    }
    public bool CheckForObject(int object_id)
    {
        foreach(Inventory_Object inv_object in inventory)
        {
            if(inv_object.object_id == object_id)
            {
                return true;
            }
        }
        return false;
    }
    public bool CheckForObject(string object_name)
    {
        foreach(Inventory_Object inv_object in inventory)
        {
            if(inv_object.object_name.ToLower() == object_name.ToLower())
            {
                return true;
            }
        }
        return false;
    }
    public int FindIndexOf(int object_id)
    {
        foreach(Inventory_Object inv_object in inventory)
        {
            if(inv_object.object_id == object_id)
            {
                return inventory.IndexOf(inv_object);
            }
        }
        return -1;
    }
    public int FindIndexOf(string object_name)
    {
        foreach (Inventory_Object inv_object in inventory)
        {
            if (inv_object.object_name.ToLower() == object_name.ToLower())
            {
                return inventory.IndexOf(inv_object);
            }
        }
        return -1;
    }
    public int FindIndexOf(Inventory_Object search_object)
    {
        foreach (Inventory_Object inv_object in inventory)
        {
            if (inv_object == search_object)
            {
                return inventory.IndexOf(inv_object);
            }
        }
        return -1;
    }
}
