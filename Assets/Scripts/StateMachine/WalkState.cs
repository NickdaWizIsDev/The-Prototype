using UnityEngine;

public class WalkState : GroundStates
{
    public override void Enter()
    {
        core.animator.SetBool(AnimationStrings.isMoving, true);
    }

    public override void Do()
    {
        if (MoveInput == Vector2.zero)
        {
            IsComplete = true;
        }
    }

    public override void Exit() { }
}