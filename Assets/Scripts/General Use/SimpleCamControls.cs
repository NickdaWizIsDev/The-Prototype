using Cinemachine;
using Cinemachine.PostFX;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class SimpleCamControls : MonoBehaviour
{
    public CinemachineVirtualCamera vcam;
    public CinemachineVolumeSettings volumeSettings;
    public ChromaticAberration a;
    public float startAberration;
    public float startAmpGain;
    float orthoSize;

    private void Start()
    {
        foreach (VolumeComponent volumeComponent in volumeSettings.m_Profile.components)
        {
            if (volumeComponent.name == "ChromaticAberration")
            {
                a = volumeComponent as ChromaticAberration;
                a.intensity.value = startAberration;
            }
        }
    }

    public void ZoomChangeOrtho(float targetFOV)
    {
        if (vcam != null) { StartCoroutine(CamZoom()); }

        IEnumerator CamZoom()
        {
            orthoSize = vcam.m_Lens.OrthographicSize;
            var timePassed = 0f;
            var duration = 1f;
            while (timePassed < duration)
            {
                // This factor moves linear from 0 to 1
                var factor = timePassed / duration;
                // This adds ease-in and ease-out 
                // see https://docs.unity3d.com/ScriptReference/Mathf.SmoothStep.html
                // Basically you can use ANY mathematical function that maps
                // the input of [0; 1] again to a range of [0;1] 
                // with the easing you like
                factor = Mathf.SmoothStep(0, 1, factor);

                // And this is how finally you use Lerp in this case
                vcam.m_Lens.OrthographicSize = Mathf.Lerp(orthoSize, targetFOV, factor);

                // This tells Unity to "pause" the routine here
                // render this frame and continue from here in the next one
                yield return null;

                // increase by the time passed since last frame
                timePassed += Time.deltaTime;
            }
        }
    }

    public void ChangeTarget(GameObject target)
    {
        if (vcam != null) { vcam.Follow = target.transform; }
    }
    public void Shake(float shakeDuration)
    {
        StartCoroutine(CamShake(shakeDuration));
    }
    IEnumerator CamShake(float duration)
    {
        var timePassed = 0f;
        while (timePassed < duration)
        {
            // This factor moves linear from 0 to 1
            var factor = timePassed / duration;
            // This adds ease-in and ease-out 
            // see https://docs.unity3d.com/ScriptReference/Mathf.SmoothStep.html
            // Basically you can use ANY mathematical function that maps
            // the input of [0; 1] again to a range of [0;1] 
            // with the easing you like
            factor = Mathf.SmoothStep(0, 1, factor);

            // And this is how finally you use Lerp in this case
            vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = Mathf.Lerp(2, 0, factor);

            // This tells Unity to "pause" the routine here
            // render this frame and continue from here in the next one
            yield return null;

            // increase by the time passed since last frame
            timePassed += Time.deltaTime;
        }
    }
    public void NoShake(float time)
    {
        StartCoroutine(UnShake(time));
    }

    IEnumerator UnShake(float duration)
    {
        var timePassed = 0f;
        while (timePassed < duration)
        {
            // This factor moves linear from 0 to 1
            var factor = timePassed / duration;
            // This adds ease-in and ease-out 
            // see https://docs.unity3d.com/ScriptReference/Mathf.SmoothStep.html
            // Basically you can use ANY mathematical function that maps
            // the input of [0; 1] again to a range of [0;1] 
            // with the easing you like
            factor = Mathf.SmoothStep(0, 1, factor);

            // And this is how finally you use Lerp in this case
            float amp = vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain;
            vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = Mathf.Lerp(amp, 0, factor);

            // This tells Unity to "pause" the routine here
            // render this frame and continue from here in the next one
            yield return null;

            // increase by the time passed since last frame
            timePassed += Time.deltaTime;
        }
    }

    public void LerpChromaticAberration(float aberrationTime)
    {
        foreach (VolumeComponent volumeComponent in volumeSettings.m_Profile.components)
        {
            if (volumeComponent.name == "ChromaticAberration")
            {
                a = volumeComponent as ChromaticAberration;
                a.intensity.value = 1;
                StartCoroutine(Aberration(aberrationTime));
            }
        }
    }

    IEnumerator Aberration(float duration)
    {
        var timePassed = 0f;
        while (timePassed < duration)
        {
            // This factor moves linear from 0 to 1
            var factor = timePassed / duration;
            // This adds ease-in and ease-out 
            // see https://docs.unity3d.com/ScriptReference/Mathf.SmoothStep.html
            // Basically you can use ANY mathematical function that maps
            // the input of [0; 1] again to a range of [0;1] 
            // with the easing you like
            factor = Mathf.SmoothStep(0, 1, factor);

            // And this is how finally you use Lerp in this case
            a.intensity.value = Mathf.Lerp(1, 0, factor);

            // This tells Unity to "pause" the routine here
            // render this frame and continue from here in the next one
            yield return null;

            // increase by the time passed since last frame
            timePassed += Time.deltaTime;
        }
    }
}
