using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RunStates : GroundStates
{
    [Header("Parameters")]
    public float speedAcceleration = 3f;
    public float maxWalkSpeed;
    public float maxFastWalkSpeed;
    public float maxRunSpeed;
    public Vector2 moveInput;

    [Header("Substates")]
    public WalkState walk;
    public SlowRunState slowRun;
    public RunState run;
    public override void Enter()
    {
        if (Mathf.Abs(Body.velocity.x) != 0f)
        {
            if (Mathf.Abs(Body.velocity.x) < maxWalkSpeed)
            {
                Set(walk);
            }
            else if (Mathf.Abs(Body.velocity.x) > maxWalkSpeed)
            {
                Set(slowRun);
            }
        }
    }
    public override void Do()
    {
        moveInput = MoveInput;
        if (Mathf.Abs(Body.velocity.x) < 0.1f && MoveInput.x == 0)
        {
            parent.Set(idleState);
            IsComplete = true;
        }
        else if (Mathf.Abs(Body.velocity.x) != 0f)
        {
            if (Mathf.Abs(Body.velocity.x) < maxWalkSpeed)
            {
                Set(walk);
            }                
            else if (Mathf.Abs(Body.velocity.x) > maxWalkSpeed && Mathf.Abs(Body.velocity.x) < maxFastWalkSpeed)
            {
                Set(slowRun);
            }
            else if (Mathf.Abs(Body.velocity.x) > maxFastWalkSpeed)
            {
                Set(run);
            }  
        }
        if (!core.Grounded)
        {
            IsComplete = true;
        }
    }
    public override void FixedDo()
    {
        //Horizontal Movement//
        if (MoveInput.x != 0)
        {
            if (Mathf.Abs(Body.velocity.x) < maxRunSpeed) Body.AddForce(new(MoveInput.x * speedAcceleration * 10, 0), ForceMode2D.Force);
        }
    }
    public override void Exit()
    {

    }
}