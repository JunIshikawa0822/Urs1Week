using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class PlacingSystem : SystemBase, IOnUpdate
{
    public override void SetUp()
    {
        gameStat.placingObjectGrid = gameStat.placingObjectGridLayout.gameObject.GetComponent<Grid>();

        for (int i = 0; i < gameStat.predictionObjectArray.Count; i++)
        {
            PlaceableObject obj = GameObject.Instantiate(gameStat.predictionObjectPrefab, Vector3.zero, Quaternion.identity);
            obj.SetUp();
            gameStat.predictionObjectArray[i] = obj;
        }
    }

    public void OnUpdate()
    {
        gameStat.selectingCellPos = SnapCoordinateToGrid(gameStat.mousePos, gameStat.placingObjectGrid, gameStat.placingObjectGridLayout);
        gameStat.predictionObjectArray[0].transform.position = gameStat.selectingCellPos;

        //確定ボタンが押された
        if (gameStat.isPlacingInput)
        {
            //置こうとしたところになにもない
            if (CanBePlaced(gameStat.predictionObjectArray[0], gameStat.occupiedTile, gameStat.placingObjectGridLayout))
            {
                Place(gameStat.objectToPlacePrefab, gameStat.selectingCellPos);  
            }
        }
    }

    private void Place(PlaceableObject _prefab, Vector3 _setPos)
    {
        PlaceableObject placedObject = GameObject.Instantiate(_prefab, _setPos, Quaternion.identity);
        placedObject.SetUp();
        gameStat.placedObjectlist.Add(placedObject);

        Vector3Int start = gameStat.placingObjectGridLayout.WorldToCell(placedObject.GetStartPosition());
        TakeArea(gameStat.occupiedTile, start, placedObject.Size);
    }

    //オブジェクトの範囲に占有タイルがあるかどうかを返す
    private bool CanBePlaced(PlaceableObject _placeableObject, TileBase _occupiedTile, GridLayout _gridLayout)
    {
        BoundsInt area = new BoundsInt();

        //WorldPosをCellPosに変える
        area.position = _gridLayout.WorldToCell(_placeableObject.GetStartPosition());
        area.size = _placeableObject.Size;

        //MainTileMapに対して、areaの範囲内のtileを全て取ってきて配列に入れる
        TileBase[] baseArray = GetTilesBlock(area);

        //配列内を調べ、一つでもwhitetileがあるならfalseを返す（置けない）
        foreach (TileBase tile in baseArray)
        {
            if (tile == _occupiedTile)
            {
                return false;
            }
        }

        //置ける
        return true;
    }

    private TileBase[] GetTilesBlock(BoundsInt _areaBox)
    {
        //areaのサイズ分TileBaseの配列を作成
        TileBase[] array = new TileBase[_areaBox.size.x * _areaBox.size.y * _areaBox.size.z];
        int counter = 0;

        //areaの中にあるgrid座標全てに対して、
        foreach (Vector3Int v in _areaBox.allPositionsWithin)
        {
            //なんかareaに対して座標変換かけてる
            Vector3Int pos = new Vector3Int(v.x, v.y, 0);

            //このグリッドのタイルマップに関して、posの位置にあるTileを取ってきてArrayに入れる
            array[counter] = gameStat.mainTileMap.GetTile(pos);
            counter++;
        }

        //このarea内にあるtileの情報が全て入った配列を返す
        return array;
    }

    private Vector3 SnapCoordinateToGrid(Vector3 position, Grid _grid, GridLayout _gridLayout)
    {
        Vector3Int cellPos = _gridLayout.WorldToCell(position);
        position = _grid.GetCellCenterWorld(cellPos);
        return position;
    }

    private void TakeArea(TileBase _Tile, Vector3Int _start, Vector3Int _size)
    {
        //startの位置からに、size分のtileを敷き詰める
        gameStat.mainTileMap.BoxFill(_start, _Tile, _start.x, _start.y, _start.x + _size.x, _start.y + _size.y);
    }
}
