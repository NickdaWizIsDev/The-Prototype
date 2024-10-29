using System.Collections;
using UnityEngine;
public class JumpState : State
{
    public AnimationClip anim;
    public State midAirState;

    public override void Enter()
    {
        Animator.Play(anim.name);
    }
    public override void Do()
    {
        if (time >= anim.length) { IsComplete = true; parent.Set(midAirState); }
    }
    public override void FixedDo()
    {

    }
}
