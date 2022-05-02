using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomInteractReceiver : MonoBehaviour
{
    public bool active = true;
    [System.NonSerialized]
    public bool signal = false;

    public void Signal(bool value)
    {
        signal = value;
    }
}
