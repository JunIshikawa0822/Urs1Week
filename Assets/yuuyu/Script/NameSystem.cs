using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class NameSystem :MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI myNameText;
    [SerializeField] private TextMeshProUGUI enemyNameText;
    [SerializeField] private TextMeshProUGUI inputFieldText;


    [ContextMenu("InputName")]
    public void InputMyName()
    {
        PhotonNetwork.NickName = inputFieldText.text;
        print(PhotonNetwork.NickName);
    }


    [ContextMenu("SetMy")]
    public void SetMyName()
    {
        myNameText.text = PhotonNetwork.NickName;
    }
    [ContextMenu("SetEnemy")]
    public void EnemyMyName()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            
            enemyNameText.text = PhotonNetwork.PlayerList[1].NickName;
        }
        else
        {
            enemyNameText.text = PhotonNetwork.PlayerList[0].NickName;
        }

    }
}
