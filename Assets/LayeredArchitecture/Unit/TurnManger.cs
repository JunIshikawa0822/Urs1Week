using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System;
using UnityEngine.SceneManagement;

public class TurnManger : MonoBehaviourPun
{
    public event Action enemyStartSetPhase;
    public event Action enemyStartMovePhase;
    public event Action resetCicle;

    public event Action result;

  
    public void EnemyStartSetPhase()
    {
        photonView.RPC(nameof(EnemyStartSetPhaseRPC), RpcTarget.Others);

    }
    public void EnemyStartMovePhase()
    {
        photonView.RPC(nameof(EnemyStartMovePhaseRPC), RpcTarget.Others);
    }
    public void ResetTurenNum()
    {
        photonView.RPC(nameof(ResetTurenNumRPC), RpcTarget.Others);
    }
    public void ResultCheckGoal()
    {
        photonView.RPC(nameof(ResultCheckRPC), RpcTarget.Others);
    }
    public void ResultSceneMove()
    {
        
        
        Invoke("MoveScene",2.0f);
    }
    public void MoveScene()
    {
        SceneManager.LoadScene("ResultScene");
        PhotonNetwork.Disconnect();
    }


    [PunRPC]
    public void EnemyStartSetPhaseRPC()
    {
        Debug.Log("呼ばれた");
        enemyStartSetPhase.Invoke();
        Debug.Log("呼ばれた2");
    }

    [PunRPC]
    public void EnemyStartMovePhaseRPC()
    {
        Debug.Log("呼ばれた");
        enemyStartMovePhase.Invoke();
        Debug.Log("呼ばれた2");
    }

    [PunRPC]
    public void ResetTurenNumRPC()
    {
        
        resetCicle.Invoke();
       
    }

    [PunRPC]
    public void ResultCheckRPC()
    {
        result.Invoke();
    }
    
}
