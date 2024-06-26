using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class PlacingSystem : SystemBase, IOnUpdate
{
    public override void SetUp()
    {
        gameStat.objectOptionsIndexArray = new int[gameStat.optionNumber];
        gameStat.predictionObjectInstancesArray = new PlaceableObject[gameStat.predictionObjectPrefabsArray.Length];
        gameStat.placingObjectGrid = gameStat.placingObjectGridLayout.gameObject.GetComponent<Grid>();

        PredictionObjectInitialize();
    }

    public void OnUpdate()
    {

        //if (!gameStat.isMySetPhase) return;
        //isMySetPhaseスタート

        #region Initialize

        //まずInitializeで選べる選択肢（左の4つ）を生成
        if (gameStat.isMySetPhaseInitialized == false)
        {
            SetPhaseInitialize();
            gameStat.isMySetPhaseInitialized = true;
        }

        #endregion

        #region Placing

        //選べる4つのうち、選択されているインデックスに入っている数字をIDとする
        int selectOptionIndex = gameStat.objectOptionsIndexArray[gameStat.selectedPlacingObjectIndex];

        if (gameStat.mousePos != Vector3.zero)
        {
            PredictionObjectPosSet(selectOptionIndex);

            gameStat.selectingCellPos = SnapCoordinateToGrid(gameStat.mousePos, gameStat.placingObjectGrid, gameStat.placingObjectGridLayout);
            gameStat.predictionObject.transform.position = gameStat.selectingCellPos;
        }
        else
        {
            gameStat.predictionObject.transform.position = new Vector3(0, 100, 0);
        }

        //確定ボタンが押された
        if (gameStat.isPlacingInput)
        {
            //置こうとしたところになにもない
            if (CanBePlaced(gameStat.predictionObject, gameStat.occupiedTile, gameStat.placingObjectGridLayout))
            { 
                //設置
                Place(selectOptionIndex, gameStat.selectingCellPos);

                //gameStat.isMySetPhase = false;

                //PhaseEnd();
            }
        }

        #endregion
    }

    private void Place(int _prefabIndex, Vector3 _setPos)
    {
        PlaceableObject placedObject = GameObject.Instantiate(gameStat.objectAllPrefabsArray[_prefabIndex], _setPos, Quaternion.identity);
        placedObject.SetUp();
        gameStat.placedObjectList.Add(placedObject);

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

    //このarea内にあるtileの情報が全て入った配列を返す
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

        return array;
    }

    //ワールド座標からセルの中心を計算して返す
    private Vector3 SnapCoordinateToGrid(Vector3 position, Grid _grid, GridLayout _gridLayout)
    {
        Vector3Int cellPos = _gridLayout.WorldToCell(position);
        position = _grid.GetCellCenterWorld(cellPos);
        return position;
    }

    //startの位置から、size分のtileを敷き詰める
    private void TakeArea(TileBase _Tile, Vector3Int _start, Vector3Int _size)
    {
        
        gameStat.mainTileMap.BoxFill(_start, _Tile, _start.x, _start.y, _start.x + _size.x, _start.y + _size.y);
    }

    //selectOptionIndexが変更された際にpredictionObjectが盤面に残らないようにする処理
    private void PredictionObjectPosSet(int _index)
    {
        for (int i = 0; i < gameStat.predictionObjectInstancesArray.Length; i++)
        {
            if (i == _index)
            {
                gameStat.predictionObject = gameStat.predictionObjectInstancesArray[i];
            }
            else
            {
                gameStat.predictionObjectInstancesArray[i].transform.position = new Vector3(0, 100, 0);
            }
        }
    }

    private void SetPhaseInitialize()
    {
        for(int i = 0; i < gameStat.objectOptionsIndexArray.Length; i++)
        {
            gameStat.objectOptionsIndexArray[i] = Random.Range(0, gameStat.objectAllPrefabsArray.Length);
        }

        Debug.Log(string.Join(",", gameStat.objectOptionsIndexArray));
    }

    private void PredictionObjectInitialize()
    {
        for (int i = 0; i < gameStat.predictionObjectPrefabsArray.Length; i++)
        {
            PlaceableObject preObj = GameObject.Instantiate(gameStat.predictionObjectPrefabsArray[i], new Vector3(0, 100, 0), Quaternion.identity);
            preObj.SetUp();
            gameStat.predictionObjectInstancesArray[i] = preObj;
        }
    }

    
}
