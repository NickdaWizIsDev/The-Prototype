using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;
public class SimpleInterpolatedLightSwitch : MonoBehaviour
{
    private Light2D myLight;

    public float onIntensity, offIntensity;

    private void Awake()
    {
        myLight = GetComponent<Light2D>();
    }

    public void SwitchOn()
    {
        StartCoroutine(Switch(myLight.intensity, onIntensity, 1));
    }
    public void SwitchOff()
    {
        StopAllCoroutines();
        StartCoroutine(Switch(myLight.intensity, offIntensity, 0.5f));
    }

    public IEnumerator Switch(float from, float to, float duration)
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

            //smootherstep math
            factor = factor * factor * factor * (factor * (6f * factor - 15f) + 10f);

            myLight.intensity = Mathf.Lerp(from, to, factor);

            // increase by the time passed since last frame
            timePassed += Time.deltaTime;

            // This tells Unity to "pause" the routine here
            // render this frame and continue from here in the next one
            yield return null;
        }
        myLight.intensity = to;
    }
}