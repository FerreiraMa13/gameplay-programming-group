using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionBroadcaster : MonoBehaviour
{
    [System.NonSerialized]
    public PlayerControllerLite player;

    public List<InteractionReceiver> transmition_receivers;
    public bool do_once = true;
    bool done = false;
    bool valid = true;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControllerLite>();
        if(transmition_receivers == null || transmition_receivers.Count < 1)
        {
            transmition_receivers = new List<InteractionReceiver>();

            var own_receiver = GetComponent<InteractionReceiver>();
            if (own_receiver != null)
            {
                transmition_receivers.Add(own_receiver);
            }
            else
            {
                valid = false;
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            player.interact_in_range++;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            player.interact_in_range--;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            if (player.interact && valid)
            {
               if( CheckConditions())
                {
                    OnHitBehaviour();
                }
            }
        }
    }

    public virtual void OnHitBehaviour()
    {
        if(!done)
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
