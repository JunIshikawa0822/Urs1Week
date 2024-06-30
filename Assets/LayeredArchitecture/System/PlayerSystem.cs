using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using static UnityEngine.RuleTile.TilingRuleOutput;
using Photon.Pun;
using Photon.Realtime;
using System;

public class PlayerSystem : SystemBase, IOnUpdate
{
    public override void SetUp()
    {
       //PlayerInit();
    }

    public void OnUpdate()
    {

        if(gameStat.isInstanitiatePlayerObj)
        {
            PlayerInit();
            gameStat.isInstanitiatePlayerObj = false;
        }
        //if (!gameStat.isMyMovePhase) return;
        //isMyMovePhaseスタート

            //まずInitializeで選べる選択肢（左の4つ）を生成
        if (gameStat.isMyMoveStart == false)
        {
            gameStat.player1PosForDamage = gameStat.player.transform.position;
            gameStat.player.MoveByProgram(gameStat.programList);
            gameStat.isMyMoveStart = true;
        }

        gameStat.isPlayerGoal = gameStat.player.GetIsGoal;
        if (gameStat.isPlayerGoal) Debug.Log("ごーーーーーーる！");

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
       
        //gameStat.player = GameObject.Instantiate(gameStat.playerPrefab, gameStat.playerStartPos.transform.position, Quaternion.identity);
        if (PhotonNetwork.IsMasterClient)
        {
            //PhotonNetwork.Instantiate("Player", gameStat.player1StartPos.position, Quaternion.identity);

            gameStat.player = PhotonNetwork.Instantiate("Player1", gameStat.player1StartPos.transform.position, Quaternion.identity).GetComponent<Player>();
            Debug.Log("Player1を生成しました");
        }
        else
        {
            gameStat.player = PhotonNetwork.Instantiate("Player2", gameStat.player2StartPos.transform.position, Quaternion.identity).GetComponent<Player>();
            Debug.Log("Player2を生成しました");
        }
        Debug.Log(gameStat.player.gameObject.name);
        
        gameStat.player.convertPosToCellPosFunc += SnapCoordinateToGrid;
        gameStat.player.goalCheckFunc += isPlayerGoal;
        gameStat.player.breakEvent += BreakPlacedObject;
        gameStat.player.damageEvent += PlayerDamage;
        gameStat.player.moveCheckFunc += MoveCheck;
        gameStat.player.jumpMoveCheckFunc += JumpMoveCheck;
        gameStat.player.breakCheckFunc += BreakCheck;
        
       

        if (PhotonNetwork.IsMasterClient)
            gameStat.player.Init(gameStat.placingObjectGridLayout, gameStat.goalPos1);
        else
            gameStat.player.Init(gameStat.placingObjectGridLayout, gameStat.goalPos2);

        //Vector3 playerPosXZ = SnapCoordinateToGrid(gameStat.player.transform.position, gameStat.placingObjectGridLayout);
        //gameStat.player.transform.position = new Vector3(playerPosXZ.x, gameStat.player.transform.lossyScale.y / 2, playerPosXZ.z);
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

    private bool isPlayerGoal(Player _player, UnityEngine.Transform _goalPos, bool _isMasterClient)
    {
        if(_isMasterClient)
        {
            if (_player.transform.position.z < _goalPos.position.z)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        else
        {
            if (_player.transform.position.z >_goalPos.position.z)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        
    }

    private void BreakPlacedObject(GameObject _object,bool _isMasterClient)
    {
        PlaceableObject obj = _object.GetComponent<PlaceableObject>();

        gameStat.placedObjectList.RemoveAt(obj.GetIndex);
        gameStat.programList.RemoveAt(obj.GetIndex);

        Debug.Log("ぶれいくされてる！！");
        obj.OnDestroy();
    }

    private void PlayerDamage()
    {
        //transform.position = gameStat.player1PosForDamage;
    }

    private bool MoveCheck(Player _player, string _direction,bool _isMasterClient)
    {
        BoundsInt player = new BoundsInt();
        player.position = gameStat.placingObjectGridLayout.WorldToCell(_player.transform.position);

        Vector3Int cell;
        if(_isMasterClient)
        {
            if (_direction == "Forward") cell = new Vector3Int(player.position.x, player.position.y + _player.GetSize.z, player.position.z);
            else if (_direction == "Right") cell = new Vector3Int(player.position.x + _player.GetSize.x, player.position.y, player.position.z);
            else if (_direction == "Left") cell = new Vector3Int(player.position.x - _player.GetSize.x, player.position.y, player.position.z);
            else if (_direction == "Backward") cell = new Vector3Int(player.position.x, player.position.y - _player.GetSize.z, player.position.z);
            else if (_direction == "RightFront") cell = new Vector3Int(player.position.x + _player.GetSize.x, player.position.y + _player.GetSize.z, player.position.z);
            else if (_direction == "LeftFront") cell = new Vector3Int(player.position.x - _player.GetSize.x, player.position.y + _player.GetSize.z, player.position.z);
            else cell = new Vector3Int(player.position.x, player.position.y + _player.GetSize.z, player.position.z);
        }
        else
        {
            if (_direction == "Forward") cell = new Vector3Int(player.position.x, player.position.y - _player.GetSize.z, player.position.z);
            else if (_direction == "Right") cell = new Vector3Int(player.position.x - _player.GetSize.x, player.position.y, player.position.z);
            else if (_direction == "Left") cell = new Vector3Int(player.position.x + _player.GetSize.x, player.position.y, player.position.z);
            else if (_direction == "Backward") cell = new Vector3Int(player.position.x, player.position.y + _player.GetSize.z, player.position.z);
            else if (_direction == "RightFront") cell = new Vector3Int(player.position.x - _player.GetSize.x, player.position.y - _player.GetSize.z, player.position.z);
            else if (_direction == "LeftFront") cell = new Vector3Int(player.position.x + _player.GetSize.x, player.position.y - _player.GetSize.z, player.position.z);
            else cell = new Vector3Int(player.position.x, player.position.y - _player.GetSize.z, player.position.z);
        }
        
        

        return CanBeMoved(_player, cell, gameStat.occupiedTilesArray);
    }

    private bool JumpMoveCheck(Player _player,bool _isMasterClient)
    {
        BoundsInt player = new BoundsInt();
        player.position = gameStat.placingObjectGridLayout.WorldToCell(_player.transform.position);

        Vector3Int cell1;
        Vector3Int cell2;

        if(_isMasterClient)
        {
            cell1 = new Vector3Int(player.position.x, player.position.y + _player.GetSize.z, player.position.z);
            cell2 = new Vector3Int(player.position.x, player.position.y + _player.GetSize.z * 2, player.position.z);
        }
        else
        {
            cell1 = new Vector3Int(player.position.x, player.position.y - _player.GetSize.z, player.position.z);
            cell2 = new Vector3Int(player.position.x, player.position.y - _player.GetSize.z * 2, player.position.z);
        }
        

        if (CanBeMoved(_player, cell1, gameStat.occupiedTilesArray) == false && CanBeMoved(_player, cell2, gameStat.occupiedTilesArray) == true)
        {
            return true;
        }
        else
        {
            return false;
        }

    }

    private bool BreakCheck(Player _player, string _direction,bool _isMasterClient)
    {
        BoundsInt player = new BoundsInt();
        player.position = gameStat.placingObjectGridLayout.WorldToCell(_player.transform.position);

        Vector3Int cell;
        if(_isMasterClient)
        {
            if (_direction == "Break") cell = new Vector3Int(player.position.x, player.position.y + _player.GetSize.z, player.position.z);
            else if (_direction == "RightBreak") cell = new Vector3Int(player.position.x + _player.GetSize.x, player.position.y, player.position.z);
            else if (_direction == "LeftBreak") cell = new Vector3Int(player.position.x - _player.GetSize.x, player.position.y, player.position.z);
            else cell = new Vector3Int(player.position.x, player.position.y + _player.GetSize.z, player.position.z);

        }
        else
        {
            if (_direction == "Break") cell = new Vector3Int(player.position.x, player.position.y - _player.GetSize.z, player.position.z);
            else if (_direction == "RightBreak") cell = new Vector3Int(player.position.x - _player.GetSize.x, player.position.y, player.position.z);
            else if (_direction == "LeftBreak") cell = new Vector3Int(player.position.x + _player.GetSize.x, player.position.y, player.position.z);
            else cell = new Vector3Int(player.position.x, player.position.y - _player.GetSize.z, player.position.z);

        }

        if (CanBeMoved(_player, cell, gameStat.occupiedTilesArray) == false)
        {
            return true;
        }
        else
        {
            Debug.Log("壊せるものがなにもないよ！！");
            return false;
        }

    }

    

   
}
