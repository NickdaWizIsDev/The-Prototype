using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Stormhead : Enemy
{
    public float timeBeforeWalking;
    public float moveTimer;
    public float attackRange;
    float direction = 1f;
    public LayerMask collisions;
    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();

        if (Vector2.Distance(transform.position, player.position) <= attackRange)
            Attack();

        if(moveTimer >= timeBeforeWalking)
        {
            canMove = true;
        }

        else moveTimer += Time.deltaTime;
        moveTimer = Mathf.Clamp(moveTimer, 0, timeBeforeWalking);
    }

    private void Attack()
    {
        groundStates.Attack();
        canMove = false;
        moveInput.x = 0;
        moveTimer = 0;
    }

    new private void FixedUpdate()
    {
        base.FixedUpdate();

        if (moveTimer < timeBeforeWalking || !canMove) return;
        moveInput.x = direction;
        Vector2 origin = new(transform.position.x, transform.position.y + 1f);
        RaycastHit2D hit = Physics2D.Raycast(origin, body.velocity.normalized, 20f, collisions);
        Debug.DrawLine(origin, hit.point, Color.green);

        if(Vector2.Distance(origin, hit.point) <= attackRange)
        {
            moveTimer = 0;
            direction *= -1;
        }
    }
}
