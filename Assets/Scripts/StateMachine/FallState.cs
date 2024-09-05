using System.Collections;
using UnityEngine;
public class FallState : AirStates
{
    public AnimationClip anim;

    public override void Enter()
    {
        Animator.SetBool("idle", false);
        Animator.Play(anim.name);
    }
    public override void Do()
    {        
        if (grounded) { IsComplete = true; parent.Set(core.groundStates); }
    }
    public override void FixedDo()
    {

    }
    public override void Exit()
    {
    }
}
