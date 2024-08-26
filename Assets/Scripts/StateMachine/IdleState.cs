using UnityEngine;

public class IdleState : GroundStates
{
    public AnimationClip anim;
    public override void Enter()
    {
        Animator.Play(anim.name);
    }
}