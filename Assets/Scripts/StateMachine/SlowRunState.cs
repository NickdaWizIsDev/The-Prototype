using UnityEngine;

public class SlowRunState : RunStates
{
    [Header("Animation Clip")]
    public AnimationClip clip;
    public override void Enter()
    {
        Animator.Play(clip.name);
    }
    public override void Exit() { }
}