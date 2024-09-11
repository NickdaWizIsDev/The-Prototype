using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : StateMachineCore
{
    //Add states here//

    [Header("Movement")]    
    public Vector2 currentVelocity;
    public bool canMove = true;
    public bool isMoving;
    public bool isLookingRight = true;

    [Header("Mana & Spells")]
    public int maxMana;
    public int currentMana;
    public float manaRestoreRate = 1f;
    public float manaRestoreAmount = 5;
    private bool gainingMana;

    [Header ("Save Data")]
    public PersistentData persistentData;

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
        airStates.jumpState.grounded = Grounded;
        airStates.midAirState.grounded = Grounded;
        airStates.fallState.grounded = Grounded;

        //Set my persistent parameters so that mana and health upgrades remain in the game even through sessions//
        persistentData.playerMaxMana = maxMana;
        persistentData.playerMaxHealth = GetComponent<Damageable>().MaxHealth;
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
        moveInput = context.ReadValue<Vector2>();
    }
    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.canceled && body.velocity.y > 0) body.velocity = new(body.velocity.x, body.velocity.y - (body.velocity.y / 2));
        else if (context.started && Grounded)
        {
            airStates.Jump();
        }
    }
    public void OnSwordAtk(InputAction.CallbackContext context)
    {
        if (context.started && Grounded) groundStates.Attack();
        else if (context.started && !Grounded) return; //Air Attacks//
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
