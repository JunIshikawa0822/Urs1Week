using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BuildingSystem : MonoBehaviour
{
    public static BuildingSystem current;
    public GridLayout gridLayout;

    private Grid grid;
    [SerializeField] private Tilemap MainTilemap;
    [SerializeField] private TileBase whiteTile; //占有されたことを示す

    public GameObject prefab1;
    public GameObject prefab2;

    private PlaceableObject objectToPlace;

    #region Unity methods

    private void Awake()
    {
        current = this;
        grid = gridLayout.gameObject.GetComponent<Grid>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            InitializeWithObject(prefab1);
        }
        else if (Input.GetKeyDown(KeyCode.B))
        {
            InitializeWithObject(prefab2);
        }

        if (!objectToPlace)
        {
            return;
        }

        //if (Input.GetKeyDown(KeyCode.Return))
        //{
        //    objectToPlace.Rotate();
        //}
        //else if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    if (CanBePlaced(objectToPlace))
        //    {
        //        objectToPlace.Place();
        //        Vector3Int start = gridLayout.WorldToCell(objectToPlace.GetStartPosition());
        //        TakeArea(start, objectToPlace.Size);
        //    }
        //    else
        //    {
        //        Destroy(objectToPlace.gameObject);
        //    }
        //}
        //else if (Input.GetKeyDown(KeyCode.Escape))
        //{
        //    Destroy(objectToPlace.gameObject);
        //}
    }

    #endregion

    #region Utils

    //マウスの位置を取ってくる
    public static Vector3 GetMouseWorldPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if(Physics.Raycast(ray, out RaycastHit raycastHit))
        {
            //rayが当たれば返す
            return raycastHit.point;
        }
        else
        {
            //当たらないなら0
            return Vector3.zero;
        }
    }

    //引数でもらった位置をグリッドに変換し、グリッドの中心を返す
    public Vector3 SnapCoordinateToGrid(Vector3 position)
    {
        Vector3Int cellPos = gridLayout.WorldToCell(position);
        position = grid.GetCellCenterWorld(cellPos);
        return position;
    }
    
    private static TileBase[] GetTilesBlock(BoundsInt area, Tilemap tilemap)
    {
        //areaのサイズ分TileBaseの配列を作成
        TileBase[] array = new TileBase[area.size.x * area.size.y * area.size.z];
        int counter = 0;

        //areaの中にあるgrid座標全てに対して、
        foreach(Vector3Int v in area.allPositionsWithin)
        {
            //なんかareaに対して座標変換かけてる
            Vector3Int pos = new Vector3Int(v.x, v.y, 0);

            //このグリッドのタイルマップに関して、posの位置にあるTileを取ってきてArrayに入れる
            array[counter] = tilemap.GetTile(pos);
            counter++;
        }

        //このarea内にあるtileの情報が全て入った配列を返す
        return array;
    }

    #endregion

    #region BuildingReplacement


    public void InitializeWithObject(GameObject prefab)
    {
        Vector3 position = SnapCoordinateToGrid(Vector3.zero);

        //new Vector(0,0,0)に生成
        GameObject obj = Instantiate(prefab, position, Quaternion.identity);

        //PlaceableObjectにObjectDragをつける（操作できるようにする）
        objectToPlace = obj.GetComponent<PlaceableObject>();
        obj.AddComponent<ObjectDrag>();
    }

    private bool CanBePlaced(PlaceableObject placeableObject)
    {
        BoundsInt area = new BoundsInt();

        //WorldPosをCellPosにかえる
        area.position = gridLayout.WorldToCell(objectToPlace.GetStartPosition());
        area.size = placeableObject.Size;

        //MainTileMapに対して、areaの範囲内のtileを全て取ってきて配列に入れる
        TileBase[] baseArray = GetTilesBlock(area, MainTilemap);

        //配列内を調べ、一つでもwhitetileがあるならfalseを返す（置けない）
        foreach (TileBase b in baseArray)
        {
            if(b == whiteTile)
            {
                return false;
            }
        }

        //置ける
        return true;
    }

    public void TakeArea(Vector3Int start, Vector3Int size)
    {
        //tileMapの位置に、size分のtileを敷き詰める
        MainTilemap.BoxFill(start, whiteTile, start.x, start.y, start.x + size.x, start.y + size.y);
    }

    #endregion
}
