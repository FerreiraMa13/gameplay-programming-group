using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionReceiver : MonoBehaviour
{
    public bool active = true;
    [System.NonSerialized]
    public bool signal = false;

    public void Signal(bool value)
    {
        signal = value;
    }
}
