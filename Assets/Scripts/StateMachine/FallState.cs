using System.Collections;
using UnityEngine;
public class FallState : State
{
    public AnimationClip anim;    

    public override void Enter()
    {
        Animator.SetBool("idle", false);
        Animator.Play(anim.name);
    }
    public override void Do()
    {        
    }
    public override void FixedDo()
    {

    }
    public override void Exit()
    {
    }
}
