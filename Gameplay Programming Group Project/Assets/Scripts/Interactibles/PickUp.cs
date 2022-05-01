using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class PickUp : MonoBehaviour
{
    private float dist;
    private GameObject player;
    private GameObject jump_power;

    private Transform player_transform;
    private GameObject power_up;

    private PlayerControllerLite player_script;

    //[SerializeField] private GameObject text_canvas;

    private float float_height = 1f;
    private float bounce_damp = 0.01f;
    [SerializeField] private float float_level = 0.5f;

    private float force;
    private Vector3 action_point;
    private Vector3 up_lift;
    [SerializeField] private Vector3 buoyancy_offset;

    public bool delete = false;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        player_transform = player.GetComponent<Transform>();
        player_script = player.GetComponent<PlayerControllerLite>();

        power_up = this.gameObject;

    }

    // Update is called once per frame
    void Update()
    {

        if (player_transform && power_up.transform)
        {
            /*if (power_up.tag != "Weapon")
            {
                power_up.transform.Rotate(2, 2, 2);

                action_point = power_up.transform.position + power_up.transform.TransformDirection(buoyancy_offset);
                force = 1f - ((action_point.y - float_level) / float_height);
                if (force > 0f)
                {
                    up_lift = -Physics.gravity * (force - power_up.GetComponent<Rigidbody>().velocity.y * bounce_damp);
                    power_up.GetComponent<Rigidbody>().AddForceAtPosition(up_lift, action_point);
                }
            }*/

            dist = Vector3.Distance(power_up.transform.position, player_transform.position);
            if (dist < 2.5)
            {
                if (power_up.tag == "DoubleJump")
                {
                    player_script.jumpPower();
                    Destroy(power_up, 0f);
                }
                else if (power_up.tag == "SprintBuff")
                {
                    player_script.sprint_power = true;
                    player_script.speed_boost_timer = 10f;
                    Destroy(power_up, 0f);
                }
                else if (power_up.tag == "HP")
                {
                    player_script.healthUp();
                    Destroy(power_up, 0f);
                }
                else if (power_up.tag == "Weapon")
                {
                    //text_canvas.SetActive(true);
                }
            }
            else if (power_up.tag == "Weapon" && dist >= 2)
            {
                //text_canvas.SetActive(false);

            }

        }
        if (delete)
        {
            deleteWeapon();
        }
    }
    private void FixedUpdate()
    {
        pickUpRotation();
    }
    private void pickUpRotation()
    {
        if (player_transform && power_up.transform && power_up.tag != "Weapon")
        {
            power_up.transform.Rotate(2, 2, 2);

            action_point = power_up.transform.position + power_up.transform.TransformDirection(buoyancy_offset);
            force = 1f - ((action_point.y - float_level) / float_height);
            if (force > 0f)
            {
                up_lift = -Physics.gravity * (force - power_up.GetComponent<Rigidbody>().velocity.y * bounce_damp);
                power_up.GetComponent<Rigidbody>().AddForceAtPosition(up_lift, action_point);
            }
        }
    }
    private void deleteWeapon()
    {
        delete = false;

        /*if (text_canvas.activeSelf)
        {
            text_canvas.SetActive(false);
            Destroy(power_up, 0f);  
        }*/
    }
  
}
