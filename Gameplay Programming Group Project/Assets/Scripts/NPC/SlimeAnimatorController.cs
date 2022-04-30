using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeAnimatorController : MonoBehaviour
{
    Animator animator;
    Slime_Controller controller;

    int jumpHash;
    int knockHash;

    private void Start()
    {
        controller = GetComponentInParent<Slime_Controller>();
        animator = GetComponent<Animator>();
        jumpHash = Animator.StringToHash("jump");
        knockHash = Animator.StringToHash("knock");
    }
    public void triggerJump()
    {
        animator.SetTrigger(jumpHash);
    }
    public void triggerKnock()
    {
        animator.SetTrigger(knockHash);
    }
    public void listenLand()
    {
        controller.ResetJump();
    }
    
}
