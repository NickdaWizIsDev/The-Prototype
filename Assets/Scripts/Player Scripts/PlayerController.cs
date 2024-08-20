using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float accelerationMultiplier = 1;
    public float maxVelocity = 15;
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

    [Header("Mana & Spells")]
    public int maxMana;
    public int currentMana;
    private bool gainingMana;

    [Header ("Save Data")]
    public PersistentData persistentData;

    [HideInInspector]
    public Animator animator;
    [HideInInspector]
    public Rigidbody2D rb2d;

    private void Awake()
    {
        //GetComponent spam, only components in the same obj//
        animator = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        //Param setters, but only on start//
        currentMana = maxMana;
    }

    void Update()
    {
        //Param setters, but every frame//
        animator.SetBool(AnimationStrings.isMoving, isMoving);
        isGrounded = animator.GetBool(AnimationStrings.isGrounded);
        canMove = animator.GetBool(AnimationStrings.canMove);
        animator.SetFloat(AnimationStrings.xVelocity, currentVelocity.x);
        isMoving = moveInput.x != 0;

        persistentData.playerMaxMana = maxMana;
        persistentData.playerMaxHealth = GetComponent<Damageable>().MaxHealth;

        //Mana Restoring Process//
        if(currentMana < maxMana) { StartCoroutine(RegainMana()); }

        //Scale flipping//
        if(isMoving)
        {
            float moveDirection = moveInput.x;
            transform.localScale = new Vector3(moveDirection, 1f, 1f);
            if (moveDirection > 0f) isLookingRight = true;
            else if (moveDirection < 0f) isLookingRight = false;
        }        

        //Attack Cooldown//
        if(!canAttack)
        {
            atkTimer -= Time.deltaTime;
        }
        if(atkTimer <= 0) { canAttack = true; }

        //Gravity multiplication (might go if I start using my own physics)//
        if(currentVelocity.y < 0 && !isGrounded)
        {
            rb2d.gravityScale = fallMultiplier;
        }
        else if (isGrounded)
        {
            rb2d.gravityScale = normalGravity;
        }
    }

    private void FixedUpdate()
    {
        //Apply force in the appopiate direction, stop adding if we're over the maximum speed//
        if (rb2d.velocity.x < maxVelocity && canMove && isLookingRight)
            rb2d.AddForce(new Vector2(accelerationMultiplier * moveInput.x, 0f), ForceMode2D.Force);
        else if (rb2d.velocity.x > -maxVelocity && canMove && !isLookingRight)
            rb2d.AddForce(new Vector2(accelerationMultiplier * moveInput.x, 0f), ForceMode2D.Force);

        //Param setters, but every couple frames//
        currentVelocity = rb2d.velocity;
    }

    //Self explanatory//
    IEnumerator RegainMana()
    {
        if(gainingMana) yield break;
        gainingMana = true;

        float time = 1f;
        float timer = 0f;

        while (timer < time)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        if (currentMana + 5 >= maxMana) currentMana = maxMana;
        else currentMana += 5;
        gainingMana = false;
    }

    //Move input handling//
    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.canceled)
            moveInput = Vector2.zero;
        
        else if (context.started && canMove)
        {
            moveInput = context.ReadValue<Vector2>();
        }
    }

    //Jump input handling//
    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started && isGrounded && canMove)
        {
            Debug.Log("Jump!");
            rb2d.AddForce(new Vector2(0, jumpImpulse), ForceMode2D.Force);
        }
        else if (context.canceled)
        {
            rb2d.gravityScale = fallMultiplier;
        }
    }

    //Attack input handling//
    public void OnFire(InputAction.CallbackContext context)
    {
        if(context.started && isGrounded && canAttack) { animator.SetTrigger(AnimationStrings.atk); atkTimer = atkCooldown; moveInput = Vector2.zero; rb2d.velocity = Vector2.zero; }
    }
}
