using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(TouchingDirections))]
public abstract class StateMachineCore : MonoBehaviour
{
    //Character Core//
    [Header("Statemachine Core")]
    public Rigidbody2D body;
    public Animator animator;
    public StateMachine machine;
    public TouchingDirections touching;
    public Vector2 moveInput;
    [Range(0f, 5f)] public float drag = 0.5f;
    [Range(0f, 1f)] public float baseDrag;

    public bool Grounded;
    public bool OnWall;

    [Header("States")]
    public GroundStates groundStates;
    public AirStates airStates;

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
