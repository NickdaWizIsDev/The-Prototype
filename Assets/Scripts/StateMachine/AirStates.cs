using System.Collections;
using UnityEngine;
public class AirStates : State
{
    public float jumpImpulse = 7;
    public float normalGravity = 3;
    public float fallMultiplier = 5;

    protected JumpState jumpState;
    protected MidAirState midAirState;
    protected FallState fallState;

    public override void Enter()
    {
        
    }
    public override void Do()
    {
        GravityControl();
    }
    public override void Exit() 
    { 

    }
    void GravityControl()
    {
        if (Body.velocity.y < 0 && !Touching.IsGrounded)
        {
            Body.gravityScale = fallMultiplier;
        }
        else if (Touching.IsGrounded)
        {
            Body.gravityScale = normalGravity;
        }
    }
}
