using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlaceableObject : MonoBehaviour
{
    public bool Placed { get; private set; }
    public Vector3Int Size;
    private Vector3[] Vertices;

    public int index;
    private Tilemap tileMap;
    private TileBase occupiedTileBase;
    private Vector3Int pos;


    public void SetUp(Tilemap _tileMap, TileBase _occupiedTileBase, TileBase _stageTileBase, int _index)
    {
        GetColliderVertexPositionLoacl();
        CalculateSizeInCells();

        this.tileMap = _tileMap;
        this.occupiedTileBase = _occupiedTileBase;
        this.index = _index;

        SetTile();
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

        for(int i = 0; i < vertices.Length; i++)
        {
            Vector3 worldPos = transform.TransformPoint(Vertices[i]);
            vertices[i] = BuildingSystem.current.gridLayout.WorldToCell(worldPos);
        }

        Size = new Vector3Int(Mathf.Abs((vertices[0] - vertices[1]).x), Mathf.Abs((vertices[0] - vertices[3]).y), 1);
    }

    public Vector3 GetStartPosition()
    {
        return transform.TransformPoint(Vertices[0]);
    }

    public int GetIndex
    {
        get { return index; }
    }

    //public void Rotate()
    //{
    //    transform.Rotate(new Vector3(0, 90, 0));
    //    Size = new Vector3Int(Size.y, Size.x, 1);

    //    Vector3[] vertices = new Vector3[Vertices.Length];

    //    for(int i = 0; i < vertices.Length; i++)
    //    {
    //        vertices[i] = Vertices[(i + 1) % Vertices.Length];
    //    }

    //    Vertices = vertices;
    //}

    //public virtual void Place()
    //{
    //    ObjectDrag drag = gameObject.GetComponent<ObjectDrag>();
    //    Destroy(drag);

    //    Placed = true;
    //}

    private void SetTile()
    {
        Vector3Int tilePos = tileMap.WorldToCell(this.transform.position);
        this.pos = tilePos;
        tileMap.SetTile(pos, occupiedTileBase);
    }

    public void OnDestroy()
    {
        tileMap.SetTile(pos, null);
        Destroy(this.gameObject);
    }
}
