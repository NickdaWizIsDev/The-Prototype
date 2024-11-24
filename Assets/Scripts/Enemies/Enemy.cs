using UnityEngine;

[RequireComponent(typeof(Damageable))]
public class Enemy : StateMachineCore
{
    public Damageable damageable;

    protected void Start()
    {
        SetupInstances();
    }
    protected void Update()
    {
        machine.state.DoBranch();
    }

    protected void FixedUpdate()
    {
        machine.state.FixedDoBranch();
    }
}