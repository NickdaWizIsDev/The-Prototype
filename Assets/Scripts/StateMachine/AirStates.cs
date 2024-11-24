using System.Collections;
using UnityEngine;
public class AirStates : State
{
    [HideInInspector] public float jumpImpulse = 7;
    [HideInInspector] public float normalGravity = 3;
    [HideInInspector] public float fallMultiplier = 5;
    [HideInInspector] public float airAcceleration = 1.5f;
    [HideInInspector] public float maxAirSpeed = 24f;
    public bool grounded;
    protected Vector2 MoveInput
    {
        get
        {
            return core.moveInput;
        }
    }

    public JumpState jumpState;
    public MidAirState midAirState;
    public FallState fallState;

    public override void Enter()
    {
        if (Body.velocity.y > 0) Set(jumpState);
        else if (Body.velocity.y <= 0) Set(midAirState);
    }
    public override void Do()
    {
        GravityControl();
        if (grounded)
        {
            IsComplete = true;
        }

        core.animator.SetBool(AnimationStrings.isGrounded, false);
    }
    public override void FixedDo()
    {
        //Horizontal Movement//
        if (MoveInput.x != 0)
        {
            if (Mathf.Abs(Body.velocity.x) < maxAirSpeed) Body.AddForce(new(MoveInput.x * airAcceleration * 10, 0), ForceMode2D.Force);
        }
    }
    public override void Exit()
    {

    }
    void GravityControl()
    {
        if (grounded)
        {
            Body.gravityScale = normalGravity;
        }
        else if (Body.velocity.y < 0 && !grounded)
        {
            Body.gravityScale = fallMultiplier;
        }
    }
    public void Jump()
    {
        Body.AddForce(new(0, jumpImpulse), ForceMode2D.Impulse);
    }

    public void MidAirJump()
    {
        Body.AddForce(new(0, jumpImpulse*1.5f), ForceMode2D.Impulse);
        Set(jumpState);
    }
}
