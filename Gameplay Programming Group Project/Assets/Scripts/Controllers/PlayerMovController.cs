using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerMovController : MonoBehaviour
{
    enum MOVEMENT_TYPE
    {
        FREE = 0,
        TOWARDS = 1
    }

    AnimationStateController animation_controller;
    PlayerCameraController camera_controller;
    CharacterController controller;
    GameplayPlayerController controls;

    public GameObject SpeedBoostParticles;
    public GameObject DoubleJumpParticles;
    public GameObject ParticleSpawnPoint;
    GameObject target_particles;
    bool particles_deployed = false;
    
    [System.NonSerialized]
    public Vector2 move;
    public float speed = 10.0f;
    [System.NonSerialized]
    public float speed_multiplier = 1.0f;
    public float speed_boost = 1.0f;
    public float deadzone = 0.1F;

    public float damage = 2.5f;
    public float attack_range = 2.5f;

    public Transform cam_transform;
    public float turn_smooth_time = 0.1f;
    private float turn_smooth_velocity;

    public bool jumping = false;
    public bool landing = false;
    public float jump_force = 10.0f;
    private float jump_velocity = 0.0f;
    public float gravity = 9.8f;
    private float additional_decay = 0.0f;

    [System.NonSerialized]
    public int number_jumps = 1;
    public int jump_attempts = 0;

    [System.NonSerialized]
    public bool hit = false;
    [System.NonSerialized]
    public bool interact = false;

    [System.NonSerialized]
    public bool attacking = false;
    public float stun_lock = 0.0f;
    bool attacked = false;
    public bool falling = false;

    [System.NonSerialized]
    public int interact_in_range = 0;
    public float camera_sensitivity = 1.0f;

    Pick_Up.PowerUpEffects status_effect = Pick_Up.PowerUpEffects.NULL;
    float effect_timer = 0.0f;

    [System.NonSerialized]
    public bool restricted = false;
    public Vector3 respawn_point = Vector3.zero;

    Vector3 towards = Vector3.zero;
    public List<NPC_Controller> npc_contollers;

    public float hp = 10.0f;
    [System.NonSerialized] public float current_hp = 10.0f;

    private void Awake()
    {
        camera_controller = GetComponent<PlayerCameraController>();
        controls = new GameplayPlayerController();
        controller = GetComponent<CharacterController>();
        animation_controller = GetComponentInChildren<AnimationStateController>();
        controls.Player.Move.performed += ctx => move = ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled += ctx => move = Vector2.zero;
        controls.Player.Jump.started += ctx => Jump();
        controls.Player.Attack.started += ctx => AttackInteract();
        controls.Player.LockOn.started += ctx => AttemptLock();

        current_hp = hp;
    }
    private void OnEnable()
    {
        controls.Player.Enable();
    }
    public void EnableInput()
    {
        controls.Player.Enable();
    }
    private void OnDisable()
    {
        controls.Player.Disable();
    }
    public void DisableInput()
    {
        controls.Player.Disable();
    }
    void SendMessage(Vector2 coordinates)
    {
        Debug.Log("Thumb-stick coordinates = " + coordinates);
    }
    void SendMessage()
    {
        Debug.Log("Input Detected");
    }
    void FixedUpdate()
    {
        HandleEffects();
        HandleAttack();

        if (!restricted)
        {
            HandleMovement();
        }
        else
        {
            MoveTowards(towards);
        }

        HandleJump();
    }
    private void Update()
    {
        if(current_hp <= 0)
        {
            Reset();
        }
        if(attacking)
        {
            if (stun_lock < 0)
            {
                attacking = false;
                attacked = false;
                stun_lock = 0;
            }
            else
            {
                stun_lock -= Time.deltaTime;
            }
        }
        HandleParticles();
        HandleAnimations();
    }
    private void HandleMovement()
    {

        Vector3 input_direction = new Vector3(move.x, 0.0f, move.y);
        Vector3 rotate = RotateCalc(input_direction, cam_transform.eulerAngles.y);
        Vector3 movement = XZMoveCalc(rotate, input_direction);

        movement.y = jump_velocity;

        controller.Move(movement * Time.deltaTime);
    }
    private void HandleAnimations()
    {
        if (!jumping)
        {
            Vector3 input_direction = new(move.x, 0.0f, move.y);
            if(camera_controller.lock_on || restricted)
            {
                input_direction = 0.5f * input_direction;
            }
            animation_controller.updateMovement(input_direction.magnitude);
        }
        if (status_effect == Pick_Up.PowerUpEffects.SPEED_BOOST)
        {
            animation_controller.speedUP(3);
        }
        else
        {
            animation_controller.speedUP(1);
        }
    }
    private void HandleJump()
    {
        if (controller.isGrounded)
        {
            if (jumping && jump_velocity <= -0.5f)
            {
                jumping = false;
                animation_controller.triggerLand();
                additional_decay = 0.0f;
                jump_attempts = 0;
            }
            else if (falling)
            {
                falling = false;
                animation_controller.triggerLand();
                additional_decay = 0.0f;
            }
            jump_velocity = -gravity * Time.deltaTime;
        }
        else
        {
            if (!falling && !jumping)
            {
                falling = true;
                animation_controller.triggerFall();
            }
            jump_velocity -= (gravity * Time.deltaTime) + additional_decay;
            additional_decay += (0.2f * speed * Time.deltaTime / speed_boost);
        }

    }
    private void HandleParticles()
    {
        if(effect_timer > 0)
        {
            if (!particles_deployed)
            {
                switch (status_effect)
                {
                    case (Pick_Up.PowerUpEffects.SPEED_BOOST):
                        {
                            target_particles = SpeedBoostParticles;
                            break;
                        }
                    case (Pick_Up.PowerUpEffects.DOUBLE_JUMP):
                        {
                            target_particles = DoubleJumpParticles;
                            break;
                        }
                }
                target_particles.SetActive(true);
                particles_deployed = true;
            }

        }
        else
        {
            if(target_particles != null && particles_deployed)
            {
                particles_deployed = false;
                target_particles.SetActive(false);
            }
        }
    }
    private void HandleAttack()
    {
        if(!jumping && !falling)
        {
            if (attacking && !attacked)
            {
                if(interact_in_range > 0)
                {
                    animation_controller.triggerInteract();
                    attacked = true;
                }
                else
                {
                    animation_controller.triggerAttack();
                    attacked = true;
                }
            }
        }
    }
    private void HandleEffects()
    {
        if(status_effect != Pick_Up.PowerUpEffects.NULL)
        {
            Debug.Log("Effect being handled");
            if(effect_timer < 0)
            {
                speed_boost = 1.0f;
                number_jumps = 1;
                status_effect = Pick_Up.PowerUpEffects.NULL;
                effect_timer = 0;
                Debug.Log("Effect dissipated");
            }
            else
            {
                effect_timer -= Time.deltaTime;
            }
        }

        switch (status_effect)
        {
            case (Pick_Up.PowerUpEffects.SPEED_BOOST):
                {
                    speed_boost = 2;
                    break;
                }
            case (Pick_Up.PowerUpEffects.DOUBLE_JUMP):
                {
                    number_jumps = 2;
                    break;
                }
        }
    }
    private void Jump()
    {
        if(jump_attempts < number_jumps && !landing)
        {
            animation_controller.triggerJump();
            jumping = true;
            jump_velocity = jump_force;
            additional_decay = 0.0f;

            jump_attempts += 1;
        }

        if(camera_controller.lock_on)
        {
            camera_controller.LockOn();
        }
    }
    private void AttackInteract()
    {
        if(/*!jumping &&*/ !attacked /*&& !falling && !landing*/)
        {
            attacking = true;
            stun_lock = 2.0f;
        }
    }
    public void TakeDamage(float damage_taken)
    {
        if(current_hp > 0)
        {
            current_hp -= damage_taken;
        }
    }
    private bool Compare2Deadzone( float value)
    {
        if (value < deadzone)
        {
            if(value > - deadzone)
            {
                return false;
            }
        }
        return true;
    }
    public void endInteraction()
    {
        attacking = false;
        attacked = false;
        hit = false;
        interact = false;
        /*Debug.Log("END");*/
    }
    public void detectHit()
    {
        if(interact_in_range > 0){ interact = true; }
        else 
        { 
            if(!hit)
            {
                foreach (NPC_Controller npc in npc_contollers)
                {
                    npc.TakeDamage(damage);
                }
                hit = true;
            }
        }
    }
    public void detectLand(bool status)
    {
        landing = status;
    }
    public bool affectPlayer(Pick_Up.PowerUpEffects effect, float duration)
    {
        effect_timer = duration;
        if (status_effect == Pick_Up.PowerUpEffects.NULL)
        {
            status_effect = effect;
            effect_timer = duration;
            return true;
        }
        else
        {
            return false;
        }
    }
    private void AttemptLock()
    {
        if(!jumping && !landing && !attacking)
        {
            camera_controller.LockOn();
        }
    }
    public bool ApproachPoint(Vector3 destination)
    {
        towards = destination;
        return (Vector3.Distance(transform.position, destination) < speed * 0.9f);
    }
    public void MoveTowards(Vector3 destination)
    {
        var offset = destination - transform.position;
        Vector3 input_direction = new Vector3(move.x, 0.0f, move.y);
        Vector3 rotate = RotateCalc(offset, destination.y);
        Vector3 movement = XZMoveCalc(rotate, input_direction);

        movement.y = jump_velocity;

        controller.Move(movement * Time.deltaTime);
    }
    private Vector3 RotateCalc(Vector3 input_direction, float anchor_rotation)
    {
        
        input_direction.Normalize();
        float rotateAngle = Mathf.Atan2(input_direction.x, input_direction.z) * Mathf.Rad2Deg + anchor_rotation;
        float smoothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, rotateAngle, ref turn_smooth_velocity, turn_smooth_time);
        transform.rotation = Quaternion.Euler(0.0f, smoothAngle, 0.0f);

        return new Vector3(0.0f, rotateAngle, 0.0f);
    }
    private Vector3 XZMoveCalc(Vector3 direction, Vector3 input)
    {

        speed_multiplier = 0.5f + 2 * input.magnitude;
        if (camera_controller.lock_on)
        {
            speed_multiplier = speed_multiplier * 0.8f;
        }
        Vector3 forward = Quaternion.Euler(direction).normalized * Vector3.forward;
        Vector3 movement = forward * speed * speed_multiplier * speed_boost;
        if ((!Compare2Deadzone(move.x) && !Compare2Deadzone(move.y)) || landing)
        {
            movement = Vector3.zero;
        }
        return movement;
    }
    public void Reset()
    {
        additional_decay = 0;
        jump_velocity = 0;
        current_hp = hp;
        controller.enabled = false;
        transform.position = respawn_point;
        controller.enabled = true;
    }
    public void AddNPC( NPC_Controller npc)
    {
        int npc_index = FindNPC(npc);
        if(npc_index < 0)
        {
            npc_contollers.Add(npc);
        }
    }
    public void RemoveNPC(NPC_Controller npc)
    {
        int npc_index = FindNPC(npc);
        if (npc_index >= 0)
        {
            npc_contollers.Remove(npc);
        }
    }
    private int FindNPC(NPC_Controller npc)
    {
        for(int i = 0; i < npc_contollers.Count; i++)
        {
            if(npc_contollers[i] == npc)
            {
                return i;
            }
        }

        return -1;
    }

}
