using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldPlayer : MonoBehaviour
{
    GameObject player = null;
    private void OnTriggerEnter(Collider other)
    {   
        if(other.gameObject.tag == "Player")
        {
            Debug.Log("Entered");
            player = other.gameObject;
            player.transform.SetParent(transform);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        
        if (player != null)
        {
            Debug.Log("Exit");
            transform.DetachChildren();
            player = null;
        }
    }
    public bool isHoldingPlayer()
    {
        return (player != null);
    }
}
