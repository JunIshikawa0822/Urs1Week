using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[System.Serializable]
public class GameStatus
{
    [Header("Player1")]
    [System.NonSerialized]
    public Player player = null;

    [SerializeField]
    public Player playerPrefab;

    [SerializeField]
    public Transform playerStartPos;

    [SerializeField]
    public Transform goalPos;


    [Header("Placing")]
    [SerializeField] public Tilemap mainTileMap;

    [SerializeField] public TileBase[] occupiedTilesArray; //占有されたことを示す

    [System.NonSerialized]
    public Grid placingObjectGrid;

    [SerializeField]
    public GridLayout placingObjectGridLayout;

    //実際に設置するオブジェクト
    [System.NonSerialized]
    public PlaceableObject objectToPlace = null;

    //実際に移動するPredictionObj
    [System.NonSerialized]
    public PlaceableObject predictionObject = null;

    [System.NonSerialized]
    public int optionNumber = 4;

    //使用可能なオブジェクト番号を選択し、格納するArray
    [System.NonSerialized]
    public int[] objectOptionsIndexArray;

    //BlockのPrefab全てを格納するArray
    [SerializeField]
    public PlaceableObject[] objectAllPrefabsArray;

    //盤面に置いてあるBlockを格納するリスト
    [System.NonSerialized]
    public List<PlaceableObject> placedObjectList = new List<PlaceableObject>();

    [System.NonSerialized]
    public List<int> programList = new List<int>();

    //PredictionObjのPrefab全てを格納するArray
    [SerializeField]
    public PlaceableObject[] predictionObjectPrefabsArray;

    //生成したPredictionObj全てを格納するArray
    [System.NonSerialized]
    public PlaceableObject[] predictionObjectInstancesArray;

    [System.NonSerialized]
    public Vector3 selectingCellPos;

    [Header("Input")]
    [System.NonSerialized]
    public Vector3 mousePos;

    [SerializeField]
    public bool isPlacingInput = false;

    //画面左の4つの選択肢のうち、どの選択肢を選んでいるか
    public int selectedPlacingObjectIndex = 0;

    public LayerMask playerLayer;

    [Header("InputDebug")]
    [System.NonSerialized]
    public bool isForward = false;
    [System.NonSerialized]
    public bool isRight = false;
    [System.NonSerialized]
    public bool isLeft = false;
    [System.NonSerialized]
    public bool isBackward = false;

    [Header("Flag")]
    [System.NonSerialized]
    public bool isMySetPhase = false;
    [System.NonSerialized]
    public bool isMySetPhaseInitialized = false;
    [System.NonSerialized]
    public bool isMyMovePhase = false;
    [System.NonSerialized]
    public bool isMyMoveStart = true;
    [System.NonSerialized]
    public bool isPhaseEnd = false;

    //ゴールしたか
    [System.NonSerialized]
    public bool isPlayerGoal = false;

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
        Forward,
        Right,
        Left,
        Backward,
        RightFront,
        LeftFront,
        Jump,
        Break
    }
}
