using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplineFollower : MonoBehaviour
{
    public enum SplineFollowerMode
    { 
        NONE = 0,
        AUTO = 1,
        INDEPENDENT = 2,
        SYNCHRONISED = 3
    }
    public BezierSpline spline;
    public SplineFollowerMode movementMode = SplineFollowerMode.NONE;
    bool valid = false;
    public bool active = true;
    [System.NonSerialized]
    PlayerMovController player;
    [Range(0,1)]
    public float starting_progress = 0.0f;
    [System.NonSerialized]
    public float progress = 0.0f;
    public bool requiresPlayer = false;

    [Header("Auto Movement")]
    public float timeTaken = 10.0f;
    public bool adjustRotation = false;
    public bool backAndForth = true;
    
    [System.NonSerialized]
    public Vector3 movementVector = Vector3.zero;
    public float turn_smooth_time = 0.1f;
    private float turn_smooth_velocity;
    float moveDirection = 1.0f;
    float timeStep;
    public Enums.Axis[] ignoreAxis;

    [Header("Synchonised Movement")]
    public SplineFollower coordenator;
    [System.NonSerialized]
    public float synched_progress = 0.0f;
    private Vector3 base_rotation;

    InteractionReceiver receiver;
    HoldPlayer holdPlayer;
    bool running = false;

    private void OnValidate()
    {
        timeStep = 1 / timeTaken;
        valid = (spline != null);
        if(player != null)
        {
            player.restricted = active;
        }
        if(adjustRotation)
        {
            base_rotation = transform.rotation.eulerAngles;
        }
        if (spline != null && !running && active)
        {
            transform.position = spline.GetPoint(starting_progress);
            progress = starting_progress;
        }
    }
    private void Awake()
    {
        player = gameObject.GetComponent<PlayerMovController>();
        receiver = GetComponent<InteractionReceiver>();
        holdPlayer = GetComponent<HoldPlayer>();
        running = true;
    }
    private void Update()
    {
        valid = isValid();
    }
    private void FixedUpdate()
    {
        if (valid && active)
        {

                StandardMove();
                FixedMove();
        }
    }
    private void StandardMove()
    {
        if (valid)
        {
            if(active)
            {
                switch (movementMode)
                {

                    case (SplineFollowerMode.INDEPENDENT):
                        {
                            if (active)
                            {
                                IndependentMove();
                            }
                            break;
                        }
                    case (SplineFollowerMode.AUTO):
                        {
                            AutoMove();
                            break;
                        }
                    case (SplineFollowerMode.SYNCHRONISED):
                        {
                            SynchMove();
                            break;
                        }
                }
            }
        }
    }
    private void FixedMove()
    {
        if (valid)
        {
            if (active)
            {
                /*switch (movementMode)
                {
                   case (SplineFollowerMode.AUTO):
                        {
                            AutoMove();
                            break;
                        }
                    case (SplineFollowerMode.SYNCHRONISED):
                        {
                            SynchMove();
                            break;
                        }
                }*/
            }
        }
    }
    private void AutoMove()
    {
        progress += moveDirection * timeStep * Time.deltaTime;
        if(backAndForth)
        {
            if (progress > 1)
            {
                progress = 1;
                moveDirection = moveDirection * -1.0f;
            }
            else if (progress < 0)
            {
                progress = 0;
                moveDirection = moveDirection * -1.0f;
            }
        }
        else
        {
            if(spline.Loop)
            {
                if (progress >= 1)
                {
                    progress -= 1 ;
                }
            }
            
        }
        
        if (adjustRotation)
        {
            RotateCalc(spline.GetDirection(progress), spline.GetPoint(progress).y);
           /* transform.rotation = Quaternion.Euler(Vector3.Lerp(transform.rotation.eulerAngles, RotateCalc(spline.GetPoint(progress), transform.position.y), Time.deltaTime));*/
        }

        transform.position = SanitizeAxis(spline.GetPoint(progress));
    }
    private void SynchMove()
    {
        synched_progress = coordenator.progress;
        progress = synched_progress;

        if (progress > 1) { progress = 1; }else if(progress < 0) { progress = 0; }

        Vector3 lerp_pos = Vector3.Slerp(transform.position, spline.GetPoint(progress), 2f);
        transform.position = lerp_pos;
    }
    private void IndependentMove()
    {
        if(player != null)
        {
            movementVector = new Vector3(player.move.x, 0.0f, player.move.y);
        }
        if(movementVector.x > 0)
        {
            moveDirection = 1.0f;
        }
        else if(movementVector.x < 0)
        {
            moveDirection = -1.0f;
        }

        if (player.ApproachPoint(spline.GetPoint(progress)))
        {
            progress += movementVector.magnitude  * moveDirection * player.speed * timeStep * Time.deltaTime;
            if (progress > 1)
            {
                progress = 1;
            }
            if (progress < 0)
            {
                progress = 0;
            }
        }
    }
    private bool ValidateAxis(Enums.Axis axis)
    {
        foreach(Enums.Axis enum_axis in ignoreAxis)
        {
            if(enum_axis == axis)
            {
                return false;
            }
        }
        return true;
    }
    private Vector3 SanitizeAxis( Vector3 direction)
    {
        Vector3 new_direction = transform.position;
        if(ValidateAxis(Enums.Axis.XAXIS))
        {
            new_direction.x = direction.x;
        }
        if(ValidateAxis(Enums.Axis.YAXIS))
        {
            new_direction.y = direction.y;
        }
        if(ValidateAxis(Enums.Axis.ZAXIS))
        {
            new_direction.z = direction.z;
        }
        return new_direction;
    }
    private Vector3 RotateCalc(Vector3 input_direction, float anchor_rotation)
    {

        input_direction.Normalize();
        float rotateAngle = Mathf.Atan2(input_direction.x, input_direction.z) * Mathf.Rad2Deg + anchor_rotation;
        float smoothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, rotateAngle, ref turn_smooth_velocity, turn_smooth_time);
        transform.rotation = Quaternion.Euler(0.0f, smoothAngle, 0.0f);

        return new Vector3(0.0f, rotateAngle, 0.0f);
    }
    private bool isValid()
    {
        if(receiver != null)
        {
            if(!receiver.active || !receiver.signal)
            {
                return false;
            }
        }
        if(requiresPlayer)
        {
            if(!holdPlayer.isHoldingPlayer())
            {
                return false;
            }
        }
        return true;
    }
}
