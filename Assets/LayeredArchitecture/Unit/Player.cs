using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Tilemaps;
using System.Drawing;
using Palmmedia.ReportGenerator.Core;
using System.Collections.Concurrent;

public class Player : MonoBehaviour
{
    private TileBase[] detectTilesArray;
    private GridLayout gridLayout;
    private Transform goalPos;

    private int yOffset;
    private bool isGoal;

    private Vector3Int playerSize;
    private Vector3[] Vertices;

    public Func<Player, Vector3Int, TileBase[], bool> canBeMovedCheckFunc;
    public Func<Vector3, GridLayout, Vector3> convertPosToCellPosFunc;
    public Func<Player, Transform, bool> goalCheckFunc;
    public event Action<GameObject> breakEvent;
    public event Action damageEvent;

    //test
    private int[] nowProgramArray;

    public void Init(GridLayout gridLayout, TileBase[] _detectTilesArray, Transform _goalPos)
    {
        this.gridLayout = gridLayout;
        this.detectTilesArray = _detectTilesArray;
        this.goalPos = _goalPos;

        GetColliderVertexPositionLoacl();
        CalculateSizeInCells();
    }

    private void GetColliderVertexPositionLoacl()
    {
        BoxCollider boxCollider = gameObject.GetComponent<BoxCollider>();
        Vertices = new Vector3[5];
        Vertices[0] = boxCollider.center + new Vector3(-boxCollider.size.x, -boxCollider.size.y, -boxCollider.size.z) * 0.5f;
        Vertices[1] = boxCollider.center + new Vector3(boxCollider.size.x, -boxCollider.size.y, -boxCollider.size.z) * 0.5f;
        Vertices[2] = boxCollider.center + new Vector3(boxCollider.size.x, -boxCollider.size.y, boxCollider.size.z) * 0.5f;
        Vertices[3] = boxCollider.center + new Vector3(-boxCollider.size.x, -boxCollider.size.y, boxCollider.size.z) * 0.5f;
        Vertices[4] = boxCollider.center + new Vector3(-boxCollider.size.x, boxCollider.size.y, -boxCollider.size.z) * 0.5f;
    }

    private void CalculateSizeInCells()
    {
        Vector3Int[] vertices = new Vector3Int[Vertices.Length];

        for (int i = 0; i < vertices.Length; i++)
        {
            Vector3 worldPos = transform.TransformPoint(Vertices[i]);
            vertices[i] = BuildingSystem.current.gridLayout.WorldToCell(worldPos);
        }

        playerSize = new Vector3Int(Mathf.Abs((vertices[0] - vertices[1]).x), Mathf.Abs((vertices[0] - vertices[3]).y), 1);
    }

    private bool MoveCheck(string _direction)
    {
        BoundsInt player = new BoundsInt();
        player.position = gridLayout.WorldToCell(transform.position);

        Vector3Int cell;

        if(_direction == "Forward") cell = new Vector3Int(player.position.x, player.position.y + playerSize.z, player.position.z);
        else if(_direction == "Right") cell = new Vector3Int(player.position.x + playerSize.x, player.position.y, player.position.z);
        else if(_direction == "Left") cell = new Vector3Int(player.position.x - playerSize.x, player.position.y, player.position.z);
        else if(_direction == "Backward") cell = new Vector3Int(player.position.x, player.position.y - playerSize.z, player.position.z);
        else if(_direction == "RightFront") cell = new Vector3Int(player.position.x + playerSize.x, player.position.y + playerSize.z, player.position.z);
        else if(_direction == "LeftFront") cell = new Vector3Int(player.position.x - playerSize.x, player.position.y + playerSize.z, player.position.z);
        else cell = new Vector3Int(player.position.x, player.position.y + playerSize.z, player.position.z);

        return canBeMovedCheckFunc(this, cell, detectTilesArray);
    }

    private bool JumpMoveCheck()
    {
        BoundsInt player = new BoundsInt();
        player.position = gridLayout.WorldToCell(transform.position);

        Vector3Int cell1;
        Vector3Int cell2;

        
        cell1 = new Vector3Int(player.position.x, player.position.y + playerSize.z, player.position.z);
        cell2 = new Vector3Int(player.position.x, player.position.y + playerSize.z * 2, player.position.z);

        if(canBeMovedCheckFunc(this, cell1, detectTilesArray) == false && canBeMovedCheckFunc(this, cell2, detectTilesArray) == true)
        {
            return true;
        }
        else
        {
            return false;
        }
        
    }

