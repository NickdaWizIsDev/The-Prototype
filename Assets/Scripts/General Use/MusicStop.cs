using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class MusicStop : MonoBehaviour
{
    public void StopPlaying(AudioSource audioSource)
    {
        StartCoroutine(VolumeDown(audioSource));
    }

    private IEnumerator VolumeDown(AudioSource audioSource)
    {
        while (audioSource.volume > 0)
        {
            audioSource.volume -= 0.001f;
            yield return null;
        }
    }
}
