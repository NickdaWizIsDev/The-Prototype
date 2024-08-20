using Assets.Scripts.General_Use;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindAndShoot : MonoBehaviour
{
    Simple2DMove move;
    public GameObject target;

    private void Awake()
    {
        move = GetComponent<Simple2DMove>();
    }

    public void Shoot()
    {
        if (move != null)
        {
            if (target != null) { move.Move(target); }
            else Debug.Log("Target for object " + gameObject.name + "is not set");
        }
    }
}
