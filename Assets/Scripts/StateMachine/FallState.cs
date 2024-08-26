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
        if (Touching.IsGrounded) IsComplete = true;
    }
    public override void Exit()
    {

    }
}
