using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractibleDestruction : MonoBehaviour
{
    public InteractionReceiver receiver;

    private void Awake()
    {
        if(receiver == null)
        {
            receiver = GetComponent<InteractionReceiver>();
        }
    }

    private void Update()
    {
        if(receiver.signal)
        {
            Destroy(gameObject);
        }
    }
}
