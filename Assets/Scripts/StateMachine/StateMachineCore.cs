using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateMachineCore : Entity
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

    public bool isLookingRight = true;
    public bool Grounded;
    public bool OnWall;
    public bool canMove = true;
    public bool isMoving;

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
    public void SelectStrategy()
    {
        if (Grounded) machine.Set(groundStates);
        else machine.Set(airStates);
    }
    public void FlipScale()
    {
        float moveDirection = Mathf.Sign(moveInput.x);
        transform.localScale = new Vector3(moveDirection, 1f, 1f);
        if (moveDirection > 0f) isLookingRight = true;
        else if (moveDirection < 0f) isLookingRight = false;
    }
}
