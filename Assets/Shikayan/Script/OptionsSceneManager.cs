using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
public class OptionsSceneManager : MonoBehaviour
{
    [SerializeField]
    private string sceneName;

    EventTrigger optionsButtonEventTrigger;

    private event Action<string> pointerDownEvent;

    private void Start()
    {
        SelectPanelInit();
    }

    private void SelectPanelInit()
    {
        optionsButtonEventTrigger = this.transform.GetComponent<EventTrigger>();

        //panel.ButtonInit();
        TriggerInsert(EventTriggerType.PointerDown).callback.AddListener((eventDate) => { PointerDownEvent(); });

        pointerDownEvent += SceneChange;
        //panel.pointerDownEvent += () => Debug.Log(_index);
    }

    private void PointerDownEvent()
    {
        if (pointerDownEvent == null) return;
        pointerDownEvent?.Invoke(sceneName);
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
}
