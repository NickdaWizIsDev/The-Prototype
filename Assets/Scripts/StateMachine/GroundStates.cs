using UnityEngine;
using System.Collections;
public class GroundStates : State
{
    public IdleState idleState;
    public RunStates runState;
    protected Vector2 MoveInput
    {
        get
        {
            return core.moveInput;
        }
    }

    public override void Enter()
    {
        if (MoveInput == Vector2.zero) Set(idleState, true);
        else
        {
            if (MoveInput.x != 0) Set(runState, true); runState.Enter();
        }

    }
    public override void Do()
    {
        if (!core.Grounded) IsComplete = true;
    }
    public override void FixedDo()
    {
        Drag();
    }
    public override void Exit()
    {
        
    }
    void Drag()
    {
        if(MoveInput == Vector2.zero && core.Grounded)
        {
            Vector2 decay = new(core.drag, 1f);
            Body.velocity *= decay;
        }
    }
}