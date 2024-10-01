using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCounter : MonoBehaviour
{
    public GameObject transition;
    public List<GameObject> enemies;

    // Start is called before the first frame update
    void Start()
    {
        transition.SetActive(false);

        for(int i = 0; i < transform.childCount; i++)
        {
            GameObject obj = transform.GetChild(i).gameObject;
            enemies.Add(obj);
        }
    }

    // Update is called once per frame
    void Update()
    {
        GameObject[] enemiesCopy = new GameObject[enemies.Count];
        enemies.CopyTo(enemiesCopy, 0);

        foreach (GameObject obj in enemiesCopy)
        {
            if (obj != null && obj.transform.childCount <= 0)
            {
                DestroyImmediate(obj);
            }
        }
        if (gameObject.transform.childCount <= 0)
        {
            transition.SetActive(true);
        }
    }
}
