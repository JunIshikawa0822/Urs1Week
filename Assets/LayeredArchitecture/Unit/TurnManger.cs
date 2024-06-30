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
        photonView.RPC(nameof(EnemyStartSetPhaseRPC), RpcTarget.Others);

    }

    [PunRPC]
    public void EnemyStartSetPhaseRPC()
    {
        Debug.Log("呼ばれた");
        enemyStartSetPhase.Invoke();
        Debug.Log("呼ばれた2");
    }
}
