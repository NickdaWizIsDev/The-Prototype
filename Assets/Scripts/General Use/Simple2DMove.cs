using System;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.General_Use
{
    public class Simple2DMove : MonoBehaviour
    {
        public bool alreadyMoving;
        public bool doReturn;
        public float velocity;

        public void Move(GameObject obj)
        {
            StartCoroutine(MoveToTargetSmooth(obj.transform.position));
        }

        public IEnumerator MoveToTargetSmooth(Vector3 targetPos)
        {
            // block concurrent routines
            if (alreadyMoving) yield break;

            alreadyMoving = true;

            if (velocity <= 0)
            {
                Debug.LogError($"{nameof(velocity)} may not be negative or 0", this);
                // Allow the next routine to start now
                alreadyMoving = false;
                yield break;
            }

            // pre-cache the initial position
            var startPos = transform.position;

            // using the given average velocity calculate how long the animation
            // shall take in total
            var distance = Vector3.Distance(startPos, targetPos);

            if (Mathf.Approximately(distance, 0))
            {
                Debug.LogWarning("Start and end position are equal!", this);
                // Allow the next routine to start now
                alreadyMoving = false;
                yield break;
            }

            var duration = distance / velocity;

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
                transform.position = Vector3.Lerp(startPos, targetPos, factor);

                // This tells Unity to "pause" the routine here
                // render this frame and continue from here in the next one
                yield return null;

                // increase by the time passed since last frame
                timePassed += Time.deltaTime;
            }

            // just to be sure to end with clean values
            transform.position = targetPos;

            // Allow the next routine to start now
            alreadyMoving = false;

            if (doReturn) { Return(startPos); }
        }

        void Return(Vector3 targetPos)
        {
            doReturn = false;
            StartCoroutine(MoveToTargetSmooth(targetPos));
        }
    }
}