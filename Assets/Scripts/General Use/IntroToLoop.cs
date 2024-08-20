using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroToLoop : MonoBehaviour
{
    AudioSource m_AudioSource;
    float m_Duration;
    float timer;

    public GameObject intro;
    public GameObject loop;

    private void Awake()
    {
        m_AudioSource = intro.GetComponent<AudioSource>();
    }

    // Start is called before the first frame update
    void Start()
    {
        m_Duration = m_AudioSource.clip.length;
        intro.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= m_Duration) { loop.SetActive(true); intro.SetActive(false); }
    }
}
