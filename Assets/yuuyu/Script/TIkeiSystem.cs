using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaikiSystem : SystemBase, IOnUpdate
{
   
    public void OnUpdate()
    {
        if (gameStat.isMatchOk)
        {
            gameStat.isMatchOk = false;
            gameStat.taikiPanel.SetActive(false);
            gameStat.gamePanel.SetActive(true);
        }
       
    }

}
