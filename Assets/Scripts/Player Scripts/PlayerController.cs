using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : StateMachineCore
{
    //Add states here//
    public JumpState jumpState;

    [Header("Movement")]    
    public Vector2 currentVelocity;
    public Vector2 moveInput;
    public bool canMove = true;
    public bool isGrounded;
    public bool isMoving;
    public bool isLookingRight;

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
        animator = GetComponent<Animator>();
        body = GetComponent<Rigidbody2D>();
        touching = GetComponent<TouchingDirections>();
    }

    void Start()
    {
        //Parameters that need to be set at game start//
        currentMana = maxMana;
        SetupInstances();
        machine.Set(idleState);
    }

    void Update()
    {        
        if(state.IsComplete) { }

        OnUpdateParameters(); //Parameters that need to be updated every frame//
        StartCoroutine(RegainMana()); //Mana Restoring//

        state.DoBranch(); //Run the state's update//
        
        isMoving = moveInput.x != 0f;
        if (isMoving) FlipScale(); //Scale flipping//
    }

    void OnUpdateParameters()
    {
        runState.moveInput = moveInput;

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
        state.FixedDoBranch(); //Run the state's fixed update//

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
        if(context.canceled) return;
        else if(context.started)
        {
            machine.Set(jumpState);
        }
    }
    public void OnSwordAtk(InputAction.CallbackContext context)
    {
        if (context.started && isGrounded && !isMoving) return; //Attack Combo//
        else if (context.started && isGrounded && isMoving) return; //Running Attack combo//
        else if (context.started && !isGrounded) return; //Air Attacks//
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
