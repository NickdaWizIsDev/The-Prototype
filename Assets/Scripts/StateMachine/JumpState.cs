using System.Collections;
using UnityEngine;
public class JumpState : AirStates
{
    public AnimationClip anim;

    public override void Enter()
    {
        Animator.Play(anim.name);
        Body.AddForce(new(0, jumpImpulse), ForceMode2D.Impulse);
    }
    public override void Do()
    {
        if (Touching.IsGrounded) IsComplete = true;

        if(time >= anim.length) { Set(midAirState); }
    }
}
