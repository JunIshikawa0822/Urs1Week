using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEngine.EventSystems.EventTrigger;
using UnityEngine.UI;

public class UISystem : SystemBase,IOnLateUpdate
{
    public override void SetUp()
    {
        for(int i=0;i<gameStat.selectPanelArray.Length;i++)
        {
            SelectPanelInit(gameStat.selectPanelArray[i],i);
        }
        SetBlosckUI();
    }
    public void OnLateUpdate()
    {
        if (gameStat.isSetRandomBlockUI)
        {
            SetBlosckUI();
            gameStat.isSetRandomBlockUI = false;
        }
        if (gameStat.isSetProgramView)
        {
            SetMyprogramListUI();
            gameStat.isSetProgramView = false;
        }
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

    private void SetBlosckUI()
    {
        for(int i=0;i< gameStat.objectOptionsIndexArray.Length; i++)
        {
            gameStat.selsectImageArray[i].sprite = gameStat.objectImageAllPrefabsArray[gameStat.objectOptionsIndexArray[i]];
            // gameStat.selectedPlacingObjectIndex
        }
    }

    private void SetMyprogramListUI()
    {
        foreach (Transform child in gameStat.scrorViewContent.transform)
        {
            child.GetComponent<ScrolViewImage>().DestroyThis();
        }

        for (int i = 0; i < gameStat.programList.Count; i++)
        {
            GameObject programImage = GameObject.Instantiate(gameStat.programViewImage, gameStat.programViewImage.transform.position, Quaternion.identity);
            programImage.transform.parent = gameStat.scrorViewContent.transform;
            programImage.GetComponent<Image>().sprite = gameStat.objectImageAllPrefabsArray[gameStat.programList[i]];
        }
       
    }
}
