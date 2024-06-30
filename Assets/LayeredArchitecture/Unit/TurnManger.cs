using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System;

public class TurnManger : MonoBehaviourPunCallbacks
{
    public event Action enemyStartSetPhase;
    public event Action enemyStartMovePhase;
    public event Action resetCicle;

    [ContextMenu("やる")]
    public void EnemyStartSetPhase()
    {
        Debug.Log("こっちyよべた");
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
}
