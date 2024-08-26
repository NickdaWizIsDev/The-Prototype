using System.Collections;
using UnityEngine;
public class MidAirState : AirStates
{
    public AnimationClip anim;

    public override void Enter()
    {
        Animator.Play(anim.name);

    }
    public override void Do()
    {
        if (grounded) IsComplete = true;
        if (Body.velocity.y < 0) { IsComplete = true; parent.Set(fallState); }
    }
    public override void FixedDo()
    {
    
    }
}
