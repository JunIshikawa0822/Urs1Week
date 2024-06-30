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
        gameStat.turnManger.result += getResultCheck;
    }
    public void OnUpdate()
    {
        
        //if (!gameStat.isMyPhase) return;
        //自分のSetターンが終わったらい相手のセットターンを始めるようにTurmmanagerに伝える
        if (gameStat.isAtackFirst)
        {
            if ((!gameStat.isMySetPhase)&&(gameStat.turnNum == 1))
            {
                Debug.Log("さいしよ");
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
                gameStat.isMyPhase = true;
                gameStat.isAtackFirst = true;
                gameStat.isMySetPhaseInitialized = false;
                gameStat.turnManger.ResetTurenNum();
            }
        }
       
        if(gameStat.isPlayerGoal)
        {
            GoalAnime(true);
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

    private void GoalAnime(bool _isWinPlayer)
    {
        if (_isWinPlayer)
        {
            Debug.Log("あなたの勝利");
        }
        else
        {
            Debug.Log("あなたの負け");
        }
        
    }
    

    private void getCHeckMyStartSetPhase()
    {
        gameStat.turnNum++;
        gameStat.isMyPhase = true;
        gameStat.isMySetPhase = true;
        StartSetBlockPhase();
    }

    private void getCHeckMyStartMovePhase()
    {
        gameStat.turnNum++;
        gameStat.isMyPhase = true;
        gameStat.isMyMovePhase = true;
        StartMovePhase();
    }

    private void getCheckResetTurnNum()
    {
        gameStat.turnNum = 1;
        gameStat.isMyPhase = false;
        gameStat.isMySetPhase = false;
        gameStat.isAtackFirst = false;
        gameStat.isMySetPhaseInitialized = false;
    }

    private void getResultCheck(bool _isWinPlayer)
    {
        GoalAnime(_isWinPlayer);
    }
}
