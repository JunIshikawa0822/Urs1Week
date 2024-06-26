using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Tilemaps;
using System.Drawing;
using System.Collections.Concurrent;
using Photon.Pun;
using Photon.Realtime;

public class Player : MonoBehaviourPun, IPunInstantiateMagicCallback
{
    private GridLayout gridLayout;
    private Transform goalPos;

    private bool isGoal;
    
    public Vector3Int playerSize= new Vector3Int(1,1,1);
    private Vector3[] bottomVertices;

    public event Func<Player, string, bool,bool> moveCheckFunc;
    public event Func<Player, bool,bool> jumpMoveCheckFunc;
    public event Func<Player, string, bool,bool> breakCheckFunc;

    public event Func<Vector3, GridLayout, Vector3> convertPosToCellPosFunc;
    public event Func<Player, Transform, bool,bool> goalCheckFunc;

    public event Action<GameObject,bool> breakEvent;
    public event Action damageEvent;
    public event Action movePhaseEnd;

    //test
    private int[] nowProgramArray;

    public void Init(GridLayout gridLayout, Transform _goalPos)
    {
        this.gridLayout = gridLayout;
        this.goalPos = _goalPos;

        GetColliderVertexPositionLoacl();
        CalculateSizeInCells();
    }

    private void GetColliderVertexPositionLoacl()
    {
        BoxCollider boxCollider = gameObject.GetComponent<BoxCollider>();
        bottomVertices = new Vector3[5];
        bottomVertices[0] = boxCollider.center + new Vector3(-boxCollider.size.x, -boxCollider.size.y, -boxCollider.size.z) * 0.5f;
        bottomVertices[1] = boxCollider.center + new Vector3(boxCollider.size.x, -boxCollider.size.y, -boxCollider.size.z) * 0.5f;
        bottomVertices[2] = boxCollider.center + new Vector3(boxCollider.size.x, -boxCollider.size.y, boxCollider.size.z) * 0.5f;
        bottomVertices[3] = boxCollider.center + new Vector3(-boxCollider.size.x, -boxCollider.size.y, boxCollider.size.z) * 0.5f;
        bottomVertices[4] = boxCollider.center + new Vector3(-boxCollider.size.x, boxCollider.size.y, -boxCollider.size.z) * 0.5f;
    }

    private void CalculateSizeInCells()
    {
        Vector3Int[] vertices = new Vector3Int[bottomVertices.Length];

        for (int i = 0; i < vertices.Length; i++)
        {
            Vector3 worldPos = transform.TransformPoint(bottomVertices[i]);
            vertices[i] = gridLayout.WorldToCell(worldPos);
        }

        playerSize = new Vector3Int(Mathf.Abs((vertices[0] - vertices[1]).x), Mathf.Abs((vertices[0] - vertices[3]).y), 1);
    }

    private bool GoalCheckFunc()
    {
        bool isMasterClient = PhotonNetwork.IsMasterClient;
        if (goalCheckFunc == null) return false;
        return goalCheckFunc(this, goalPos,isMasterClient);
    }
    [ContextMenu("前")]
    public void MoveForward()
    {
        
        if(photonView.IsMine)
        {
            bool isMasterClient= PhotonNetwork.IsMasterClient;
            if (moveCheckFunc(this, "Forward", isMasterClient))
            {
                Vector3 posXZ;
                if (isMasterClient)
                {
                    posXZ = convertPosToCellPosFunc(transform.position + new Vector3(0, 0, 1), gridLayout);
                }
                else
                {
                    posXZ = convertPosToCellPosFunc(transform.position - new Vector3(0, 0, 1), gridLayout);
                }
                transform.position = new Vector3(posXZ.x, transform.lossyScale.y / 2, posXZ.z);
            }
            else
            {
                Debug.Log("CantMove");
            }
        }
        
    }
    [ContextMenu("右")]
    public void MoveRight()
    {

        //ForceBackWard();
        if (photonView.IsMine)
        {
            bool isMasterClient = PhotonNetwork.IsMasterClient;
            if (moveCheckFunc(this, "Right",isMasterClient))
            {
                Vector3 posXZ;
                if (isMasterClient)
                {
                    posXZ = convertPosToCellPosFunc(transform.position + new Vector3(1, 0, 0), gridLayout);
                }
                else
                {
                    posXZ = convertPosToCellPosFunc(transform.position - new Vector3(1, 0, 0), gridLayout);
                }
                transform.position = new Vector3(posXZ.x, transform.lossyScale.y / 2, posXZ.z);
            }
            else
            {
                Debug.Log("CantMove");
            }
        }
        
    }
    [ContextMenu("日左")]
    public void MoveLeft()
    {
        if (photonView.IsMine)
        {
            bool isMasterClient = PhotonNetwork.IsMasterClient;
            if (moveCheckFunc(this, "Left",isMasterClient))
            {
                Vector3 posXZ;
                if (isMasterClient)
                {
                   posXZ = convertPosToCellPosFunc(transform.position + new Vector3(-1, 0, 0), gridLayout);
                }
                else
                {
                    posXZ = convertPosToCellPosFunc(transform.position - new Vector3(-1, 0, 0), gridLayout);
                }
                 
                transform.position = new Vector3(posXZ.x, transform.lossyScale.y / 2, posXZ.z);
            }
            else
            {
                Debug.Log("CantMove");
            }
        }
        
    }
    [ContextMenu("後ろ")]
    public void MoveBackward()
    {
        if (photonView.IsMine)
        {
            bool isMasterClient = PhotonNetwork.IsMasterClient;
            if (moveCheckFunc(this, "Backward",isMasterClient))
            {
                Vector3 posXZ;
                if (isMasterClient)
                {
                    posXZ = convertPosToCellPosFunc(transform.position + new Vector3(0, 0, -1), gridLayout);
                }
                else
                {
                    posXZ = convertPosToCellPosFunc(transform.position - new Vector3(0, 0, -1), gridLayout);
                }
                
                transform.position = new Vector3(posXZ.x, transform.lossyScale.y / 2, posXZ.z);
            }
            else
            {
                Debug.Log("CantMove");
            }
        }
        
    }

