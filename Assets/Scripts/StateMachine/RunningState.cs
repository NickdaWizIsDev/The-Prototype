using UnityEngine;
using System.Collections;

public class RunningState : GroundStates
{
    public override void Enter()
    {
        core.animator.SetBool(AnimationStrings.isMoving, true);
    }
    public override void Do()
    {
        Animator.speed = Helpers.Map(Mathf.Abs(Body.velocity.x), core.groundStates.maxThrottleSpeed, core.groundStates.maxRunSpeed, 1f, 2f, true);
        if (MoveInput == Vector2.zero)
        {
            IsComplete = true;
        }
    }

    public override void Exit() 
    {
        Animator.speed = 1f;
    }
}