using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pick_Up : MonoBehaviour
{
    public enum PowerUpType
    {
        NULL = 0,
        ONCE = 1,
        SPAWN = 2,
        ETERNAL = 3,
        LIMITED = 4
    }
    public enum PowerUpEffects
    {
        NULL = 0,
        DOUBLE_JUMP = 1,
        SPEED_BOOST = 2
    }

    public bool active = true;
    public float duration = 5.0f;
    public GameObject particles;
    public PowerUpEffects powerUpEffect = PowerUpEffects.NULL;

    [System.NonSerialized] public bool particles_used = false;
    [System.NonSerialized] public PowerUpType type = PowerUpType.ONCE;
    [System.NonSerialized] public GameObject player;

    private void OnTriggerEnter(Collider other)
    {
        if(other .CompareTag("Player"))
        {
            player = other.gameObject;
            PickUp();
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.gameObject;
            PickUp();
        }
    }
    public void PickUp()
    {
        if (active)
        {
            if (particles != null && !particles_used)
            {
                Instantiate(particles, transform.position, transform.rotation);
                particles_used = true;
            }
            if (type != PowerUpType.ETERNAL)
            {
                active = false;
                gameObject.GetComponent<MeshRenderer>().enabled = false;
            }
            else
            {
                particles_used = false;
            }
            ActivateEffect();
        }
    }
    public void ActivateEffect()
    {
        player.GetComponent<PlayerMovController>().affectPlayer(powerUpEffect, duration);
    }
}
