using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionalBroadcaster : MonoBehaviour
{
    public Inventory inventory;
    public List<InteractionReceiver> transmition_receivers;
    public bool do_once = true;
    bool done = false;

    void Start()
    {
        if (transmition_receivers == null || transmition_receivers.Count < 1)
        {
            transmition_receivers = new List<InteractionReceiver>();

            var own_receiver = GetComponent<InteractionReceiver>();
            if (own_receiver != null)
            {
                transmition_receivers.Add(own_receiver);
            }
        }
    }

    private void Update()
    {
        if (CheckConditions())
        {
            OnHitBehaviour();
        }
    }

    public virtual void OnHitBehaviour()
    {
        if (!done)
        {
            foreach (InteractionReceiver receiver in transmition_receivers)
            {
                receiver.Signal(true);
            }

            if (do_once)
            {
                done = true;
            }
        }

    }

    private bool CheckConditions()
    {
        var conditions = GetComponents<Condition>();
        if (conditions != null)
        {
            foreach (Condition condition in conditions)
            {
                if (!condition.CheckCondition())
                {
                    return false;
                }
            }
        }
        return true;
    }
}
