using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HealthTextDisplay : MonoBehaviour
{
    Damageable damageable;
    public TextMeshPro text;

    private void Awake()
    {
        damageable = GetComponent<Damageable>();
    }

    // Start is called before the first frame update
    void Start()
    {
        text.text = damageable.Health.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        text.text = damageable.Health.ToString();
    }
}
