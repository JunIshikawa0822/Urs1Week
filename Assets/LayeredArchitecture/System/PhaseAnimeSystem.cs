using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhaseAnimeSystem : SystemBase, IOnUpdate
{


    public override void SetUp()
    {
        gameStat.turnNum = 1;
        gameStat.turnManger.enemyStartSetPhase += getCHeckMyStartSetPhase;
        gameStat.turnManger.enemyStartMovePhase += getCHeckMyStartMovePhase;
        gameStat.turnManger.resetCicle += getCheckResetTurnNum;
        gameStat.turnManger.result += getResultCheck;
        

    }
    public void OnUpdate()
    {
        
        //if (gameStat.isMatchOk && (!gameStat.isFirstUI))
        if(gameStat.isMatchOk&&gameStat.isFirstUI)
        {
            Debug.Log("入った");
            gameStat.isFirstUI = false;
            if (gameStat.isMaster)
            {
                gameStat.nowPhaseText.text = "SetPhase";
                StartSetBlockPhase();
            }
            else
            {
                gameStat.nowPhaseText.text = "EnemyPhase";
                EnenmyPhaseUI();
            }
        }


        if (gameStat.isPlayerGoal)
        {
            gameStat.isPlayerGoal = false;
            GoalAnime(true);
            gameStat.turnManger.ResultCheckGoal();

        }
        else
        {
            //自分のSetターンが終わったらい相手のセットターンを始めるようにTurmmanagerに伝える
            if (gameStat.isAtackFirst)
            {
                if ((!gameStat.isMySetPhase) && (gameStat.turnNum == 1))
                {
                    Debug.Log("さいしよ");
                    gameStat.nowPhaseText.text = "EnemyPhase";
                    gameStat.turnNum++;
                    EnenmyPhaseUI();
                    gameStat.turnManger.EnemyStartSetPhase();


                }
                if ((!gameStat.isMyMovePhase) && (gameStat.turnNum == 3))
                {
                    gameStat.nowPhaseText.text = "EnemyPhase";
                    gameStat.turnNum++;
                    EnenmyPhaseUI();
                    gameStat.turnManger.EnemyStartMovePhase();

                }
            }
            else
            {
                if ((!gameStat.isMySetPhase) && (gameStat.turnNum == 2))
                {
                    gameStat.nowPhaseText.text = "EnemyPhase";
                    gameStat.turnNum++;
                    EnenmyPhaseUI();
                    gameStat.turnManger.EnemyStartMovePhase();

                }
                if ((!gameStat.isMyMovePhase) && (gameStat.turnNum == 4))
                {
                    //わんサイクル終了
                    gameStat.turnNum = 1;
                    gameStat.isMySetPhase = true;
                    gameStat.isMyPhase = true;
                    gameStat.isAtackFirst = true;
                    gameStat.isMySetPhaseInitialized = false;
                    StartSetBlockPhase();
                    gameStat.turnManger.ResetTurenNum();
                    StartSetBlockPhase();
                }
            }
        }
       
        

       


    }


    //アニメーションやテロップなどを動かす処理をかく
    private void StartSetBlockPhase()
    {
        gameStat.nowPhaseText.text = "SetPhase";
        //GameObject obj = GameObject.Instantiate(gameStat.mySetFhaseUI, gameStat.canvas.transform.position, Quaternion.identity);
        GameObject obj = GameObject.Instantiate(gameStat.mySetFhaseUI, gameStat.mySetFhaseUI.transform.position, Quaternion.identity);
        obj.transform.SetParent(gameStat.canvas.transform, false);
        //obj.transform.parent = gameStat.canvas.transform;
        Debug.Log("SetBlockターンをかいし");
    }
    //アニメーションやテロップなどを動かす処理をかく
    private void StartMovePhase()
    {
        gameStat.nowPhaseText.text = "MovePhase";
        //GameObject obj = GameObject.Instantiate(gameStat.myMoveFhaseUI, gameStat.canvas.transform.position, Quaternion.identity);
        GameObject obj = GameObject.Instantiate(gameStat.myMoveFhaseUI, gameStat.myMoveFhaseUI.transform.position, Quaternion.identity);
        obj.transform.SetParent(gameStat.canvas.transform, false);
        //obj.transform.parent = gameStat.canvas.transform;
        Debug.Log("Moveターンを開始");
    }
    private void EnenmyPhaseUI()
    {
        //GameObject obj = GameObject.Instantiate(gameStat.enemyFhaseUI, gameStat.canvas.transform.position, Quaternion.identity);
        GameObject obj = GameObject.Instantiate(gameStat.enemyFhaseUI, gameStat.enemyFhaseUI.transform.position, Quaternion.identity);
        obj.transform.SetParent(gameStat.canvas.transform, false);
        //obj.transform.parent = gameStat.canvas.transform;
    }


   

    private void GoalAnime(bool _isWinPlayer)
    {
        if (_isWinPlayer)
        {
            //GameObject obj = GameObject.Instantiate(gameStat.winUI, gameStat.canvas.transform.position, Quaternion.identity);
            GameObject obj = GameObject.Instantiate(gameStat.winUI,gameStat.winUI.transform.position, Quaternion.identity);
            obj.transform.SetParent(gameStat.canvas.transform, false);
            //obj.transform.parent = gameStat.canvas.transform;
            Debug.Log("あなたの勝利");
        }
        else
        {
            //GameObject obj = GameObject.Instantiate(gameStat.loseUI, gameStat.canvas.transform.position, Quaternion.identity);
            GameObject obj = GameObject.Instantiate(gameStat.loseUI, gameStat.loseUI.transform.position, Quaternion.identity);
            obj.transform.SetParent(gameStat.canvas.transform, false);
            //obj.transform.parent = gameStat.canvas.transform;
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

    private void getResultCheck()
    {
        GoalAnime(false);
    }
   
}