using UnityEngine;
using UnityEngine.Audio;

public class DataManager : MonoBehaviour
{
    // Singleton instance
    public static DataManager Instance { get; private set; }

    // Data to persist
    public float volume = 1.0f;
    public bool hasSeenIntro = false;
    public AudioMixer mixer;

    private void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        mixer.SetFloat("Volume", volume);
    }
}
