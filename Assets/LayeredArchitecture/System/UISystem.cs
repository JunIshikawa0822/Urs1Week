using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEngine.EventSystems.EventTrigger;

public class UISystem : SystemBase,IOnLateUpdate
{
    public override void SetUp()
    {
        for(int i=0;i<gameStat.selectPanelArray.Length;i++)
        {
            SelectPanelInit(gameStat.selectPanelArray[i],i);
        }
    }
    public void OnLateUpdate()
    {

    }

    private void SelectPanelInit(ButtonBase _panel,int _index)
    {
        Debug.Log(_index);
        ButtonBase panel = _panel.GetComponent<ButtonBase>();
        if (panel == null) return;

        panel.ButtonInit();

        TriggerInsert(panel,EventTriggerType.PointerDown).callback.AddListener((eventDate)=> { panel.PointerDownEvent(); });
        Debug.Log("確認" + _index);
        panel.pointerDownEvent += () => gameStat.selectedPlacingObjectIndex = _index;
        panel.pointerDownEvent += () => Debug.Log(_index);
        
    }


    private Entry TriggerInsert(ButtonBase _button,EventTriggerType _eventTriggerType)
    {
        Entry entryTrigger = new Entry();
        entryTrigger.eventID = _eventTriggerType;
        _button.buttonEventTrigger.triggers.Add(entryTrigger);
        return entryTrigger;
    }
}
