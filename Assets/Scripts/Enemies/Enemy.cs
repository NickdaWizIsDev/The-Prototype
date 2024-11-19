using UnityEngine;

[RequireComponent(typeof(Damageable))]
public class Enemy : StateMachineCore
{
    public Damageable damageable;

    private void Start()
    {
        SetupInstances();
    }
    private void Update()
    {
        machine.state.DoBranch();
    }
}