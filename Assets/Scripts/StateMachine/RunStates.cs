using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RunStates : State
{
    [Header("Parameters")]
    public float speedAcceleration = 1f;
    public float maxWalkSpeed;
    public float maxThrottleSpeed;
    public float maxRunSpeed;
    protected Vector2 MoveInput
    {
        get
        {
            return core.moveInput;
        }
    }

    [Header("Substates")]
    public State walk, throttle, run, idleState;
    public override void Enter()
    {
        if (Mathf.Abs(Body.velocity.x) > maxWalkSpeed)
        {
            if (Mathf.Abs(Body.velocity.x) > maxThrottleSpeed) Set(run, true);
            else Set(throttle, true);
        }
        else Set(walk, true);
    }
    public override void Do()
    {
        if (Mathf.Abs(Body.velocity.x) < 0.05f && MoveInput.x == 0)
        {
            parent.Set(idleState);
            IsComplete = true;
        }
        else if (Mathf.Abs(Body.velocity.x) != 0f)
        {
            if (Mathf.Abs(Body.velocity.x) < maxWalkSpeed)
            {
                Set(walk, true);
            }                
            else if (Mathf.Abs(Body.velocity.x) > maxWalkSpeed && Mathf.Abs(Body.velocity.x) < maxThrottleSpeed)
            {
                Set(throttle, true);
            }
            else if (Mathf.Abs(Body.velocity.x) > maxThrottleSpeed)
            {
                Set(run, true);
            }  
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
        machine.state = idleState;
    }
    public void RunAttack()
    {
        //caca
    }
}