    public void MoveRightFront()
    {
        if (photonView.IsMine)
        {
            bool isMasterClient = PhotonNetwork.IsMasterClient;
            if (moveCheckFunc(this, "RightFront",isMasterClient))
            {
                Vector3 posXZ;
                if (isMasterClient)
                {
                    posXZ = convertPosToCellPosFunc(transform.position + new Vector3(1, 0, 1), gridLayout);
                }
                else
                {
                    posXZ = convertPosToCellPosFunc(transform.position - new Vector3(1, 0, 1), gridLayout);
                }
                
                transform.position = new Vector3(posXZ.x, transform.lossyScale.y / 2, posXZ.z);
            }
            else
            {
                Debug.Log("CantMove");
            }
        }
        
    }

    public void MoveLeftFront()
    {
        if (photonView.IsMine)
        {
            bool isMasterClient = PhotonNetwork.IsMasterClient;
            if (moveCheckFunc(this, "LeftFront",isMasterClient))
            {
                Vector3 posXZ;
                if (isMasterClient)
                {
                    posXZ = convertPosToCellPosFunc(transform.position + new Vector3(-1, 0, 1), gridLayout);
                }
                else
                {
                    posXZ = convertPosToCellPosFunc(transform.position - new Vector3(-1, 0, 1), gridLayout);
                }
                transform.position = new Vector3(posXZ.x, transform.lossyScale.y / 2, posXZ.z);
            }
            else
            {
                Debug.Log("CantMove");
            }
        }
        
    }

    public void MoveJump()
    {
        if (photonView.IsMine)
        {
            bool isMasterClient = PhotonNetwork.IsMasterClient;
            if (jumpMoveCheckFunc(this,isMasterClient))
            {
                Vector3 posXZ;
                if (isMasterClient)
                {
                    posXZ = convertPosToCellPosFunc(transform.position + new Vector3(0, 0, 2), gridLayout);
                }
                else
                {
                    posXZ = convertPosToCellPosFunc(transform.position - new Vector3(0, 0, 2), gridLayout);
                }
                    transform.position = new Vector3(posXZ.x, transform.lossyScale.y / 2, posXZ.z);
            }
            else
            {
                Debug.Log("CantMove");
            }
        }
        
    }

    public void MoveBreak()
    {
        if (photonView.IsMine)
        {
            bool isMasterClient = PhotonNetwork.IsMasterClient;
            if (breakCheckFunc(this, "Break",isMasterClient))
            {
                if (!Physics.Raycast(this.transform.position, transform.forward, out RaycastHit hitInfo, playerSize.z)) return;

                if (hitInfo.collider.gameObject.CompareTag("PlaceableObject"))
                {
                    if (breakEvent == null) return;
                    breakEvent.Invoke(hitInfo.collider.gameObject,isMasterClient);
                }
                else
                {
                    DamageEvent();
                }
            }
            else
            {
                Debug.Log("CantMove");
            }
        }
        
    }

    public void MoveRightBreak()
    {
        if (photonView.IsMine)
        {
            if (breakCheckFunc(this, "RightBreak",isActiveAndEnabled))
            {
                if (!Physics.Raycast(this.transform.position, transform.right, out RaycastHit hitInfo, playerSize.z)) return;

                if (hitInfo.collider.gameObject.CompareTag("PlaceableObject"))
                {
                    if (breakEvent == null) return;
                    breakEvent.Invoke(hitInfo.collider.gameObject,isActiveAndEnabled);
                }
                else
                {
                    DamageEvent();
                }
            }
            else
            {
                Debug.Log("CantMove");
            }
        }
        
    }

