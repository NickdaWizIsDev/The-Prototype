using System.Collections;
using UnityEngine;
public class SimpleParallax : MonoBehaviour
{
    public Transform player;

    private void Update()
    {
        transform.position = new(transform.position.x, player.position.y);
    }
}