    private bool BreakCheck()
    {
        BoundsInt player = new BoundsInt();
        player.position = gridLayout.WorldToCell(transform.position);

        Vector3Int cell;

        
        cell = new Vector3Int(player.position.x, player.position.y + playerSize.z, player.position.z);

        if (canBeMovedCheckFunc(this, cell, detectTilesArray) == false)
        {
            return true;
        }
        else
        {
            Debug.Log("壊せるものがなにもないよ！！");
            return false;
        }
        
    }

    private bool GoalCheckFunc()
    {
        if (goalCheckFunc == null) return false;
        return goalCheckFunc(this, goalPos);
    }

    public void MoveForward()
    {
        if (MoveCheck("Forward"))
        {
            Vector3 posXZ = convertPosToCellPosFunc(transform.position + new Vector3(0, 0, 1), gridLayout);
            transform.position = new Vector3(posXZ.x, transform.lossyScale.y / 2, posXZ.z);
            //Debug.Log("Move Forward");
        }
        else
        {
            Debug.Log("CantMove");
        }
    }

    public void MoveRight()
    {
        if (MoveCheck("Right"))
        {
            Vector3 posXZ = convertPosToCellPosFunc(transform.position + new Vector3(1, 0, 0), gridLayout);
            transform.position = new Vector3(posXZ.x, transform.lossyScale.y / 2, posXZ.z);
            //Debug.Log("Move Right");
        }
        else
        {
            Debug.Log("CantMove");
        }
    }

    public void MoveLeft()
    {
        if (MoveCheck("Left"))
        {
            Vector3 posXZ = convertPosToCellPosFunc(transform.position + new Vector3(-1, 0, 0), gridLayout);
            transform.position = new Vector3(posXZ.x, transform.lossyScale.y / 2, posXZ.z);
        }
        else
        {
            Debug.Log("CantMove");
        }
    }

    public void MoveBackward()
    {
        if (MoveCheck("Backward"))
        {
            Vector3 posXZ = convertPosToCellPosFunc(transform.position + new Vector3(0, 0, -1), gridLayout);
            transform.position = new Vector3(posXZ.x, transform.lossyScale.y / 2, posXZ.z);
        }
        else
        {
            Debug.Log("CantMove");
        }
    }

    public void MoveRightFront()
    {
        if (MoveCheck("RightFront"))
        {
            Vector3 posXZ = convertPosToCellPosFunc(transform.position + new Vector3(1, 0, 1), gridLayout);
            transform.position = new Vector3(posXZ.x, transform.lossyScale.y / 2, posXZ.z);
        }
        else
        {
            Debug.Log("CantMove");
        }
    }

    public void MoveLeftFront()
    {
        if (MoveCheck("LeftFront"))
        {
            Vector3 posXZ = convertPosToCellPosFunc(transform.position + new Vector3(1, 0, -1), gridLayout);
            transform.position = new Vector3(posXZ.x, transform.lossyScale.y / 2, posXZ.z);
        }
        else
        {
            Debug.Log("CantMove");
        }
    }

    public void MoveJump()
    {
        if (JumpMoveCheck())
        {
            Vector3 posXZ = convertPosToCellPosFunc(transform.position + new Vector3(0, 0, 2), gridLayout);
            transform.position = new Vector3(posXZ.x, transform.lossyScale.y / 2, posXZ.z);
        }
        else
        {
            Debug.Log("CantMove");
        }
    }

    public void MoveBreak()
    {
        if (BreakCheck())
        {
            if (!Physics.Raycast(this.transform.position, transform.forward, out RaycastHit hitInfo, playerSize.z))return;

            Debug.Log("お、あるねえ");

            if (hitInfo.collider.gameObject.CompareTag("PlaceableObject"))
            {
                if (breakEvent == null) return;
                breakEvent.Invoke(hitInfo.collider.gameObject);
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

        #region LocalMethods

        void DamageEvent()
        {
            if (damageEvent == null) return;
            damageEvent.Invoke();
        }
        #endregion
    }



    public Vector3Int GetSize
    {
        get { return playerSize; }
    }

    public int GetYOffset
    {
        get { return yOffset; }
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

        StartCoroutine(AnimationWait());
    }

    IEnumerator AnimationWait()
    {
        //Debug.Log(string.Join(", ", nowList));
        yield return new WaitForSeconds(1.0f);
        
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
                MoveBackward();
            }
            else if (_code == 4)
            {
                MoveJump();
            }
            else if (_code == 5)
            {
                MoveBreak();
                //Debug.Log(string.Join(", ", nowList));
            }

            isGoal = GoalCheckFunc();

            if (isGoal)
            {
                yield break;
            }

            yield return new WaitForSeconds(2.0f);
        }

        Debug.Log("ターンエンド");
    }
}