    public void MoveLeftBreak()
    {
        if (photonView.IsMine)
        {
            if (breakCheckFunc(this, "LeftBreak",isActiveAndEnabled))
            {
                if (!Physics.Raycast(this.transform.position, -transform.right, out RaycastHit hitInfo, playerSize.z)) return;

                if (hitInfo.collider.gameObject.CompareTag("PlaceableObject"))
                {
                    if (breakEvent == null) return;
                    breakEvent.Invoke(hitInfo.collider.gameObject,isActiveAndEnabled);
                }
                else
                {
                    DamageEvent();
                }
            }
            else
            {
                Debug.Log("CantMove");
            }
        }
       
    }

    private void DamageEvent()
    {
        if (photonView.IsMine)
        {
            if (damageEvent == null) return;
            damageEvent.Invoke();
        }
        
    }

    public void ForceBackWard()
    {
        if (photonView.IsMine)
        {
            PhotonView targetPhotonView;
            if (photonView.ViewID == 1001)
            {
                Debug.Log("1001です");
                targetPhotonView = PhotonView.Find(2001);
            }
            else if (photonView.ViewID == 2001)
            {
                targetPhotonView = PhotonView.Find(1001);
                Debug.Log("2001です");
            }
            else
            {
                targetPhotonView = photonView;
                Debug.Log("IDわからん");
            }
            // 対象のPhotonView IDを指定
            targetPhotonView.RPC("BackWard", RpcTarget.Others);
        }
    }

    //相手の駒を下げる　p
    [PunRPC]
    public void BackWard()
    {
        MoveBackward();
        Debug.Log("下げられた");
    }
    



    public Vector3Int GetSize
    {
        get { return playerSize; }
    }

    public bool GetIsGoal
    {
        get { return isGoal; }
    }

    public void MoveByProgram(List<int> _program)
    {
        Debug.Log("MoveStart");
        if (_program == null) return;

        nowProgramArray = new int[_program.Count];
        for(int i = 0; i < _program.Count; i++)
        {
            nowProgramArray[i] = _program[i];
        }
        Debug.Log("いまからこのプログラムは {" + string.Join(",", nowProgramArray) + "}");

        //MoveTest();
        StartCoroutine(AnimationWait());
    }

    IEnumerator AnimationWait()
    {
        Debug.Log("OK");
        //Debug.Log(string.Join(", ", nowList));
        //yield return new WaitForSeconds(1.0f);
        
        foreach (int _code in nowProgramArray)
        {
            if (_code == 0)
            {
                MoveForward();
            }
            else if (_code == 1)
            {
                MoveRight();
            }
            else if (_code == 2)
            {
                MoveLeft();
            }
            else if (_code == 3)
            {
                //MoveBackward();
                ForceBackWard();
            }
            else if (_code == 4)
            {
                MoveJump();
            }
            else if (_code == 5)
            {
                MoveBreak();
            }
            else if (_code == 6)
            {
                MoveRightFront();
            }
            else if (_code == 7)
            {
                MoveLeftFront();
            }
            else if (_code == 8)
            {
                MoveRightBreak();
            }
            else if (_code == 9)
            {
                MoveLeftBreak();
            }

            isGoal = GoalCheckFunc();
        

           

            yield return new WaitForSeconds(1.2f);
        }
        movePhaseEnd.Invoke();
        Debug.Log("ターンエンド");
    }

    private void MoveTest()
    {

        foreach (int _code in nowProgramArray)
        {
            if (_code == 0)
            {
                MoveForward();
            }
            else if (_code == 1)
            {
                MoveRight();
            }
            else if (_code == 2)
            {
                MoveLeft();
            }
            else if (_code == 3)
            {
                //MoveBackward();
                ForceBackWard();
                Debug.Log("下げたい");
            }
            else if (_code == 4)
            {
                MoveJump();
            }
            else if (_code == 5)
            {
                MoveBreak();
            }
            else if (_code == 6)
            {
                MoveRightFront();
            }
            else if (_code == 7)
            {
                MoveLeftFront();
            }
            else if (_code == 8)
            {
                MoveRightBreak();
            }
            else if (_code == 9)
            {
                MoveLeftBreak();
            }

            isGoal = GoalCheckFunc();
        }
        //isMovePhaseをfalseにする
        movePhaseEnd.Invoke();
    }
    
    void IPunInstantiateMagicCallback.OnPhotonInstantiate(PhotonMessageInfo info)
    {
        if (info.Sender.IsLocal)
        {
            //Debug.Log("自身がネットワークオブジェクトを生成しました");
            
        }
        else
        {
            //Debug.Log("他プレイヤーがネットワークオブジェクトを生成しました");
        }
    }
    
}
