using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;
using UnityEngine.EventSystems;
using System;

public class OptionsButtonBase : MonoBehaviour
{
    [SerializeField]
    private GameObject objectUI;

    [SerializeField]
    private GameObject isSelectUI;

    EventTrigger optionsButtonEventTrigger;

    private event Action<GameObject> pointerDownEvent;
    private event Action<GameObject> pointerEnterEvent;
    private event Action<GameObject> pointerExitEvent;

    private void Start()
    {
        SelectPanelInit();
    }

    private void SelectPanelInit()
    {
        optionsButtonEventTrigger = this.transform.GetComponent<EventTrigger>();

        Debug.Log(optionsButtonEventTrigger);

        //panel.ButtonInit();
        TriggerInsert(EventTriggerType.PointerDown).callback.AddListener((eventDate) => { PointerDownEvent(); });
        TriggerInsert(EventTriggerType.PointerEnter).callback.AddListener((eventDate) => { PointerEnterEvent(); });
        TriggerInsert(EventTriggerType.PointerExit).callback.AddListener((eventDate) => { PointerExitEvent(); });

        pointerDownEvent += ActiveChange;
        pointerEnterEvent += ActiveChange;
        pointerExitEvent += ActiveChange;
        //panel.pointerDownEvent += () => Debug.Log(_index);
    }

    private void PointerDownEvent()
    {
        if (pointerDownEvent == null) return;
        pointerDownEvent?.Invoke(objectUI);

        if (isSelectUI.activeSelf != true) return;
        pointerEnterEvent?.Invoke(isSelectUI);
    }

    private void PointerEnterEvent()
    {
        if (pointerEnterEvent == null) return;
        if (isSelectUI.activeSelf != false) return;
        pointerEnterEvent?.Invoke(isSelectUI);
    }

    private void PointerExitEvent()
    {
        if (pointerExitEvent == null) return;
        if (isSelectUI.activeSelf != true) return;
        pointerExitEvent?.Invoke(isSelectUI);
    }

    private Entry TriggerInsert(EventTriggerType _eventTriggerType)
    {
        Entry entryTrigger = new Entry();
        entryTrigger.eventID = _eventTriggerType;
        optionsButtonEventTrigger.triggers.Add(entryTrigger);

        return entryTrigger;
    }

    public void ActiveChange(GameObject _object)
    {
        if (_object == objectUI)
        {
            objectUI.SetActive(!_object.activeSelf);
        }
        else if(_object == isSelectUI)
        {
            isSelectUI.SetActive(!_object.activeSelf);
        }
    }
}
