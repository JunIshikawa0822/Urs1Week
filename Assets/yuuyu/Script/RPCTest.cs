using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class RPCTest : MonoBehaviourPunCallbacks
{
    bool ids;
    [ContextMenu("RPC")]

    public void RPCt()
    {
        photonView.RPC("RpcSendMessage", RpcTarget.Others, "こんにちは");
    }


    [PunRPC]
    private void RpcSendMessage(string message)
    {
        Debug.Log(message);
        ids = true;
    }
}
