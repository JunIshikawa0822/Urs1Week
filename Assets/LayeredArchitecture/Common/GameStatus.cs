using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[System.Serializable]
public class GameStatus
{
    [SerializeField] public Tilemap mainTileMap;
    [SerializeField] public TileBase occupiedTile; //占有されたことを示す

    [System.NonSerialized]
    public Grid placingObjectGrid;

    [SerializeField]
    public GridLayout placingObjectGridLayout;

    //実際に設置するオブジェクト
    [System.NonSerialized]
    public PlaceableObject objectToPlace;

    //実際に移動するPredictionObj
    [System.NonSerialized]
    public PlaceableObject predictionObject;

    //盤面に置くBlockのPrefab
    [SerializeField]
    public PlaceableObject objectToPlacePrefab;

    //Blockのパターン全てを格納するArray
    [SerializeField]
    public PlaceableObject[] objectAllPrefabArray;

    //PredictionObjのPrefab
    [SerializeField]
    public PlaceableObject predictionObjectPrefab;

    //PredictionObjのパターン全てを格納するArray
    [SerializeField]
    public PlaceableObject[] predictionObjectAllPrefabArray;

    //盤面に置いてあるBlockを格納するリスト
    [System.NonSerialized]
    public List<PlaceableObject> placedObjectList = new List<PlaceableObject>();

    [SerializeField]
    public Vector3 mousePos;

    [SerializeField]
    public Vector3 selectingCellPos;

    //[SerializeField]
    //public GridLayout gridLayout;

    [Header("Input")]
    [SerializeField]
    public bool isPlacingInput = false;

    //画面左の4つの選択肢のうち、どの選択肢を選んでいるか
    public int selectedPlacingObjectIndex = 0;

    [Header("Flag")]
    [System.NonSerialized]
    public bool isMySetPhase = false;
    [System.NonSerialized]
    public bool isMySetPhaseInitialized = false;
    [System.NonSerialized]
    public bool isMyMovePhase = false;
    [System.NonSerialized]
    public bool isPhaseEnd = false;

    [SerializeField]
    public LayerMask layHitlayer;

    [SerializeField]
    public InputNameType placingInputDownName;

    public enum InputNameType
    {
        MouseButtonRight,
        MouseButtonLeft,
        space,
        right_shift,
        left_shift
    }

    public enum BlockPattern
    {
        Right,
        Left,
        Forward,

    }
}
