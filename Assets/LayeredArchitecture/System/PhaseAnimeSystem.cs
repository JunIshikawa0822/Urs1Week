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
        if (gameStat.isMatchOk && (!gameStat.isFirstUI))
        {
            gameStat.isFirstUI = true;
            if (gameStat.isMaster) { StartSetBlockPhase(); }
            else { EnenmyPhaseUI(); }

        }
        //if (!gameStat.isMyPhase) return;
        //自分のSetターンが終わったらい相手のセットターンを始めるようにTurmmanagerに伝える
        if (gameStat.isAtackFirst)
        {
            if ((!gameStat.isMySetPhase)&&(gameStat.turnNum == 1))
            {
                Debug.Log("さいしよ");
                gameStat.turnNum++;
                EnenmyPhaseUI();
                gameStat.turnManger.EnemyStartSetPhase();
            }
            if ((!gameStat.isMyMovePhase) && (gameStat.turnNum == 3))
            {
                
                gameStat.turnNum++;
                EnenmyPhaseUI();
                gameStat.turnManger.EnemyStartMovePhase();
            }
        }
        else
        {
            if((!gameStat.isMySetPhase)&&(gameStat.turnNum == 2))
            {
                
                gameStat.turnNum++;
                EnenmyPhaseUI();
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
                StartSetBlockPhase();
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
        GameObject obj = GameObject.Instantiate(gameStat.mySetPhaseUI, gameStat.mySetPhaseUI.transform.position, Quaternion.identity);
        obj.transform.parent = gameStat.canvas.transform;
        Debug.Log("SetBlockターンをかいし");
    }
    //アニメーションやテロップなどを動かす処理をかく
    private void StartMovePhase()
    {
        GameObject obj = GameObject.Instantiate(gameStat.myMovePhaseUI, gameStat.myMovePhaseUI.transform.position, Quaternion.identity);
        obj.transform.parent = gameStat.canvas.transform;
        Debug.Log("Moveターンを開始");
    }
    private void EnenmyPhaseUI()
    {
        GameObject obj = GameObject.Instantiate(gameStat.enemyTurenUI, gameStat.enemyTurenUI.transform.position, Quaternion.identity);
        obj.transform.parent = gameStat.canvas.transform;
    }


    private void GoalAnime(bool _isWinPlayer)
    {
        if (_isWinPlayer)
        {
            GameObject obj = GameObject.Instantiate(gameStat.winUI, gameStat.winUI.transform.position, Quaternion.identity);
            obj.transform.parent = gameStat.canvas.transform;
            Debug.Log("あなたの勝利");
        }
        else
        {
            GameObject obj = GameObject.Instantiate(gameStat.loseUI, gameStat.loseUI.transform.position, Quaternion.identity);
            obj.transform.parent = gameStat.canvas.transform;
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
        EnenmyPhaseUI();
    }

    private void getResultCheck(bool _isWinPlayer)
    {
        GoalAnime(_isWinPlayer);
    }
}
