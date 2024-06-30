using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class ResultButtonBase : MonoBehaviour
{
    [SerializeField]
    private string sceneName;

    [SerializeField]
    private GameObject isSelectUI;

    EventTrigger optionsButtonEventTrigger;

    private event Action<string> pointerDownEvent;
    private event Action pointerEnterEvent;
    private event Action pointerExitEvent;

    private void Start()
    {
        SelectPanelInit();
    }

    private void SelectPanelInit()
    {
        optionsButtonEventTrigger = this.transform.GetComponent<EventTrigger>();

        //panel.ButtonInit();
        TriggerInsert(EventTriggerType.PointerDown).callback.AddListener((eventDate) => { PointerDownEvent(); });
        TriggerInsert(EventTriggerType.PointerEnter).callback.AddListener((eventDate) => { PointerEnterEvent(); });
        TriggerInsert(EventTriggerType.PointerExit).callback.AddListener((eventDate) => { PointerExitEvent(); });

        pointerDownEvent += SceneChange;
        pointerEnterEvent += ActiveChange;
        pointerExitEvent += ActiveChange;
        //panel.pointerDownEvent += () => Debug.Log(_index);
    }

    private void PointerDownEvent()
    {
        if (pointerDownEvent == null) return;
        pointerDownEvent?.Invoke(sceneName);
    }

    private void PointerEnterEvent()
    {
        if (pointerEnterEvent == null) return;
        pointerEnterEvent?.Invoke();
    }

    private void PointerExitEvent()
    {
        if (pointerExitEvent == null) return;
        pointerExitEvent?.Invoke();
    }

    private Entry TriggerInsert(EventTriggerType _eventTriggerType)
    {
        Entry entryTrigger = new Entry();
        entryTrigger.eventID = _eventTriggerType;
        optionsButtonEventTrigger.triggers.Add(entryTrigger);

        return entryTrigger;
    }

    private void SceneChange(string _sceneName)
    {
        SceneManager.LoadScene(_sceneName);
    }

    private void ActiveChange()
    { 
        isSelectUI.SetActive(!isSelectUI.activeSelf);
    }
}
