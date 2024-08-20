using System.Collections;
using UnityEngine;

using UnityEngine.SceneManagement;
public class LevelCheck : MonoBehaviour
{
    public int levelID;
    public PersistentData persistentData;

    // Use this for initialization
    public void Start()
    {
        levelID = SceneManager.GetActiveScene().buildIndex;

        persistentData.levelChecks[levelID] = true;
    }
}