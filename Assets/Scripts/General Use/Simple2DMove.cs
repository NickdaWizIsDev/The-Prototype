using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
public class Simple2DMove : MonoBehaviour
{
    public bool alreadyMoving;
    public float velocity;
    public UnityEvent onComplete;
    public float durationForUIMove;
    public float Velocity { get { return velocity; } set { velocity = value; } }
    public float DurationForUIMove { get { return durationForUIMove; } set { durationForUIMove = value; } }

    public void MoveSmooth(GameObject obj)
    {
        StartCoroutine(MoveToTargetSmooth(obj.transform.position, true));
    }

    public void MoveLinear(GameObject obj)
    {
        StartCoroutine(MoveToTargetSmooth(obj.transform.position, false));
    }

    public IEnumerator MoveToTargetSmooth(Vector3 targetPos, bool smooth)
    {
        // block concurrent routines
        if (alreadyMoving) yield break;

        alreadyMoving = true;

        if (Velocity <= 0)
        {
            Debug.LogError($"{nameof(Velocity)} may not be negative or 0", this);
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

        var duration = distance / Velocity;

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

            //factor = Mathf.SmoothStep(0, 1, factor);

            //Ease in out, but smoother
            if (smooth) factor = factor * factor * factor * (factor * (6f * factor - 15f) + 10f);

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
        onComplete.Invoke();
    }

    public void UIMoveSmooth(RectTransform targetPos)
    {
        CanvasRenderer obj = GetComponent<CanvasRenderer>();

        obj.GetComponent<RectTransform>().DOAnchorPos(targetPos.anchoredPosition, durationForUIMove);
        Invoke(nameof(OnComplete), durationForUIMove);
    }
    void OnComplete()
    {
        onComplete.Invoke();
    }
}