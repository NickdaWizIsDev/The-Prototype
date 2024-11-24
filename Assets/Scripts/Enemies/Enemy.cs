using System;
using UnityEngine;

[RequireComponent(typeof(Damageable))]
public class Enemy : StateMachineCore
{
    public Damageable damageable;
    public AirVariables airVariables;
    public GroundVariables groundVariables;

    public Transform player;

    protected float jumpImpulse => airVariables.jumpImpulse;
    protected float normalGravity => airVariables.normalGravity;
    protected float fallMultiplier => airVariables.fallMultiplier;
    protected float airAcceleration => airVariables.airAcceleration;
    protected float maxAirSpeed => airVariables.maxAirSpeed;
    protected float groundSpeedAcceleration => groundVariables.groundSpeedAcceleration;
    protected float maxWalkSpeed => groundVariables.maxWalkSpeed;
    protected float maxThrottleSpeed => groundVariables.maxThrottleSpeed;
    protected float maxRunSpeed => groundVariables.maxRunSpeed;
    [Serializable]
    public class AirVariables
    {
        public float jumpImpulse = 7;
        public float normalGravity = 3;
        public float fallMultiplier = 5;
        public float airAcceleration = 1.5f;
        public float maxAirSpeed = 24f;
    }
    [Serializable]
    public class GroundVariables
    {
        public float groundSpeedAcceleration = 1.5f;
        public float maxWalkSpeed = 3f;
        public float maxThrottleSpeed = 12f;
        public float maxRunSpeed = 20f;
    }

    protected void Start()
    {
        SetupInstances();
        SelectStrategy();
    }
    protected void Update()
    {
        machine.state.DoBranch();

        UpdateParameters();
        isMoving = moveInput.x != 0f;
        if (isMoving) FlipScale(); //Scale flipping//
    }

    private void UpdateParameters()
    {
        Grounded = touching.IsGrounded;
        OnWall = touching.IsOnWall;
        airStates.grounded = Grounded;
        airStates.jumpImpulse = jumpImpulse;
        airStates.maxAirSpeed = maxAirSpeed;
        airStates.normalGravity = normalGravity;
        airStates.airAcceleration = airAcceleration;
        airStates.fallMultiplier = fallMultiplier;
        groundStates.speedAcceleration = groundSpeedAcceleration;
        groundStates.maxWalkSpeed = maxWalkSpeed;
        groundStates.maxThrottleSpeed = maxThrottleSpeed;
        groundStates.maxRunSpeed = maxRunSpeed;
    }

    protected void FixedUpdate()
    {
        machine.state.FixedDoBranch();
    }
}