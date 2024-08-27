using System.Collections;
using UnityEngine;
public class FallState : AirStates
{
    public AnimationClip anim;

    public override void Enter()
    {
        Animator.Play(anim.name);

    }
    public override void Do()
    {        
        if (grounded) { IsComplete = true; core.machine.Set(core.groundStates); }
    }
    public override void FixedDo()
    {

    }
    public override void Exit()
    {

    }
}
