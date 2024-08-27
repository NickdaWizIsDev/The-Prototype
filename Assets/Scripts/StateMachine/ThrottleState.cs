using UnityEngine;
using System.Collections;

public class ThrottleState : RunStates
{
    [Header("Animation Clip")]
    public AnimationClip clip;
    public override void Enter()
    {
        Animator.Play(clip.name);
    }
    public override void Exit() { }
}