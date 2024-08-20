using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : Damageable
{
    public Transform cameraPos;

    [Header("Movement")]
    public float accelerationMultiplier;
    public float maxVelocity;
    public float jumpImpulse;
    public float fallMultiplier;
    public Vector2 currentVelocity;
    public Vector2 moveInput;
    public bool canMove;
    public bool isGrounded;
    public bool isMoving;
    public bool isLookingRight;

    [Header("Attack Values")]
    public float atkCooldown;
    public float atkTimer;
    public bool canAttack;

    [Header("Mana & Spells")]
    public float maxMana;
    public float currentMana;

    // Start is called before the first frame update
    void Start()
    {
        currentMana = maxMana;
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetBool(AnimationStrings.isMoving, isMoving);
        isGrounded = animator.GetBool(AnimationStrings.isGrounded);
        canMove = animator.GetBool(AnimationStrings.canMove);
        animator.SetFloat(AnimationStrings.xVelocity, currentVelocity.x);

        isMoving = moveInput.x != 0;

        if(isMoving)
        {
            float moveDirection = moveInput.x;
            transform.localScale = new Vector3(moveDirection, 1f, 1f);
            if (moveDirection > 0f) isLookingRight = true;
            else if (moveDirection < 0f) isLookingRight = false;
        }        

        if(!canAttack)
        {
            atkTimer -= Time.deltaTime;
        }
        if(atkTimer <= 0) { canAttack = true; }

        currentVelocity = rb2d.velocity;

        if(currentVelocity.y < 0 && !isGrounded)
        {
            rb2d.gravityScale = fallMultiplier;
        }
        else if (isGrounded)
        {
            rb2d.gravityScale = 3;
        }

        if (moveInput.y != 0)
        {
            CamLook(moveInput.y);
        }
        else if(moveInput.y == 0)
        {
            cameraPos.localPosition = new Vector2(0f, 0f);
        }
    }

    private void FixedUpdate()
    {
        if (rb2d.velocity.x < maxVelocity && canMove && isLookingRight)
            rb2d.AddForce(new Vector2(accelerationMultiplier * (Mathf.Abs(moveInput.x)), 0f), ForceMode2D.Force);
        else if (rb2d.velocity.x > -maxVelocity && canMove && !isLookingRight)
            rb2d.AddForce(new Vector2(-accelerationMultiplier * Mathf.Abs(moveInput.x), 0f), ForceMode2D.Force);
    }

    public void CamLook(float offset)
    {
        cameraPos.localPosition = new Vector2(0f, (Mathf.Clamp(offset * 2, -2, 4)));
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.canceled)
            moveInput = Vector2.zero;
        
        else if (context.started && canMove)
        {
            moveInput = context.ReadValue<Vector2>();
        }
    }

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

    public void OnFire(InputAction.CallbackContext context)
    {
        if(context.started && isGrounded && canAttack) { animator.SetTrigger(AnimationStrings.atk); atkTimer = atkCooldown; moveInput = Vector2.zero; rb2d.velocity = Vector2.zero; }
    }
}
