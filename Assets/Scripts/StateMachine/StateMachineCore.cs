using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateMachineCore : MonoBehaviour
{
    //Character Core//
    [Header("Statemachine Core")]
    public Rigidbody2D body;
    public Animator animator;
    public StateMachine machine;
    public TouchingDirections touching;

    [Header("States")]
    public State state;
    public IdleState idleState;
    public RunState runState;

    public void SetupInstances()
    {
        machine = gameObject.AddComponent<StateMachine>();

        State[] allChildStates = GetComponentsInChildren<State>();
        foreach (State state in allChildStates)
        {
            state.SetCore(this);
        }
    }
}
