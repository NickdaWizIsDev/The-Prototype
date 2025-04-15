using System.Collections;
using UnityEngine;
public class MidAirState : State
{
    public AnimationClip anim;
    public State fallState;

    public override void Enter()
    {
        Animator.Play(anim.name);
    }
    public override void Do()
    {
        if (Body.linearVelocity.y < 0) { IsComplete = true; parent.Set(fallState); }
    }
    public override void FixedDo()
    {
    
    }
}
