using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleRotation : MonoBehaviour
{
    public float degreesPerSecond;

    void Update()
    {
        transform.Rotate(0, 0, degreesPerSecond * Time.deltaTime); //rotates 50 degrees per second around z axis }
    }
}
