using Cinemachine;
using System.Collections;
using UnityEngine;
public class SimpleParallax : MonoBehaviour
{
    public Transform player;
    public float offset;
    float yPos;
    private float length;
    private float startPos;

    private void Start()
    {
        startPos = transform.position.x;
        yPos = transform.localPosition.y;
        length = gameObject.GetComponent<SpriteRenderer>().sprite.bounds.size.x;
    }

    private void Update()
    {
        float distance = (player.transform.position.x * offset);
        transform.position = new Vector3(startPos + distance, transform.position.y, transform.position.z);

        float temp = (player.transform.position.x * (1 - offset));

        if (temp > startPos + length)
        {
            startPos += length;
        }
        else if (temp < startPos - length)
        {
            startPos -= length;
        }
    }
}