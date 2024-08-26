using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RunState : GroundStates
{
    [Header("Parameters")]
    public float speedAcceleration = 3f;
    public float maxWalkSpeed;
    public float maxFastWalkSpeed;
    public float maxRunSpeed;
    [Range(0f, 2f)] public float drag = 0.3f;

    [Header("Anims")]
    AnimationClip walk;
    AnimationClip fastWalk;
    AnimationClip run;

    public Vector2 moveInput;

    public override void Enter()
    {
        
    }
    public override void Do()
    {
        if(!Grounded)
        {
            IsComplete = true;
            Set(fallState);
        }
        if (Mathf.Approximately(Body.velocity.x, 0f) && moveInput.x == 0)
        {
            IsComplete = true;
            Set(idleState);
        }
        else
        {
            if (Mathf.Abs(Body.velocity.x) > 0f)
                Animator.Play(walk.name);
            else if (Mathf.Abs(Body.velocity.x) > maxWalkSpeed)
                Animator.Play(fastWalk.name);
            else if (Mathf.Abs(Body.velocity.x) > maxFastWalkSpeed)
                Animator.Play(run.name);
        }
    }
    public override void FixedDo()
    {
        //Horizontal Movement//
        if (moveInput.x != 0)
        {
            if (Mathf.Abs(Body.velocity.x) <= maxRunSpeed) Body.AddForce(new(moveInput.x * speedAcceleration * 10, 0), ForceMode2D.Force);
        }
    }
    public override void Exit()
    {
        StartCoroutine(Drag()); //Ground Drag//
    }

    IEnumerator Drag()
    {
        if (moveInput.x != 0 || Body.velocity == Vector2.zero || !Grounded) yield break;
        else if (moveInput.x == 0f && Grounded && Body.velocity.x > 0)
        {
            float duration = 1f;
            var timePassed = 0f;
            float value;
            while (timePassed < duration)
            {
                // This factor moves linear from 0 to 1
                var factor = timePassed / duration;
                // This adds ease-in and ease-out 
                // see https://docs.unity3d.com/ScriptReference/Mathf.SmoothStep.html
                // Basically you can use ANY mathematical function that maps
                // the input of [0; 1] again to a range of [0;1] 
                // with the easing you like
                factor = Mathf.SmoothStep(0, 1, factor);

                // And this is how finally you use Lerp in this case
                value = Mathf.Lerp(0, drag, factor);

                // This tells Unity to "pause" the routine here
                // render this frame and continue from here in the next one
                yield return value;

                // increase by the time passed since last frame
                timePassed += Time.deltaTime;
            }
        }
    }
}