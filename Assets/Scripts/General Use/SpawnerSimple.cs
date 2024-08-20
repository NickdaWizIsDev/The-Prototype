using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerSimple : MonoBehaviour
{
    public Transform spawnPosition;
    public GameObject spawnObject;

    public bool spawnAsChild;

    public void Spawnear()
    {
        if (spawnAsChild) { Instantiate(spawnObject, spawnPosition); }
        else { Instantiate(spawnObject, spawnPosition.position, spawnPosition.rotation); }
    }
}
