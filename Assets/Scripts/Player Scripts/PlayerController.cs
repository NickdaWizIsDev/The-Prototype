using Assets.Scripts.General_Use;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float speedAcceleration = 3f;
    public float maxSpeed = 15;
    [Range(0f, 2f)] public float drag = 0.3f;
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
    float swordDamage;

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
        
        isMoving = moveInput.x != 0f;
        if (isMoving) FlipScale(); //Scale flipping//

        GravityControl(); //Gravity multiplication (might go if I start using my own physics//
    }

    void OnUpdateParameters()
    {
        animator.SetBool(AnimationStrings.isMoving, isMoving);
        animator.SetFloat(AnimationStrings.xVelocity, Mathf.Abs(currentVelocity.x));
        animator.SetFloat(AnimationStrings.yVelocity, currentVelocity.y);
        isGrounded = animator.GetBool(AnimationStrings.isGrounded);
        canMove = animator.GetBool(AnimationStrings.canMove);

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

    private void FixedUpdate()
    {   
        PlayerMovement(); //Move the player//
        StartCoroutine(Drag()); //Ground Drag//

        //Parameters that only need to be updated on a fixed timeframe//
        currentVelocity = body.velocity;
    }

    void PlayerMovement()
    {
        //Horizontal Movement//
        if (moveInput.x != 0)
        {
            if(Mathf.Abs(body.velocity.x) <= maxSpeed) body.AddForce(new(moveInput.x * speedAcceleration * 10, 0), ForceMode2D.Force);
        }

        //Vertical Movement//
        if (moveInput.y != 0 && isGrounded)
        {
            //Pending ladder climbing, also going down two-way platforms//
        }
    }
    IEnumerator Drag()
    {
        if (moveInput.x != 0 || body.velocity == Vector2.zero || !isGrounded) yield break;
        else if (moveInput.x == 0f && isGrounded && body.velocity.x > 0)
        {
            float duration = 1f;
            var timePassed = 0f;
            float value = 0f;
            while (timePassed < duration)
            {
                // This factor moves linear from 0 to 1
                var factor = timePassed / duration;
                // This adds ease-in and ease-out 
                // see https://docs.unity3d.com/ScriptReference/Mathf.SmoothStep.html
                // Basically you can use ANY mathematical function that maps
                // the input of [0; 1] again to a range of [0;1] 
                // with the easing you like
                factor = Mathf.SmoothStep(0, 1, factor);

                // And this is how finally you use Lerp in this case
                value = Mathf.Lerp(0, drag, factor);

                // This tells Unity to "pause" the routine here
                // render this frame and continue from here in the next one
                yield return value;

                // increase by the time passed since last frame
                timePassed += Time.deltaTime;
            }
        }
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
            animator.SetTrigger(AnimationStrings.jump);
            isGrounded = false;
            body.AddForce(new(0, jumpImpulse), ForceMode2D.Impulse);
        }
    }
    public void OnSwordAtk(InputAction.CallbackContext context)
    {
        if(context.started && isGrounded && canMove && !isMoving) { animator.SetTrigger(AnimationStrings.swordAtk); }
        else if(context.started && isGrounded && canMove && isMoving) { animator.SetTrigger(AnimationStrings.swordMoveAtk); }
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
