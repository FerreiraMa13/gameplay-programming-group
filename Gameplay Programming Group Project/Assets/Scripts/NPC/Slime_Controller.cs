using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime_Controller : NPC_Controller
{
    public float jump_cooldown = 20.0f;
    public float knockback = 7.0f;
    public float stun_received = 0.25f;
    public float size_multiplier = 3.0f;
    public GameObject slime_prefab;

    [System.NonSerialized] public bool landed = true;
    [System.NonSerialized] public int lives = 2;

    private float jump_timer = 0.0f;
    private bool being_knocked = false;
    private float stun_timer = 0.0f;
    public bool standard_stats = false;
    private bool just_spawned = true;
    public float default_hp; 
    private bool once = true;

    SlimeAnimatorController animator;
    Rigidbody rb;

    protected override void OwnAwake()
    {
        animator = GetComponentInChildren<SlimeAnimatorController>();
        rb = GetComponent<Rigidbody>();
        character_hp = default_hp;
        if(standard_stats)
        {
            ApplyMultiplier(size_multiplier);
        }
    }
    protected override void OwnUpdate()
    {
        if (patrol_route != null && enemy_state == enemyState.PATROLING)
        {
            patrol_route.active = !landed;
        }

        if(stun_timer >= 0)
        {
            stun_timer -= Time.deltaTime;
        }
        else
        {
            stun_timer = 0;
        }

        if(player_controller != null)
        {
            speed_muliplier = 2.0f;
        }
        else
        {
            speed_muliplier = 1.0f;
        }
        
        if (landed)
        {
            if(just_spawned)
            {
                just_spawned = false;
            }
            if (jump_timer < 0)
            {
                if (!attacking && enemy_state != enemyState.IDLE)
                {
                    animator.triggerJump();
                    landed = false;
                    jump_timer = 0;
                }
            }
            else
            {
                jump_timer -= Time.deltaTime;
            }
        }
    }
    public override void Deathrattle()
    {
        if(once)
        {
            if (size_multiplier > 1 && lives > 0)
            {
                Debug.Log("Dead");
                SpawnSlime(lives);
                SpawnSlime(lives);
            }
            once = false;
        }
    }
    public void SpawnSlime(float size)
    {
        Vector3 random_pos_mod = new Vector3(Random.Range(-5.0f, 5.0f), Random.Range(0.0f, 5.0f), Random.Range(-5.0f, 5.0f));
        GameObject slime = Instantiate(slime_prefab, transform.position + random_pos_mod, transform.rotation);
        slime.transform.parent = transform.parent;
        slime.transform.localScale = slime.transform.localScale / (3 + 1 - size);
        var slime_controller = slime.GetComponent<Slime_Controller>();
        slime_controller.character_hp = default_hp;
        slime_controller.lives = lives - 1;
        slime_controller.jump_timer = Random.Range(0.0f, 0.9f);
       slime_controller.ApplyMultiplier(size);
        slime_controller.default_state = enemyState.ROAMING;
        slime_controller.player_controller = player_controller;
        slime_controller.KnockBack();
    }
    public void ApplyMultiplier(float multiplier)
    {
        character_hp = default_hp * multiplier;
        Debug.Log(character_hp);
        size_multiplier = multiplier;
        damage = 2 * multiplier;
        standard_stats = false;
    }
    protected override bool ConditionalMove()
    {
        return (!(landed || being_knocked || stun_timer > 0));
    }
    public override void TakeDamage(float damage_taken)
    {
        if(!just_spawned)
        {
            base.TakeDamage(damage_taken);
        }
    }
    protected override void AdditionalAttackEffects()
    {
        stun_timer = stun_received;
        KnockBack();
    }
    protected override float CalculateMultiplier()
    {
        return base.CalculateMultiplier() * size_multiplier;
    }
    public void ResetJump()
    {
        landed = true;
        being_knocked = false;
        jump_timer = jump_cooldown;
    }
    private void KnockBack()
    {
        if(player_controller != null)
        {
            Vector3 knockback_vector = transform.position - player_controller.transform.position;
            knockback_vector.y = 1;
            rb.AddForce(knockback_vector.normalized * knockback / size_multiplier, ForceMode.Impulse);
            being_knocked = true;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (ConditionalMove())
            {
                player_controller.TakeDamage(damage);

                KnockBack();
            }
        }
    }
}
