using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplineBoundTrigger : MonoBehaviour
{
    public Cinemachine.CinemachineVirtualCameraBase new_camera;
    PlayerMovController p_controller;
    GameObject player;
    public Cinemachine.CinemachineVirtualCameraBase og_camera;
    [Range(0, 1)]
    public int spline_end = 0;
    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        p_controller = player.GetComponent<PlayerMovController>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            if(!p_controller.restricted)
            {
                p_controller.restricted = true;
                og_camera.Priority = 9;
                new_camera.Priority = 10;
                player.GetComponent<SplineFollower>().active = true;
                player.GetComponent<SplineFollower>().progress = spline_end;
            }
            else
            {
                p_controller.restricted = false;
                og_camera.Priority = 10;
                new_camera.Priority = 9;
                player.GetComponent<SplineFollower>().active = false;
            }
        }
    }


}
