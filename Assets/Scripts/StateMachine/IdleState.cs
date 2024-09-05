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
        Animator.SetBool("idle", true);
        if (MoveInput != Vector2.zero && core.Grounded) 
        { 
            IsComplete = true;
            parent.Set(runStates); Animator.SetBool("idle", false);
        }
    }

    public override void Exit() {  }
}