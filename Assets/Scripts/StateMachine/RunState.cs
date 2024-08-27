using UnityEngine;
using System.Collections;

public class RunState : RunStates
{
    [Header("Animation Clip")]
    public AnimationClip clip;
    public override void Enter()
    {
        Animator.Play(clip.name);
    }
    public override void Do()
    {
        Animator.speed = Helpers.Map(Mathf.Abs(Body.velocity.x), 12, 18, 1f, 2f, true);
    }

    public override void Exit() 
    {
        Animator.speed = 1f;
    }
}