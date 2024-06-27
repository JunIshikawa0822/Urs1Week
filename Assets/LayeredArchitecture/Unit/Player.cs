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
    private GridLayout gridLayout;
    private Transform goalPos;

    private bool isGoal;

    private Vector3Int playerSize;
    private Vector3[] bottomVertices;

    public event Func<Player, string, bool> moveCheckFunc;
    public event Func<Player, bool> jumpMoveCheckFunc;
    public event Func<Player, string, bool> breakCheckFunc;

    public event Func<Vector3, GridLayout, Vector3> convertPosToCellPosFunc;
    public event Func<Player, Transform, bool> goalCheckFunc;

    public event Action<GameObject> breakEvent;
    public event Action damageEvent;

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
            vertices[i] = BuildingSystem.current.gridLayout.WorldToCell(worldPos);
        }

        playerSize = new Vector3Int(Mathf.Abs((vertices[0] - vertices[1]).x), Mathf.Abs((vertices[0] - vertices[3]).y), 1);
    }

    private bool GoalCheckFunc()
    {
        if (goalCheckFunc == null) return false;
        return goalCheckFunc(this, goalPos);
    }

    public void MoveForward()
    {
        if (moveCheckFunc(this, "Forward"))
        {
            Vector3 posXZ = convertPosToCellPosFunc(transform.position + new Vector3(0, 0, 1), gridLayout);
            transform.position = new Vector3(posXZ.x, transform.lossyScale.y / 2, posXZ.z);
        }
        else
        {
            Debug.Log("CantMove");
        }
    }

    public void MoveRight()
    {
        if (moveCheckFunc(this, "Right"))
        {
            Vector3 posXZ = convertPosToCellPosFunc(transform.position + new Vector3(1, 0, 0), gridLayout);
            transform.position = new Vector3(posXZ.x, transform.lossyScale.y / 2, posXZ.z);
        }
        else
        {
            Debug.Log("CantMove");
        }
    }

    public void MoveLeft()
    {
        if (moveCheckFunc(this, "Left"))
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
        if (moveCheckFunc(this, "Backward"))
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
        if (moveCheckFunc(this, "RightFront"))
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
        if (moveCheckFunc(this, "LeftFront"))
        {
            Vector3 posXZ = convertPosToCellPosFunc(transform.position + new Vector3(-1, 0, 1), gridLayout);
            transform.position = new Vector3(posXZ.x, transform.lossyScale.y / 2, posXZ.z);
        }
        else
        {
            Debug.Log("CantMove");
        }
    }

    public void MoveJump()
    {
        if (jumpMoveCheckFunc(this))
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
        if (breakCheckFunc(this, "Break"))
        {
            if (!Physics.Raycast(this.transform.position, transform.forward, out RaycastHit hitInfo, playerSize.z))return;

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
    }

    public void MoveRightBreak()
    {
        if (breakCheckFunc(this, "RightBreak"))
        {
            if (!Physics.Raycast(this.transform.position, transform.right, out RaycastHit hitInfo, playerSize.z)) return;

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
    }

    public void MoveLeftBreak()
    {
        if (breakCheckFunc(this, "LeftBreak"))
        {
            if (!Physics.Raycast(this.transform.position, -transform.right, out RaycastHit hitInfo, playerSize.z)) return;

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
    }

    private void DamageEvent()
    {
        if (damageEvent == null) return;
        damageEvent.Invoke();
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

            if (isGoal)
            {
                yield break;
            }

            yield return new WaitForSeconds(2.0f);
        }

        Debug.Log("ターンエンド");
    }
}
