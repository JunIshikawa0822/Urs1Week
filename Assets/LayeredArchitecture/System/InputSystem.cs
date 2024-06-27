using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;
using System;

public class InputSystem : SystemBase, IOnPreUpdate
{
    public void OnPreUpdate()
    {
        gameStat.mousePos = GetMouseWorldPosition(Input.mousePosition);

        Vector3 posXZ = SnapCoordinateToGrid(gameStat.mousePos, gameStat.placingObjectGrid, gameStat.placingObjectGridLayout);
        gameStat.selectingCellPos = new Vector3(posXZ.x, 0.5f, posXZ.z);

        gameStat.isPlacingInput = InputDown(gameStat.placingInputDownName);

        GetBlockSelectInput();
        KeyInput();
    }

    private void KeyInput()
    {
        //gameStat.isForward = (Input.GetKeyDown(KeyCode.UpArrow)) ? true : false;
        if (Input.GetKeyDown(KeyCode.UpArrow))
            Debug.Log("OK");
        gameStat.isRight = (Input.GetKeyDown(KeyCode.RightArrow)) ? true : false;
        gameStat.isLeft = (Input.GetKeyDown(KeyCode.LeftArrow)) ? true : false;
        gameStat.isBackward = (Input.GetKeyDown(KeyCode.DownArrow)) ? true : false;

        gameStat.isMyMoveStart = Input.GetKeyDown(KeyCode.Return) ? false : true;

        //if (Input.GetKeyDown(KeyCode.O)) gameStat.isMyMoveStart = false;
        if (Input.GetKeyDown(KeyCode.P)) gameStat.isMySetPhaseInitialized = false;
    }

    private Vector3 GetMouseWorldPosition(Vector3 _point)
    {
        Ray mouseRay = Camera.main.ScreenPointToRay(_point);

        if (Physics.Raycast(mouseRay, out RaycastHit raycastHit, Mathf.Infinity, gameStat.layHitlayer))
        {
            return raycastHit.point;
        }
        else
        {
            //当たらないなら0
            return Vector3.zero;
        }
    }

    //ワールド座標からセルの中心を計算して返す
    private Vector3 SnapCoordinateToGrid(Vector3 position, Grid _grid, GridLayout _gridLayout)
    {
        Vector3Int cellPos = _gridLayout.WorldToCell(position);
        position = _grid.GetCellCenterWorld(cellPos);
        return position;
    }

    private bool InputDown(GameStatus.InputNameType _isInput)
    {
        return IsInputDown(_isInput);
    }

    private bool IsInputDown(GameStatus.InputNameType _inputName)
    {
        //Debug.Log("JunIshikawa");
        bool isInputBool = false;

        if (_inputName == GameStatus.InputNameType.MouseButtonRight)
        {
            isInputBool = MouseBool(1);
        }
        else if (_inputName == GameStatus.InputNameType.MouseButtonLeft)
        {
            isInputBool = MouseBool(0);
        }
        else
        {
            isInputBool = KeyBool(_inputName.ToString());
        }

        return isInputBool;

        bool MouseBool(int _mouseNumber)
        {
            if (Input.GetMouseButtonDown(_mouseNumber))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        bool KeyBool(string _keyName)
        {
            string[] words = _keyName.Split("_");
            string conventionKeyName = string.Join(" ", words);

            if (Input.GetKeyDown(conventionKeyName))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
    /*
    private void GetBlockSelectInput()
    {
        if (!Input.anyKeyDown) return;

        string keyStr = Input.inputString;
        Debug.Log(keyStr);
        int strToInt;
        
        if (!int.TryParse(keyStr, out strToInt))
        {
            return;
        }
        else if (strToInt > 3) 
        {
            return;
        }
        else
        {
            Debug.Log(strToInt);
            gameStat.selectedPlacingObjectIndex = strToInt;
        }
    }
    */

    private void GetBlockSelectInput()
    {
       

        if (Input.GetKeyDown(KeyCode.Alpha0))
        {

            gameStat.selectedPlacingObjectIndex = 0;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            gameStat.selectedPlacingObjectIndex = 1;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
           
            gameStat.selectedPlacingObjectIndex = 2;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {

            gameStat.selectedPlacingObjectIndex = 3;
        }
    }
}
 