using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostTrigger : MonoBehaviour
{
    public int speedMultiplier;

    public void OnTriggerStay2D(Collider2D collision)
    {
        Debug.Log("Boost!");
        collision.GetComponent<Rigidbody2D>().AddForce(Vector2.right * speedMultiplier / 10, ForceMode2D.Impulse);
    }
}
