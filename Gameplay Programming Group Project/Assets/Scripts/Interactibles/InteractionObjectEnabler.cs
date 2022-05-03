using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionObjectEnabler : MonoBehaviour
{

    public InteractionReceiver receiver;

    public bool do_once = true;
    public bool enable = true;
    private bool done = false;
    private bool valid = true;

    public List<GameObject> gameObjectsToEnable;

    private void Awake()
    {
        if(receiver == null)
        {
            receiver = GetComponent<InteractionReceiver>();
        }
        if(gameObjectsToEnable == null || gameObjectsToEnable.Count < 1)
        {
            valid = false;
        }

    }

    private void Update()
    {
        if(valid)
        {
            if(!done)
            {
                if(receiver.signal)
                {
                    foreach(GameObject gameObject in gameObjectsToEnable)
                    {
                        gameObject.SetActive(enable);
                    }
                }
            }
        }
    }
}
