using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelPortal : MonoBehaviour
{
    public bool portalActive = false;

    public int targetSceneIndex;
    public PersistentData data;

    public SpriteRenderer spriteRenderer;

    private bool isLevelActive;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Start()
    {
        if (data.levelChecks[targetSceneIndex])
        {
            portalActive = true;
        }
    }

    public void Update()
    {
        if (portalActive)
        {
            spriteRenderer.color = Color.white;
        }
        else if(!portalActive)
        {
            spriteRenderer.color = Color.black;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (portalActive)
        {
            SceneManager.LoadScene(targetSceneIndex);
        }
    }
}
