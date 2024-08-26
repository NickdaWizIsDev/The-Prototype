using UnityEngine;

public class IdleState : GroundStates
{
    public AnimationClip anim;
    public override void Enter()
    {
        Animator.Play(anim.name);
    }

    public override void Do()
    {
        if (MoveInput != Vector2.zero) 
        { 
            IsComplete = true;
            parent.Set(runState);
        }
    }
}