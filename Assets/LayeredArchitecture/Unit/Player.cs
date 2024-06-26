using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Tilemaps;
using System.Drawing;

public class Player : MonoBehaviour
{
    private TileBase[] detectTilesArray;
    private GridLayout gridLayout;

    private Vector3Int playerSize;
    private Vector3[] Vertices;

    public Func<Player, Vector3Int, TileBase[], bool> canBeMovedCheckFunc;
    public Func<Vector3, GridLayout, Vector3> convertPosToCellPosFunc;

    public void Init(GridLayout gridLayout, TileBase[] _detectTilesArray)
    {
        this.gridLayout = gridLayout;
        this.detectTilesArray = _detectTilesArray;

        GetColliderVertexPositionLoacl();
        CalculateSizeInCells();
    }

    private void GetColliderVertexPositionLoacl()
    {
        BoxCollider boxCollider = gameObject.GetComponent<BoxCollider>();
        Vertices = new Vector3[4];
        Vertices[0] = boxCollider.center + new Vector3(-boxCollider.size.x, -boxCollider.size.y, -boxCollider.size.z) * 0.5f;
        Vertices[1] = boxCollider.center + new Vector3(boxCollider.size.x, -boxCollider.size.y, -boxCollider.size.z) * 0.5f;
        Vertices[2] = boxCollider.center + new Vector3(boxCollider.size.x, -boxCollider.size.y, boxCollider.size.z) * 0.5f;
        Vertices[3] = boxCollider.center + new Vector3(-boxCollider.size.x, -boxCollider.size.y, boxCollider.size.z) * 0.5f;
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

    private bool CanbeMoved(string _direction)
    {
        BoundsInt player = new BoundsInt();
        player.position = gridLayout.WorldToCell(transform.position);

        Vector3Int cell;

        switch (_direction)
        {
            case "Forward":
                cell = new Vector3Int(player.position.x, player.position.y, player.position.z + playerSize.z);
                break;

            case "Backward":
                cell = new Vector3Int(player.position.x, player.position.y, player.position.z - playerSize.z);
                break;

            case "Right":
                cell = new Vector3Int(player.position.x + playerSize.x, player.position.y, player.position.z);
                break;

            case "Left":
                cell = new Vector3Int(player.position.x - playerSize.x, player.position.y, player.position.z);
                break;

            default:
                cell = new Vector3Int(player.position.x, player.position.y, player.position.z + playerSize.z);
                break;
        }

        return canBeMovedCheckFunc(this, cell, detectTilesArray);
    }

    public void MoveForward()
    {
        if (CanbeMoved("Forward"))
        {
            transform.position = convertPosToCellPosFunc(transform.position + new Vector3(0, 0, 1),gridLayout);
        }

        Debug.Log("Move Forward");
    }

    public void MoveRight()
    {
        if (CanbeMoved("Right"))
        {
            transform.position = convertPosToCellPosFunc(transform.position + new Vector3(1, 0, 0), gridLayout);
        }

        Debug.Log("Move Right");
    }

    public void MoveLeft()
    {
        if (CanbeMoved("Left"))
        {
            transform.position = convertPosToCellPosFunc(transform.position + new Vector3(-1, 0, 0), gridLayout);
        }

        Debug.Log("Move Left");
    }

    public void MoveBackward()
    {
        if (CanbeMoved("Backward"))
        {
            transform.position = convertPosToCellPosFunc(transform.position + new Vector3(0, 0, -1), gridLayout);
        }

        Debug.Log("Move Backward");
    }

    public Vector3Int GetSize
    {
        get { return playerSize; }
    }
}
