using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using System.Collections.Generic;
using ExitGames.Client.Photon;

// MonoBehaviourPunCallbacksを継承して、PUNのコールバックを受け取れるようにする
public class PhotonSystem : SystemBase, IConnectionCallbacks, IMatchmakingCallbacks, IInRoomCallbacks
{
   
    public override void SetUp()
    {
        // PhotonServerSettingsの設定内容を使ってマスターサーバーへ接続する
        PhotonNetwork.ConnectUsingSettings();
        // PUNのコールバック対象に登録する
        PhotonNetwork.AddCallbackTarget(this);

    }
    //isFazeEndがとんできてtrueになったかを判断


    // マスターサーバーへの接続が成功した時に呼ばれるコールバック
    void IConnectionCallbacks.OnConnectedToMaster()
    {
        Debug.Log("マスターサーバーへの接続が成功");
        // ランダムなルームに参加する
        PhotonNetwork.JoinRandomRoom();
        // "Room"という名前のルームに参加する（ルームが存在しなければ作成して参加する）
        //PhotonNetwork.JoinOrCreateRoom("Room", new RoomOptions(), TypedLobby.Default);
    }
    // ランダムで参加できるルームが存在しないなら、新規でルームを作成する
    void IMatchmakingCallbacks.OnJoinRandomFailed(short returnCode, string message)
    {
        // ルームの参加人数を2人に設定する
        var roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 2;
        PhotonNetwork.CreateRoom(null, roomOptions);
    }

    // ゲームサーバーへの接続が成功した時に呼ばれるコールバック
    void IMatchmakingCallbacks.OnJoinedRoom()
    {

        // ルームが満員になったら、以降そのルームへの参加を不許可にする
        if (PhotonNetwork.CurrentRoom.PlayerCount == PhotonNetwork.CurrentRoom.MaxPlayers)
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;
            gameStat.isMatchOk = true;
        }


        if (PhotonNetwork.IsMasterClient)
        {
            gameStat.isMaster = true;
            gameStat.isMyPhase = true;
            gameStat.isMySetPhase = true;
        }
        else
        {
            gameStat.isMaster = false;
            gameStat.isMyPhase = false;
            gameStat.isMySetPhase = false;
        }
        gameStat.isInstanitiatePlayerObj = true;
        gameStat.isEnterRoom = true;

        Debug.Log(PhotonNetwork.LocalPlayer.ActorNumber);

        /*
        if (PhotonNetwork.IsMasterClient)
        {
            //gameStat.player = PhotonNetwork.Instantiate("Player", gameStat.player1StartPos.position, Quaternion.identity).GetComponent<Player>();
            //Debug.Log(gameStat.player.playerSize);
            gameStat.isInstanitiatePlayerObj = true;
            Debug.Log("Playerを生成しました");

        }
        else
        {
            //gameStat.player = PhotonNetwork.Instantiate("Player", gameStat.player2StartPos.transform.position, Quaternion.identity).GetComponent<Player>();
            Debug.Log("Playerを生成しました");
        }
        */

    }
    
    public void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        Debug.Log("MatchOk");
        gameStat.isMatchOk = true;
        throw new System.NotImplementedException();
    }
    
    
    // Photonのサーバーから切断された時に呼ばれるコールバック
    void IConnectionCallbacks.OnDisconnected(DisconnectCause cause)
    {
        Debug.Log($"サーバーとの接続が切断されました: {cause.ToString()}");
        // PUNのコールバック対象の登録を解除する
        PhotonNetwork.RemoveCallbackTarget(this);
    }
    
    
    void IConnectionCallbacks.OnConnected() { }
    //void IConnectionCallbacks.OnDisconnected(DisconnectCause cause) { }
    void IConnectionCallbacks.OnRegionListReceived(RegionHandler regionHandler) { }
    void IConnectionCallbacks.OnCustomAuthenticationResponse(Dictionary<string, object> data) { }
    void IConnectionCallbacks.OnCustomAuthenticationFailed(string debugMessage) { }

    void IMatchmakingCallbacks.OnFriendListUpdate(List<FriendInfo> friendList) { }
    void IMatchmakingCallbacks.OnCreatedRoom() { }
    void IMatchmakingCallbacks.OnCreateRoomFailed(short returnCode, string message) { }
    void IMatchmakingCallbacks.OnJoinRoomFailed(short returnCode, string message) { }
    //void IMatchmakingCallbacks.OnJoinRandomFailed(short returnCode, string message) { }
    void IMatchmakingCallbacks.OnLeftRoom() { }

    //public virtual void OnPlayerEnteredRoom(Player newPlayer) { }
    public virtual void OnPlayerLeftRoom(Player otherPlayer) { }
    public virtual void OnJoinRandomFailed(short returnCode, string message) { }
    public virtual void OnConnectedToMaster() { }
    public virtual void OnRoomPropertiesUpdate() { }
    public virtual void OnPlayerPropertiesUpdate() { }
    
    
    
    
    public void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        throw new System.NotImplementedException();
    }

    public void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
    {
        throw new System.NotImplementedException();
    }

    public void OnPlayerPropertiesUpdate(Photon.Realtime.Player targetPlayer, Hashtable changedProps)
    {
        throw new System.NotImplementedException();
    }

    public void OnMasterClientSwitched(Photon.Realtime.Player newMasterClient)
    {
        throw new System.NotImplementedException();
    }
    
    
}