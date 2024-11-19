using UnityEngine;
using DG.Tweening;

public class SimpleAudioControls : MonoBehaviour
{
    public AudioSource source;

    public void FadeOut(float timeToFade)
    {
        source.DOFade(0, timeToFade);
    }

    public void FadeIn(float timeToFade)
    {
        source.DOFade(1, timeToFade);
    }
}