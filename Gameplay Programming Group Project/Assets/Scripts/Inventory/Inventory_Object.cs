using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory_Object
{
    [System.NonSerialized] public string object_name;
    [System.NonSerialized] public int object_id;
    [System.NonSerialized] public int object_quantity;

    public Inventory_Object(string new_name, int new_id, int new_quantity = 1)
    {
        object_name = new_name;
        object_id = new_id;
        object_quantity = new_quantity;
    }
    public static bool operator ==(Inventory_Object a, Inventory_Object b)
    {
        return (a.object_name == b.object_name && a.object_id == b.object_id);
    }
    public static bool operator !=(Inventory_Object a, Inventory_Object b)
    {
        return (a.object_name != b.object_name || a.object_id == b.object_id);
    }
    public override bool Equals(object obj)
    {
        return base.Equals(obj);
    }
    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
    public override string ToString()
    {
        return base.ToString();
    }
}
