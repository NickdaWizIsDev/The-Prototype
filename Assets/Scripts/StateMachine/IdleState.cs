using UnityEngine;

public class IdleState : GroundStates
{
    public override void Enter()
    {
        core.animator.SetBool(AnimationStrings.isMoving, false);
    }

    public override void Do()
    {
        if (MoveInput != Vector2.zero && core.Grounded) 
        { 
            IsComplete = true;
            parent.Set(runStates);
        }
    }

    public override void Exit() {  }
}