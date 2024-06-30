using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Photon.Pun;
using Photon.Realtime;


public class PlaceableObject : MonoBehaviourPun, IPunInstantiateMagicCallback
{
    public bool Placed { get; private set; }
    public Vector3Int Size;
    private Vector3[] Vertices;

    private int index;
    private Tilemap tileMap;
    private TileBase occupiedTileBase;
    private Vector3Int pos;
    private GridLayout gridLayout;

    public Tilemap tileMapPUN;
    public TileBase tileBasePUN1;
    public TileBase tileBasePUN2;

    public void SetUp(Tilemap _tileMap, TileBase _occupiedTileBase, int _index, GridLayout _gridLayout)
    {
        this.gridLayout = _gridLayout;
        this.tileMap = _tileMap;
        this.occupiedTileBase = _occupiedTileBase;
        this.index = _index;
        GetColliderVertexPositionLoacl();
        CalculateSizeInCells();
        SetTiles();
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
            vertices[i] = gridLayout.WorldToCell(worldPos);
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

    public void SetTiles()
    {
        Debug.Log("おけた");
        if (tileMap == null)
        {

            tileMapPUN = GameObject.Find("StageTileMap").GetComponent<Tilemap>();
            Vector3Int tilePos = tileMapPUN.WorldToCell(this.transform.position);
            Debug.Log("相手のほんとにおけた");
            Debug.Log(PhotonNetwork.IsMasterClient);
            if(PhotonNetwork.IsMasterClient)
                tileMapPUN.SetTile(pos, tileBasePUN1);
            else
                tileMapPUN.SetTile(pos, tileBasePUN2);
        }
        else
        {
            Vector3Int tilePos = tileMap.WorldToCell(this.transform.position);
            Debug.Log("ほんとにおけた");
            this.pos = tilePos;
            tileMap.SetTile(pos, occupiedTileBase);
        }

       
        
    }

    public void OnDestroy()
    {
        if (tileMap == null) return;

        tileMap.SetTile(pos, null);
        Destroy(this.gameObject);
    }
    void IPunInstantiateMagicCallback.OnPhotonInstantiate(PhotonMessageInfo info)
    {
        if (info.photonView.IsMine)
        {
            PhotonView targetPhotonView;
            targetPhotonView = info.photonView;
            // 対象のPhotonView IDを指定
            targetPhotonView.RPC("SetTileRPC", RpcTarget.Others);
        }
        /*
        if (info.Sender.IsLocal)
        {
            
            Debug.Log("自身がネットワークオブジェクトを生成しました");
            //this.GetComponent<PhotonView>().RPC("SetTileRPC", RpcTarget.Others);

        }
        else
        {
           
            
            Debug.Log("他プレイヤーがネットワークオブジェクトを生成しました");
            SetTiles();
            //this.GetComponent<PhotonView>().RPC("SetTileRPC", RpcTarget.All);
        }
        */
    }
    [PunRPC]
    public void SetTileRPC()
    {
        Debug.Log("タイルをセット");
        SetTiles();
        
    }
}
