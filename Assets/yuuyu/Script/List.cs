using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class List : MonoBehaviourPunCallbacks
{
    List<GameObject> myProgramList = new List<GameObject>();
    List<GameObject> enemyProgramList = new List<GameObject>();



    private void Start()
    {
        //相手のAddEnemyProgramListを呼ぶ
        photonView.RPC(nameof(AddEnemyProgramList), RpcTarget.Others, 1);
        AddMyProgramList(1);
    }


    private void AddMyProgramList(int _num)
    {
        
    }


    [PunRPC]
    private void AddEnemyProgramList(int _num)
    {

    }

}
