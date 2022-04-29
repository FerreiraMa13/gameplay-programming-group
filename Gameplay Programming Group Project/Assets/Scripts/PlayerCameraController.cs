using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraController : MonoBehaviour
{
    public List<Cinemachine.CinemachineVirtualCameraBase> cameras;
    [System.NonSerialized] public Cinemachine.CinemachineTargetGroup target_group;
    /*[System.NonSerialized] public PlayerMovController controller;*/
    [System.NonSerialized] public PlayerControllerLite controller;
    [System.NonSerialized] public int camera_index = 0;
    [System.NonSerialized] public bool lock_on = false;
    public float camera_sensitivity = 1.0f;
    public Vector2 lockon_range = new Vector2(1.0f, 25.0f);
    GameObject playerLookAt;
    LockOnTarget lockOnTarget;
    public List<VisualHelpMeasurement> visualMeasurements;

    private void Awake()
    {
        /*target_group = GameObject.FindGameObjectWithTag("CameraTargetGroup").GetComponent<Cinemachine.CinemachineTargetGroup>();*/
        playerLookAt = GameObject.FindGameObjectWithTag("LookAt Point");
        if(playerLookAt == null)
        {
            playerLookAt = GameObject.FindGameObjectWithTag("Player");
        }
    }
    private void Update()
    {
        UpkeepLockOn();
    }
    public void UpdateCamera(int new_index)
    {
        camera_index = new_index;
        if (cameras.Count >= camera_index)
        {
            controller.cam_transform = cameras[camera_index].transform;
        }
    }
    public void EngageCamera()
    {
        if (cameras.Count >= camera_index)
        {
            if(SearchForTarget())
            {
                lock_on = true;
                cameras[camera_index].Priority = 1;
                camera_index = cameras.Count - 1;
                cameras[camera_index].Priority = 10;
                target_group.AddMember(lockOnTarget.transform, 1.5f, 4);
                cameras[camera_index].LookAt = target_group.gameObject.transform;
                /*cameras[camera_index].LookAt = lockOnTarget.transform;*/
            }
        }
    }
    private void DisengageCamera()
    {
        if (cameras.Count >= camera_index)
        {
            lock_on = false;
            cameras[camera_index].Priority = 1;
            /*cameras[camera_index].LookAt = playerLookAt.transform;*/
            target_group.RemoveMember(lockOnTarget.gameObject.transform);
            camera_index = 0;
            cameras[camera_index].Priority = 10;
            
        }
    }
    private bool SearchForTarget()
    {
        var possibleTargets = FindObjectsOfType<LockOnTarget>();
        float minDistance = lockon_range.y;

        foreach(var target in possibleTargets)
        {
            float temp_distance = Vector3.Distance(transform.position, target.transform.position);
            float height_difference = target.transform.position.y - transform.position.y;
            if (temp_distance < minDistance && height_difference < 4  && LinecastForTarget(target.transform))
            {
                minDistance = temp_distance;
                lockOnTarget = target;
            }
        }

        if(minDistance < lockon_range.y)
        {
            return LinecastForTarget();
        }
        else
        {
            return false;
        }
    }
    private bool LinecastForTarget()
    {
        if(Physics.Linecast(transform.position, lockOnTarget.transform.position))
        {
            return false;
        }
        return true;
    }
    private bool LinecastForTarget(Transform target)
    {
        if (Physics.Linecast(transform.position, target.position))
        {
            return false;
        }
        return true;
    }
    private void UpkeepLockOn()
    {
        if(lock_on)
        {
            float temp_distance = Vector3.Distance(transform.position, lockOnTarget.transform.position);
            if (temp_distance > lockon_range.y || temp_distance < lockon_range.x|| !LinecastForTarget())
            {
                DisengageCamera();
            }
        }
    }
    public bool LockOn()
    {
        Debug.Log("LockOn Input");
        if (lock_on)
        {
            DisengageCamera();
            return false;
        }
        else
        {
            EngageCamera();
            return true;
        }
    }

    private void OnValidate()
    {
        if(visualMeasurements.Count > 0)
        {
            visualMeasurements[0].distance = -lockon_range.x;
            visualMeasurements[0].UpdatePosition();
            if (visualMeasurements.Count > 1)
            {
                visualMeasurements[1].distance = -lockon_range.y;
                visualMeasurements[1].UpdatePosition();
            }
        }
    }
}
