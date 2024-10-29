using UnityEngine;
using System.Collections;

public class ThrottleState : GroundStates
{
    [Header("Animation Clip")]
    public AnimationClip clip;
    public override void Enter()
    {
        Animator.Play(clip.name);
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