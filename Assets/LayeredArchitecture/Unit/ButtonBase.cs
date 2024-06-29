using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;
public class ButtonBase :MonoBehaviour
{
    public event Action pointerDownEvent;
    public event Action pointerUpEvent;
    public event Action pointerEnterEvent;
    public event Action pointerExitEvent;
    [System.NonSerialized]
    public Image buttonImage;
    [System.NonSerialized]
    public EventTrigger buttonEventTrigger;

    public void ButtonInit()
    {
        buttonImage = GetComponent<Image>();
        buttonEventTrigger = GetComponent<EventTrigger>();
    }
    public void PointerDownEvent()
    {
        if (pointerDownEvent == null) return;
        pointerDownEvent?.Invoke();
    }
    public void PointerUpEvent()
    {
        if (pointerUpEvent == null) return;
        pointerUpEvent?.Invoke();
    }
    public void PointerEnterEvent()
    {
        if (pointerEnterEvent == null) return;
        pointerEnterEvent?.Invoke();
    }
    public void PointerExitEvent()
    {
        if (pointerExitEvent == null) return;
        pointerExitEvent?.Invoke();
    }

}
