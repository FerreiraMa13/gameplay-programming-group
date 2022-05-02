using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryInteractible : MonoBehaviour
{
    public InteractionReceiver receiver;
    public List<Inventory> inventories;
    public bool do_once;
    public bool despawn;

    public List<string> givenItemsNames;
    public List<int> givenItemsQuantity;

    private bool done = false;
    private bool valid = true;

    private void Awake()
    {
        if (receiver == null)
        {
            receiver = GetComponent<InteractionReceiver>();
        }
        if (inventories == null || inventories.Count < 1)
        {
            inventories = new List<Inventory>();
            var player = GameObject.FindGameObjectWithTag("Player");
            var player_inv = player.GetComponent<Inventory>();

            if (player_inv == null)
            {
                valid = false;
            }
            else
            {
                inventories.Add(player_inv);
            }
        }

        if (givenItemsNames == null ||
            givenItemsQuantity == null ||
            givenItemsNames.Count != givenItemsQuantity.Count ||
            receiver == null)
        {
            valid = false;
        }
    }

    private void Update()
    {
        if (valid && receiver.signal)
        {
            if (!done)
            {
                foreach (Inventory inventory in inventories)
                {
                    for (int i = 0; i < givenItemsQuantity.Count; i++)
                    {
                        inventory.AddNewObject(givenItemsNames[i], givenItemsQuantity[i]);
                    }
                }

                if(do_once)
                {
                    done = true;
                }

                if(despawn)
                {
                    var caster = GetComponent<InteractionBroadcaster>();
                    if(caster != null)
                    {
                        caster.player.interact_in_range--;
                    }
                    gameObject.SetActive(false);
                }
            }
        }
    }
}
