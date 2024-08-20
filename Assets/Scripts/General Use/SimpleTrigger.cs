using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SimpleTrigger : MonoBehaviour
{
    public UnityEvent onTrigger, onTriggerStay, onTriggerExit;

    public bool onlyPlayer = false;
    private bool isColliding;
    private int collisionCount;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!onlyPlayer) onTrigger.Invoke();

        else if (onlyPlayer)
        {
            if (collision.CompareTag("Player")) onTrigger.Invoke();
        }

        collisionCount++;
        isColliding = true;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!onlyPlayer) onTriggerStay.Invoke();

        else if (onlyPlayer)
        {
            if (collision.CompareTag("Player")) onTriggerStay.Invoke();
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        collisionCount--;
        if(collisionCount != 0) { return; }
        else if(collisionCount <= 0) { isColliding = false; }

        if(!onlyPlayer) onTriggerExit.Invoke();

        else if(onlyPlayer)
        {
            if(collision.CompareTag("Player")) onTriggerExit.Invoke();
        }
    }    
}
