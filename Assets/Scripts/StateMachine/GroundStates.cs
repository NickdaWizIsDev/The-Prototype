using UnityEngine;
using System.Collections;
public class GroundStates : State
{
    public IdleState idleState;
    public RunStates runStates;
    public BaseAttackState attackState;
    protected Vector2 MoveInput
    {
        get
        {
            return core.moveInput;
        }
    }

    public override void Enter()
    {
        if (MoveInput == Vector2.zero) Set(idleState);
        else
        {
            if (MoveInput.x != 0) Set(runStates);
        }

    }
    public override void Do()
    {
        if (!core.Grounded) { IsComplete = true; parent.Set(core.airStates); }
        if (MoveInput.x != 0) Set(runStates);
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
        if (MoveInput == Vector2.zero && core.Grounded && Body.velocity.x != 0)
        {
            Body.drag = core.drag;
        }
        else Body.drag = core.baseDrag;
    }
    public void Attack()
    {
        if (machine.state == runStates) { runStates.RunAttack(); }
        else { if (machine.state != attackState) Set(attackState); else attackState.BasicAttack(); Body.velocity = Vector2.zero; }
    }
}