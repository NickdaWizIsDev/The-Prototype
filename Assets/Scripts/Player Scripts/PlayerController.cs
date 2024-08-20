using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 7;
    [Range(1f, 0f)] public float drag = 0.3f;
    public float jumpImpulse = 7;
    public float normalGravity = 3;
    public float fallMultiplier = 5;
    public Vector2 currentVelocity;
    public Vector2 moveInput;
    public bool canMove = true;
    public bool isGrounded;
    public bool isMoving;
    public bool isLookingRight;

    [Header("Attack Values")]
    public float atkCooldown;
    public float atkTimer;
    public bool canAttack = true;
    bool attackOnCD;

    [Header("Mana & Spells")]
    public int maxMana;
    public int currentMana;
    public float manaRestoreRate = 1f;
    public float manaRestoreAmount = 5;
    private bool gainingMana;

    [Header ("Save Data")]
    public PersistentData persistentData;

    [HideInInspector]
    public Animator animator;
    [HideInInspector]
    public Rigidbody2D body;

    private void Awake()
    {
        //GetComponent spam, but only components in the same obj, keep it to a minimum//
        animator = GetComponent<Animator>();
        body = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        //Parameters that need to be set at game start//
        currentMana = maxMana;
    }

    void Update()
    {        
        OnUpdateParameters(); //Parameters that need to be updated every frame//
        StartCoroutine(RegainMana()); //Mana Restoring//
        
        isMoving = moveInput != Vector2.zero;
        if (isMoving) FlipScale(); //Scale flipping//

        if (!canAttack) attackOnCD = true;
        AttackCooldown(); //Attack Cooldown//
        GravityControl(); //Gravity multiplication (might go if I start using my own physics)//        
        MoveInput(); //Movement Input Handler//
    }

    void OnUpdateParameters()
    {
        animator.SetBool(AnimationStrings.isMoving, isMoving);
        isGrounded = animator.GetBool(AnimationStrings.isGrounded);
        canMove = animator.GetBool(AnimationStrings.canMove);
        animator.SetFloat(AnimationStrings.xVelocity, currentVelocity.x);

        //Set my persistent parameters so that mana and health upgrades remain in the game even through sessions//
        persistentData.playerMaxMana = maxMana;
        persistentData.playerMaxHealth = GetComponent<Damageable>().MaxHealth;
    }

    void FlipScale()
    {
        float moveDirection = moveInput.x;
        transform.localScale = new Vector3(moveDirection, 1f, 1f);
        if (moveDirection > 0f) isLookingRight = true;
        else if (moveDirection < 0f) isLookingRight = false;
    }
    void AttackCooldown()
    {
        if (attackOnCD)
        {
            atkTimer += Time.deltaTime;
        }
        if (atkTimer >= atkCooldown) { canAttack = true; attackOnCD = false; atkTimer = 0f; }
    }
    void GravityControl()
    {
        if (currentVelocity.y < 0 && !isGrounded)
        {
            body.gravityScale = fallMultiplier;
        }
        else if (isGrounded)
        {
            body.gravityScale = normalGravity;
        }
    }
    public void MoveInput()
    {
        moveInput.x = Input.GetAxis("Horizontal");
        moveInput.y = Input.GetAxis("Vertical");
    }

    private void FixedUpdate()
    {   
        PlayerMovement(); //Move the player//
        Drag(); //Ground Drag//

        //Parameters that only need to be updated every couple frames//
        currentVelocity = body.velocity;
    }

    void PlayerMovement()
    {
        //Horizontal Movement//
        if (moveInput.x != 0)
        {
            body.velocity = new((moveInput.x * speed), body.velocity.y);
        }

        //Vertical Movement//
        if (moveInput.y != 0 && isGrounded)
        {
            body.velocity = new(body.velocity.x, moveInput.y * jumpImpulse);
        }
    }
    void Drag()
    {
        if (moveInput == Vector2.zero && isGrounded) body.velocity *= drag;
    }

    //Attack input handling//
    public void OnAttack(InputAction.CallbackContext context)
    {
        if(context.started && isGrounded && canAttack && !isMoving) { animator.SetTrigger(AnimationStrings.atk); }
        else if(context.started && isGrounded && canAttack && isMoving) { animator.SetTrigger(AnimationStrings.moveAtk); }
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
