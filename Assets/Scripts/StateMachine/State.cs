using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State : MonoBehaviour
{
    public StateMachine machine;
    public StateMachine parent;
    public State CurrentState => machine.state;

    protected StateMachineCore core;
    protected Animator Animator => core.animator;
    protected Rigidbody2D Body => core.body;
    public bool IsComplete { get; protected set; }

    protected float startTime;

    public float time => Time.time - startTime;

    public virtual void Enter()
    {

    }
    public virtual void Do()
    {

    }
    public virtual void FixedDo()
    {

    }
    public virtual void Exit()
    {

    }
    public void Initialise(StateMachine _parent)
    {
        if(parent == null)
        {
            parent = _parent;
        }
        IsComplete = false;
        startTime = Time.time;
    }
    public void SetCore(StateMachineCore _core)
    {
        core = _core;
        machine = gameObject.AddComponent<StateMachine>();
    }
    protected void Set(State newState, bool forceReset = false)
    {
        machine.Set(newState, forceReset);
    }
    public void DoBranch()
    {
        Do();
        if(CurrentState != null) CurrentState.DoBranch();
    }
    public void FixedDoBranch()
    {
        FixedDo();
        if (CurrentState != null) CurrentState.FixedDoBranch();
    }
}
