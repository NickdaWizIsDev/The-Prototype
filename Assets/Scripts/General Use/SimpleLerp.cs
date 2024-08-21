using Cinemachine;
using System.Collections;
using UnityEditor.PackageManager;
using UnityEngine;

namespace Assets.Scripts.General_Use
{
    public abstract class SimpleLerp : MonoBehaviour
    {
        public static IEnumerator LerpWithTime(float duration)
        {
            var timePassed = 0f;
            float value = 0f;
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
                value = Mathf.Lerp(3, 0, factor);

                // This tells Unity to "pause" the routine here
                // render this frame and continue from here in the next one
                yield return value;

                // increase by the time passed since last frame
                timePassed += Time.deltaTime;
            }
        }
    }
}