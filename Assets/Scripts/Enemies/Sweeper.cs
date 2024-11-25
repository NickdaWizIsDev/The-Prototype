using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Sweeper : Enemy
{
    public bool isActive;
    public float timeBeforeWalking;
    public float moveTimer;
    public float attackRange = 1f;
    public float walkRange = 5f;
    float direction = 1f;
    public LayerMask collisions;
    bool move;
    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        canMove = false;
    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();
        canMove = animator.GetBool(AnimationStrings.canMove);
        animator.SetBool("isActive", isActive);
        if (Grounded) machine.Set(groundStates);
        else machine.Set(airStates);

        if (Vector2.Distance(transform.position, player.position) <= attackRange && isActive)
            Attack();

        isActive = Vector2.Distance(transform.position, player.position) <= walkRange;

        if (canMove)
        {
            if (moveTimer >= timeBeforeWalking)
            {
                move = true;
            }

            else moveTimer += Time.deltaTime;
            moveTimer = Mathf.Clamp(moveTimer, 0, timeBeforeWalking);
        }
    }

    private void Attack()
    {
        groundStates.Attack();
        Vector2 distance = player.position - transform.position;
        if (distance.x < 0) transform.localScale = new(-1, 1);
        move = false;
        moveInput.x = 0;
        moveTimer = 0;
    }

    new private void FixedUpdate()
    {
        base.FixedUpdate();

        if (moveTimer < timeBeforeWalking || !move) return;
        moveInput.x = direction;
        Vector2 origin = new(transform.position.x, transform.position.y + .5f);
        RaycastHit2D hit = Physics2D.Raycast(origin, body.velocity.normalized, 50f, collisions);
        Debug.DrawLine(origin, hit.point, Color.green);

        if (Vector2.Distance(origin, hit.point) <= attackRange)
        {
            moveTimer = 0;
            direction *= -1;
        }
    }
}
