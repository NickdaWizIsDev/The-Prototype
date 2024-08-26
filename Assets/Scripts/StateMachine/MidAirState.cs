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
        if (Touching.IsGrounded) IsComplete = true;
        if (time >= anim.length) { Set(fallState); }
    }
}
