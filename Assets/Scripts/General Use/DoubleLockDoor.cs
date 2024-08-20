using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DoubleLockDoor : MonoBehaviour
{
    public UnityEvent onOpen, onClose;

    public bool open1;
    public bool open2;

    bool open, closed;

    private void Start()
    {

    }

    public void Open1()
    {
        open1 = true;
        if (open2 && !open) { onOpen.Invoke(); closed = false; open = true; }
    }

    public void Open2()
    {
        open2 = true;
        if (open1 && !open) { onOpen.Invoke(); closed = false; open = true; }
    }

    public void Close1()
    {
        open1 = false;
        if(!closed) { onClose.Invoke(); closed = true; open = false; }
    }

    public void Close2()
    {
        open2 = false;
        if(!closed) { onClose.Invoke(); closed = true; open = false; }
    }
}
