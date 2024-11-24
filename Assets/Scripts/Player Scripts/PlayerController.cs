using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class PlayerController : StateMachineCore
{
    //Add states here//

    [Header("Movement Variables")]
    public AirVariables airVariables;
    public GroundVariables groundVariables;
    public DashVariables dashVariables;
    float jumpBufferTime => airVariables.jumpBufferTime;
    float coyoteTime => airVariables.coyoteTime;
    float jumpImpulse => airVariables.jumpImpulse;
    float normalGravity => airVariables.normalGravity;
    float fallMultiplier => airVariables.fallMultiplier;
    float airAcceleration => airVariables.airAcceleration;
    float maxAirSpeed => airVariables.maxAirSpeed;
    float groundSpeedAcceleration => groundVariables.groundSpeedAcceleration;
    float maxWalkSpeed => groundVariables.maxWalkSpeed;
    float maxThrottleSpeed => groundVariables.maxThrottleSpeed;
    float maxRunSpeed => groundVariables.maxRunSpeed;
    float jumpBufferTimer;
    float coyoteTimer;
    float dashTimer;

    public Vector2 currentVelocity;
    public bool canMove = true;
    public bool isMoving;
    public bool isLookingRight = true;

    bool buffer = false;

    [Header("Mana & Spells")]
    public int maxMana;
    public int currentMana;
    public float manaRestoreRate = 1f;
    public float manaRestoreAmount = 5;
    private bool gainingMana;

    [Header ("Save Data")]
    public PersistentData persistentData;
    public Damageable hp;

    [Serializable]
    public class AirVariables
    {
        public float jumpBufferTime = 0.3f;
        public float coyoteTime = 0.2f;
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
    [Serializable]
    public class DashVariables
    {
        public float dashForce = 25f;
        public float dashCooldown;
    }

    private void Awake()
    {
        //GetComponent spam, but only components in the same obj, keep it to a minimum//
    }

    void Start()
    {
        //Parameters that need to be set at game start//
        currentMana = maxMana;
        SetupInstances();
        SelectStrategy();
    }

    void Update()
    {        
        if (machine.state.IsComplete) { SelectStrategy(); }
        machine.state.DoBranch(); //Run the state's update//

        OnUpdateParameters(); //Parameters that need to be updated every frame//
        StartCoroutine(RegainMana()); //Mana Restoring//        
        isMoving = moveInput.x != 0f;
        if (isMoving) FlipScale(); //Scale flipping//
    }

    void SelectStrategy()
    {
        if (Grounded) machine.Set(groundStates);
        else machine.Set(airStates);
    }

    void OnUpdateParameters()
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

        if (!Grounded) coyoteTimer -= Time.deltaTime;
        coyoteTimer = Mathf.Clamp(coyoteTimer, 0, coyoteTime);
        if(Grounded && coyoteTimer != coyoteTime) coyoteTimer = coyoteTime;

        if (buffer)
        {
            jumpBufferTimer -= Time.deltaTime;
            jumpBufferTimer = Mathf.Clamp(jumpBufferTimer, 0, jumpBufferTime);
            if(jumpBufferTimer <= 0) { buffer = false; return; }
            else if(jumpBufferTimer > 0 && Grounded) { airStates.Jump(); jumpBufferTimer = 0; buffer = false; }
        }

        if(dashTimer > 0)
        {
            dashTimer -= Time.deltaTime;
            dashTimer = Mathf.Clamp(dashTimer, 0, dashVariables.dashCooldown);
        }

        //Set my persistent parameters so that mana and health upgrades remain in the game even through sessions//
        persistentData.playerMaxMana = maxMana;
        persistentData.playerMaxHealth = hp.MaxHealth;
    }

    void FlipScale()
    {
        float moveDirection = Mathf.Sign(moveInput.x);
        transform.localScale = new Vector3(moveDirection, 1f, 1f);
        if (moveDirection > 0f) isLookingRight = true;
        else if (moveDirection < 0f) isLookingRight = false;
    }

    private void FixedUpdate()
    {
        machine.state.FixedDoBranch(); //Run the state's fixed update//

        //Parameters that only need to be updated on a fixed timeframe//
        currentVelocity = body.velocity;
    }

    //Inputs from New Input System//
    public void OnMove(InputAction.CallbackContext context)
    {
        if(animator.GetBool(AnimationStrings.canMove)) moveInput = context.ReadValue<Vector2>();
    }
    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.canceled && body.velocity.y > 0) body.velocity = new(body.velocity.x, body.velocity.y - (body.velocity.y / 2));
        else if (context.started)
        {
            if(Grounded) {airStates.Jump(); coyoteTimer = 0; }
            else if(!Grounded && coyoteTimer != 0) { airStates.MidAirJump(); }
            else if(!Grounded && coyoteTimer <= 0 && !buffer) { buffer = true; jumpBufferTimer = jumpBufferTime; }
        }
    }
    public void OnSwordAtk(InputAction.CallbackContext context)
    {
        if (context.started && Grounded) groundStates.Attack();
        else if (context.started && !Grounded) return; //Air Attacks//
    }
    public void OnDash(InputAction.CallbackContext context)
    {
        if (context.started && dashTimer <= 0)
        {
            body.AddForce(new(dashVariables.dashForce * transform.localScale.x, 0), ForceMode2D.Impulse);
            dashTimer = dashVariables.dashCooldown;
        }
    }

    //Self explanatory//
    IEnumerator RegainMana()
    {
        if (gainingMana) yield break;
        gainingMana = true;

        if (currentMana >= maxMana) { currentMana = maxMana; yield break; }

        float timer = 0f;

        while (timer < manaRestoreRate)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        if (currentMana + manaRestoreAmount >= maxMana) currentMana = maxMana;
        else currentMana += 5;
        gainingMana = false;
    }
}
