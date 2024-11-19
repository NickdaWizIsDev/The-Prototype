using Assets.Scripts.General_Use;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindAndShoot : Simple2DMove
{
    public GameObject target;
    public int a;

    public void Shoot()
    {
        if (target != null) { MoveLinear(target); }
        else Debug.Log("Target for object " + gameObject.name + "is not set");
    }
}
