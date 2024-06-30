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

    [ContextMenu("やる")]
    public void EnemyStartSetPhase()
    {
        Debug.Log("こっちyよべた");
        photonView.RPC("EnemyStartSetPhaseRPC", RpcTarget.Others,"呼んだよ");

    }

    [PunRPC]
    public void EnemyStartSetPhaseRPC(string message)
    {
        Debug.Log(message);
        enemyStartMovePhase.Invoke();
    }
}
