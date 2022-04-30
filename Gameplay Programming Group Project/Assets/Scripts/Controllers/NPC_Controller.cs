using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPC_Controller : MonoBehaviour
{
    public enum enemyState
    {
        INVALID = -1,
        IDLE = 0,
        ROAMING = 1,
        PATROLING = 2,
        DISENGAGING = 3,
        CHASING = 4
    }
    public enemyState default_state = enemyState.IDLE;
    public SplineFollower patrol_route;
    [Header("Character Stats")]
    public float character_hp = 10.0f;
    public float damage = 5.0f;
    public float attack_range = 5.0f;
    public float attack_speed = 5.0f;
    public float movement_speed = 5.0f;
    public float reaction_speed = 2.0f;
    public float character_armor = 0.0f;
    public float give_up_time = 1.0f;
    public Vector2 roaming_bounds_x = new Vector2(-5,5);
    public Vector2 roaming_bounds_z = new Vector2(-5, 5);
    

    [System.NonSerialized] public float distance_from_target;
    [System.NonSerialized] public bool line_of_sight;
    [System.NonSerialized] public bool damage_calc;
    [System.NonSerialized] public enemyState enemy_state;
    [System.NonSerialized] public bool attacking = false;
    [System.NonSerialized] public float speed_muliplier = 1.0f;
    [System.NonSerialized] public Vector3 destination;

    private float reaction_timer = 0.0f;
    private float turn_smooth_velocity;
    private float turn_smooth_time = 0.2f;
    private float give_up_timer = -0.1f;
    private bool giving_up = false;
    private Vector3 target_destination;
    private float scale = 5.0f;

    protected PlayerMovController player_controller;

    /*protected int supports = 0;
    protected float additional_decay = 0.0f;
    protected float gravity = 9.8f;
    protected float gravity_pull = 0.0f;*/

    private void Awake()
    {
        enemy_state = default_state;
        OwnAwake();
    }
    
    private void OnValidate()
    {
        patrol_route = GetComponent<SplineFollower>();
        
        if (default_state == enemyState.PATROLING && patrol_route != null)
        {
            this.transform.position = patrol_route.spline.GetPoint(0);
        }
        if(patrol_route != null)
        {
            patrol_route.timeTaken = 100 * 1 / movement_speed;
        }
    }
    private void Update()
    {
        if (character_hp <= 0)
        {
            Die();
        }

        if (player_controller != null)
        {
            distance_from_target = Vector3.Distance(transform.position, player_controller.transform.position);
            line_of_sight = !(Physics.Linecast(transform.position, player_controller.transform.position));
        }
        else
        {
            distance_from_target = -1;
            line_of_sight = false;
        }

        OwnUpdate();

        if(ConditionalMove())
        {
            if (give_up_timer > 0)
            {
                give_up_timer -= Time.deltaTime;
            }
            else if (giving_up)
            {
                giving_up = false;
                player_controller = null;
                give_up_timer = 0;
            }
        }
    }
    private void FixedUpdate()
    {
        OwnFixedUpdate();
        /*HandleGravity();*/
        if(ConditionalMove())
        {
            enemy_state = Move();
        }
    }
    public virtual void TakeDamage(float damage_taken)
    {
        
        damage_taken -= character_armor;
        if (damage_taken > 0)
        {
            character_hp -= damage_taken;
        }
        AdditionalAttackEffects();

    }
    public void Die()
    {
        Deathrattle();
        if (character_hp <= 0)
        {
            if (player_controller != null)
            {
                player_controller.RemoveNPC(gameObject.GetComponent<NPC_Controller>());
            }
            Destroy(gameObject);
        }
    }
    public bool DamageConditions()
    {
        return player_controller.hit && line_of_sight && (distance_from_target <= player_controller.attack_range + transform.localScale.x / 2 && distance_from_target > 0);
    }
    private enemyState Move()
    {
        float dt = Time.deltaTime;
        switch (enemy_state)
        {
            case (enemyState.IDLE):
                {
                    return Idle(dt);
                }
            case (enemyState.ROAMING):
                {
                    return Roam(dt);
                }
            case (enemyState.PATROLING):
                {
                    return Patrol(dt);
                }
            case (enemyState.DISENGAGING):
                {
                    return Disengage(dt);
                }
            case (enemyState.CHASING):
                {
                    return Chase(dt);
                }
        }
        return enemyState.INVALID;
    }
    private enemyState Idle(float dt)
    {
        if (player_controller != null)
        {
            return enemyState.CHASING;
        }
        return enemyState.IDLE;
    }
    private enemyState Patrol(float dt)
    {
        if (player_controller != null)
        {
            if(patrol_route != null)
            {
                patrol_route.active = false;
            }
            return enemyState.CHASING;
        }
        if (patrol_route != null && ConditionalMove())
        {
            patrol_route.active = true;
        }
        return enemyState.PATROLING;
    }
    private enemyState Roam(float dt)
    {
        if(player_controller != null)
        {
            return enemyState.CHASING;
        }
        if (destination == Vector3.zero || GetSuppressedDistance(destination, Enums.Axis.YAXIS) < scale)
        {
            Vector3 old_destination = destination;
            while(GetSuppressedDistance(destination, old_destination, Enums.Axis.YAXIS) < scale * 2)
            {
                destination = Vector3.zero;
                destination.x = Random.Range(roaming_bounds_x.x, roaming_bounds_x.y);
                destination.z = Random.Range(roaming_bounds_z.x, roaming_bounds_z.y);
                destination = transform.parent.position + destination;
            }
        }

        MoveTowards(destination);
        return enemyState.ROAMING;
    }
    private enemyState Disengage(float dt)
    {
        if (default_state == enemyState.PATROLING)
        {
            if (ConditionalMove())
            {
                Vector3 non_y_pos = new Vector3(transform.position.x, 0f, transform.position.z);
                Vector3 non_y_destination = patrol_route.spline.GetPoint(patrol_route.progress);
                non_y_destination.y = 0;
                if (Vector3.Distance(non_y_pos, non_y_destination) < scale)
                {
                    patrol_route.active = true;
                    return enemyState.PATROLING;
                }
                else
                {
                    Debug.Log(Vector3.Distance(non_y_pos, non_y_destination));
                    MoveTowards(patrol_route.spline.GetPoint(patrol_route.progress));
                }
            }
        }
        else
        {
            return default_state;
        }
        return enemyState.DISENGAGING;
    }
    private enemyState Chase(float dt)
    {
        if (player_controller != null)
        {
            if (reaction_timer <= 0)
            {
                target_destination = player_controller.transform.position;
                reaction_timer = reaction_speed;
            }
            else
            {
                reaction_timer -= dt;
            }

            MoveTowards(target_destination);
            return enemyState.CHASING;
        }
        return enemyState.DISENGAGING;
    }
    public void MoveTowards(Vector3 destination)
    {
        var offset = destination - transform.position;
        Vector3 rotate = RotateCalc(offset, destination.y);
        Vector3 movement = XZMoveCalc(rotate);
        /*movement.y += gravity_pull;
        if (!ConditionalMove())
        {
            movement.x = 0.0f;
            movement.z = 0.0f;
        }*/
        Vector3 next_pos = transform.position + movement;
        transform.position = Vector3.Lerp(transform.position, next_pos, Time.deltaTime);
    }
    private Vector3 RotateCalc(Vector3 direction, float anchor_rotation)
    {

        direction.Normalize();
        float rotateAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + anchor_rotation;
        float smoothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, rotateAngle, ref turn_smooth_velocity, turn_smooth_time);
        transform.rotation = Quaternion.Euler(0.0f, smoothAngle, 0.0f);

        return new Vector3(0.0f, rotateAngle, 0.0f);
    }
    private Vector3 XZMoveCalc(Vector3 direction)
    {
        Vector3 forward = Quaternion.Euler(direction).normalized * Vector3.forward;
        Vector3 movement = forward *  CalculateMultiplier();
        return movement;
    }

    protected float GetSuppressedDistance(Vector3 c_vector, Enums.Axis axis)
    {
        Vector3 new_pos = transform.position;
        Vector3 new_vector = c_vector;
        switch (axis)
        {
            case (Enums.Axis.XAXIS):
                {
                    new_pos.x = 0;
                    new_vector.x = 0;
                    break;
                }
            case (Enums.Axis.YAXIS):
                {
                    new_pos.y = 0;
                    new_vector.y = 0;
                    break;
                }
            case (Enums.Axis.ZAXIS):
                {
                    new_pos.z = 0;
                    new_vector.z = 0;
                    break;
                }
        }
        return Vector3.Distance(new_pos, new_vector);
    }
    protected float GetSuppressedDistance(Vector3 vector_a, Vector3 vector_b, Enums.Axis axis)
    {
        Vector3 new_pos = vector_a;
        Vector3 new_vector = vector_b;
        switch (axis)
        {
            case (Enums.Axis.XAXIS):
                {
                    new_pos.x = 0;
                    new_vector.x = 0;
                    break;
                }
            case (Enums.Axis.YAXIS):
                {
                    new_pos.y = 0;
                    new_vector.y = 0;
                    break;
                }
            case (Enums.Axis.ZAXIS):
                {
                    new_pos.z = 0;
                    new_vector.z = 0;
                    break;
                }
        }
        return Vector3.Distance(new_pos, new_vector);
    }
 
    /*private void HandleGravity()
    {
        if (supports > 0)
        {
            additional_decay = 0.0f;
            gravity_pull = - gravity * Time.deltaTime;
        }
        else
        {
            gravity_pull -= (gravity * Time.deltaTime) + additional_decay;
            additional_decay += (0.2f * Time.deltaTime);
        }
    }*/
    /*private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Ground" && transform.position.y >= collision.transform.position.y)
        {
            supports ++;
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if(collision.gameObject.tag == "Ground" && transform.position.y >= collision.transform.position.y)
        {
            supports --;
        }
    }*/

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            player_controller = other.gameObject.GetComponent<PlayerMovController>();
            player_controller.AddNPC(gameObject.GetComponent<NPC_Controller>());
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            player_controller = other.gameObject.GetComponent<PlayerMovController>();
            player_controller.RemoveNPC(gameObject.GetComponent<NPC_Controller>());
            give_up_timer = give_up_time;
            giving_up = true;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            /*if(DamageConditions())
            {
                TakeDamage(player_controller.damage);
            }*/
        }
    }

    protected virtual void OwnAwake() { }
    protected virtual void OwnUpdate() { }
    protected virtual void OwnFixedUpdate() { }
    public virtual void Deathrattle()
    {
        Debug.Log("Mob Died");
    }
    protected virtual bool ConditionalMove()
    {
        return true;
    }
    protected virtual void AdditionalAttackEffects(){ }
    protected virtual float CalculateMultiplier()
    {
        return movement_speed * speed_muliplier;
    }
}
