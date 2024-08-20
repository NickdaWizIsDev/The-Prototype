using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SimpleDoor : MonoBehaviour
{
    public UnityEvent onOpen, onClose;
    public bool startOpen;

    bool open, closed;

    private void Start()
    {
        if(!startOpen) closed = true;
    }

    public void Open()
    {
        if(!open) { onOpen.Invoke(); open = true; closed = false; }
    }

    public void Close()
    {
        if (!closed) { onClose.Invoke(); closed = true; open = false; }
    }
}
