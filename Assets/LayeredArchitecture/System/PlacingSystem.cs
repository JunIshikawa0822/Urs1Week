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
        gameStat.predictionObjectInstancesArray = new PredictionObject[gameStat.predictionObjectPrefabsArray.Length];
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
        int listIndex = gameStat.objectOptionsIndexArray[gameStat.selectedPlacingObjectIndex];

        if (gameStat.mousePos != Vector3.zero)
        {
            PredictionObjectPosSet(listIndex);
            gameStat.predictionObject.transform.position = gameStat.selectingCellPos;
        }
        else
        {
            if (gameStat.predictionObject == null) return;
            gameStat.predictionObject.transform.position = new Vector3(0, 100, 0);
        }

        //確定ボタンが押された
        if (!gameStat.isPlacingInput) return;

        if (isPlaceableArea(gameStat.player, gameStat.predictionObject))
        {
            //置こうとしたところになにもない
            if (CanBePlaced(gameStat.predictionObject, gameStat.occupiedTilesArray, gameStat.placingObjectGridLayout))
            {
                //設置
                Place(listIndex, gameStat.selectingCellPos);

                //Debug.Log("program : " + string.Join(",", gameStat.programList));
                //gameStat.isMySetPhase = false;

                //PhaseEnd();
            }
            else
            {
                Debug.Log("なんらかのもんだい");
            }
        }

        #endregion
    }

    private void Place(int _Index, Vector3 _setPos)
    {
        PlaceableObject placedObject = GameObject.Instantiate(gameStat.objectAllPrefabsArray[_Index], _setPos, Quaternion.identity);
        placedObject.SetUp(gameStat.mainTileMap, gameStat.occupiedTilesArray[0], gameStat.placedObjectList.Count);

        gameStat.placedObjectList.Add(placedObject);
        gameStat.programList.Add(_Index);
    }

    //オブジェクトの範囲に占有タイルがあるかどうかを返す
    private bool CanBePlaced(PredictionObject _predictionObject, TileBase[] _occupiedTilesArray, GridLayout _gridLayout)
    {
        BoundsInt area = new BoundsInt();

        //WorldPosをCellPosに変える
        area.position = _gridLayout.WorldToCell(_predictionObject.GetStartPosition());
        area.size = _predictionObject.GetPredictionObjSize;

        //MainTileMapに対して、areaの範囲内のtileを全て取ってきて配列に入れる
        TileBase[] baseArray = GetTilesBlock(area);

        Vector3 playerPos = gameStat.player.transform.position;
        if (area.Contains(new Vector3Int((int)playerPos.x, (int)playerPos.y, (int)playerPos.z)))
        {
            return false;
        }
        
        //配列内を調べ、一つでもwhitetileがあるならfalseを返す（置けない）
        foreach (TileBase tile in baseArray)
        {
            foreach (TileBase occupiedTile in _occupiedTilesArray)
            {
                if (tile == occupiedTile)
                {
                    return false;
                }
            }
        }
        
        //置ける
        return true;
    }

    private bool isPlaceableArea(Player _player, PredictionObject _predictionObject)
    {
        //Ray mouseRay = Camera.main.ScreenPointToRay(gameStat.mousePos);
        //if(Physics.Raycast(mouseRay, out RaycastHit hitInfo, Mathf.Infinity, gameStat.playerLayer))
        //{
        //    Debug.Log(hitInfo.collider.gameObject.name);
        //    Debug.Log("playerだよ");
        //    return false;
        //}
        if (gameStat.mousePos == Vector3.zero)
        {
            Debug.Log("枠外");
            return false;
        }
        if (_predictionObject.transform.position.z > _player.transform.position.z + _player.GetSize.z)
        {
            Debug.Log("おけないよ");
            return false;
        }
        else
        {
            Debug.Log("おけるよ");
            return true;
        }
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

    //startの位置から、size分のtileを敷き詰める
    //private void TakeArea(TileBase _tile, Tilemap _tileMap, Vector3Int _start, Vector3Int _size)
    //{
        
    //    _tileMap.BoxFill(_start, _tile, _start.x, _start.y, _start.x + _size.x, _start.y + _size.y);
    //}

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
            PredictionObject preObj = GameObject.Instantiate(gameStat.predictionObjectPrefabsArray[i], new Vector3(0, 100, 0), Quaternion.identity);
            preObj.SetUp();
            gameStat.predictionObjectInstancesArray[i] = preObj;
        }
    }
}
