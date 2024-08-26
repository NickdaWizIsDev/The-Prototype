using System.Collections;
using UnityEngine;
public class JumpState : AirStates
{
    public AnimationClip anim;

    public override void Enter()
    {
        Animator.Play(anim.name);
    }
    public override void Do()
    {
        if (grounded) { IsComplete = true; }
        if (time >= anim.length) { IsComplete = true; parent.Set(midAirState); }
    }
    public override void FixedDo()
    {

    }
}
