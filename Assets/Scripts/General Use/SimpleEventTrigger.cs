using UnityEngine;
using UnityEngine.Events;

public class SimpleEventTrigger : MonoBehaviour
{
    public UnityEvent onTrigger;

    public void Trigger()
    {
        onTrigger.Invoke();
    }
}