using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseAnimeSystem : SystemBase, IOnUpdate
{

    
    public override void SetUp()
    {
        gameStat.turnNum = 1;
        gameStat.turnManger.enemyStartSetPhase += getCHeckMyStartSetPhase;
        gameStat.turnManger.enemyStartMovePhase += getCHeckMyStartMovePhase;
        gameStat.turnManger.resetCicle+= getCheckResetTurnNum;
    }
    public void OnUpdate()
    {
        //if (!gameStat.isMyPhase) return;
        //自分のSetターンが終わったらい相手のセットターンを始めるようにTurmmanagerに伝える
        if (gameStat.isAtackFirst)
        {
            if ((!gameStat.isMySetPhase)&&(gameStat.turnNum == 1))
            {
                gameStat.turnNum++;
                gameStat.turnManger.EnemyStartSetPhase();
            }
            if ((!gameStat.isMyMovePhase) && (gameStat.turnNum == 3))
            {
                gameStat.turnNum++;
                gameStat.turnManger.EnemyStartMovePhase();
            }
        }
        else
        {
            if((!gameStat.isMySetPhase)&&(gameStat.turnNum == 2))
            {
                gameStat.turnNum++;
                gameStat.turnManger.EnemyStartMovePhase();
            }
            if ((!gameStat.isMyMovePhase)&& (gameStat.turnNum == 4))
            {
                //わんサイクル終了
                gameStat.turnNum = 1;
                gameStat.isMySetPhase = true;
                gameStat.isAtackFirst = true;
            }
        }
       

        //自分のMoveターンが終わったらい相手のセットターンを始めるようにTurmmanagerに伝える
        if (!gameStat.isMyMovePhase)
        {
            gameStat.turnManger.EnemyStartMovePhase();
        }
        
    }


    //アニメーションやテロップなどを動かす処理をかく
    private void StartSetBlockPhase()
    {
        Debug.Log("SetBlockターンをかいし");
    }
    //アニメーションやテロップなどを動かす処理をかく
    private void StartMovePhase()
    {
        Debug.Log("Moveターンを開始");
    }
    private void PhaseEnd()
    {

    }

    private void getCHeckMyStartSetPhase()
    {
        gameStat.turnNum++;
        gameStat.isMySetPhase = true;
        StartSetBlockPhase();
    }

    private void getCHeckMyStartMovePhase()
    {
        gameStat.turnNum++;
        gameStat.isMyMovePhase = true;
        StartMovePhase();
    }

    private void getCheckResetTurnNum()
    {
        gameStat.turnNum = 1;
    }
}
