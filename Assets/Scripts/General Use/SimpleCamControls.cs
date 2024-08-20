using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleCamControls : MonoBehaviour
{
    public CinemachineVirtualCamera vcam;


    public void Shake(float shakeDuration)
    {
        StartCoroutine(CamShake(shakeDuration));
    }
    public IEnumerator CamShake(float duration)
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
}
