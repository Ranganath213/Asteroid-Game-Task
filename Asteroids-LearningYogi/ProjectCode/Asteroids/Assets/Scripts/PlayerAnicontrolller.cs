using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnicontrolller : MonoBehaviour
{
    public Animator animator;

    public void AnimatorFalse()
    {
        animator.enabled = false;
    }
    public void AnimatorTrue()
    {
        animator.enabled = true;
      
        GameManager.instance.SwitchStartText(true);
        GameManager.instance.isStarted = false;
    }
}
