using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SimpleTextMoveAndFade : MonoBehaviour
{
    Transform rectTransform;
    float timeElapsed;

    public TextMeshProUGUI textUI;
    public Color startColor;
    public Vector3 moveSpeed;
    public float timeToFade;

    private void Awake()
    {
        rectTransform = GetComponent<Transform>();
        textUI = GetComponent<TextMeshProUGUI>();
        startColor = textUI.color;
    }

    // Update is called once per frame
    void Update()
    {
        rectTransform.position += moveSpeed * Time.deltaTime;
        timeElapsed += Time.deltaTime;

        if(timeElapsed < timeToFade)
        {
            float fadeAlpha = startColor.a * (1 - (timeElapsed / timeToFade));
            textUI.color = new(textUI.color.r, textUI.color.g, textUI.color.b, fadeAlpha);
        }
        else if(timeElapsed >= timeToFade)
        {
            Destroy(gameObject);
        }
    }
}
