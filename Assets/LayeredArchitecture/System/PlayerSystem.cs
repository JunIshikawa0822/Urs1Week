using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerSystem : SystemBase, IOnUpdate
{
    public override void SetUp()
    {
        PlayerInit();
    }

    public void OnUpdate()
    {
        //if (!gameStat.isMyMovePhase) return;
        //isMyMovePhaseスタート

        //まずInitializeで選べる選択肢（左の4つ）を生成
        if (gameStat.isMyMoveStart == false)
        {
            gameStat.player.MoveByProgram(gameStat.programList);
            gameStat.isMySetPhaseInitialized = true;
        }

        if (gameStat.isForward)
        {
            gameStat.player.MoveForward();
        }
        else if (gameStat.isRight)
        {
            gameStat.player.MoveRight();
        }
        else if (gameStat.isLeft)
        {
            gameStat.player.MoveLeft();
        }
        else if (gameStat.isBackward)
        {
            gameStat.player.MoveBackward();
        }
    }

    private void PlayerInit()
    {
        gameStat.player.canBeMovedCheckFunc = CanBeMoved;
        gameStat.player.convertPosToCellPosFunc = SnapCoordinateToGrid;

        gameStat.player.Init(gameStat.placingObjectGridLayout, gameStat.occupiedTilesArray);
        //Debug.Log(gameStat.player.GetYOffset);

        Vector3 playerPosXZ = SnapCoordinateToGrid(gameStat.player.transform.position, gameStat.placingObjectGridLayout);
        //Debug.Log(gameStat.player.GetSize.y);
        Debug.Log((int)gameStat.player.GetSize.y / 2);
        gameStat.player.transform.position = new Vector3(playerPosXZ.x, gameStat.player.transform.lossyScale.y / 2, playerPosXZ.z);
    }

    private bool CanBeMoved(Player _player, Vector3Int _cell, TileBase[] _occupiedTilesArray)
    {
        //BoundsInt player = new BoundsInt();
        BoundsInt area = new BoundsInt();

        //player.position = _gridLayout.WorldToCell(_player.transform.position);

        //WorldPosをCellPosに変える
        area.position = _cell;
        area.size = _player.GetSize;

        //MainTileMapに対して、areaの範囲内のtileを全て取ってきて配列に入れる
        TileBase[] baseArray = GetTilesBlock(area);

        //配列内を調べ、一つでもwhitetileがあるならfalseを返す（置けない）
        foreach (TileBase tile in baseArray)
        {
            foreach(TileBase occupiedTile in _occupiedTilesArray)
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

    private Vector3 SnapCoordinateToGrid(Vector3 _position, GridLayout _gridLayout)
    {
        Vector3Int cellPos = _gridLayout.WorldToCell(_position);
        _position = gameStat.placingObjectGrid.GetCellCenterWorld(cellPos);

        //Debug.Log(_position);
        return _position;
    }
}
