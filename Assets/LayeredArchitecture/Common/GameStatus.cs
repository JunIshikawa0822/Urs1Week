using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

[System.Serializable]
public class GameStatus
{
    [Header("Player1")]
    [System.NonSerialized]
    public Player player = null;

    [SerializeField]
    public Player playerPrefab;

    [SerializeField]
    public GameObject playerObj;

    [SerializeField]
    public Transform player1StartPos;

    [SerializeField]
    public Transform player2StartPos;

    [SerializeField]
    public Transform goalPos1;

    [SerializeField]
    public Transform goalPos2;

    [SerializeField]
    public Vector3 player1PosForDamage;

    [SerializeField]
    public Vector3 player2PosForDamage;

    [SerializeField]
    public bool isInstanitiatePlayerObj=false;

    [Header("Placing")]
    [SerializeField] public Tilemap mainTileMap;

    [SerializeField] public TileBase[] occupiedTilesArray; //占有されたことを示す
    [SerializeField] public TileBase stageTile;

    [System.NonSerialized]
    public Grid placingObjectGrid;

    [SerializeField]
    public GridLayout placingObjectGridLayout;

    //実際に設置するオブジェクト
    [System.NonSerialized]
    public PlaceableObject objectToPlace = null;

    //実際に移動するPredictionObj
    [System.NonSerialized]
    public PredictionObject predictionObject = null;

    [System.NonSerialized]
    public int optionNumber = 4;

    //使用可能なオブジェクト番号を選択し、格納するArray
    [System.NonSerialized]
    public int[] objectOptionsIndexArray;

    //BlockのPrefab全てを格納するArray
    [SerializeField]
    public PlaceableObject[] objectAllPrefabsArray;

    //BlockUIのPrefab全てを格納するArray
    [SerializeField]
    public Sprite[] objectImageAllPrefabsArray;

    //盤面に置いてあるBlockを格納するリスト
    [System.NonSerialized]
    public List<PlaceableObject> placedObjectList = new List<PlaceableObject>();

    [System.NonSerialized]
    public List<int> programList = new List<int>();

    //PredictionObjのPrefab全てを格納するArray
    [SerializeField]
    public PredictionObject[] predictionObjectPrefabsArray;

    //生成したPredictionObj全てを格納するArray
    [System.NonSerialized]
    public PredictionObject[] predictionObjectInstancesArray;

    [System.NonSerialized]
    public Vector3 selectingCellPos;

    [Header("Input")]
    //[System.NonSerialized]
    public Vector3 mousePos;

    [SerializeField]
    public bool isPlacingInput = false;

    //画面左の4つの選択肢のうち、どの選択肢を選んでいるか
    public int selectedPlacingObjectIndex = 0;

    [System.NonSerialized]
    public LayerMask playerLayer = 1 << 7;

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
    //[System.NonSerialized]
    public bool isMySetPhase = false;
    [System.NonSerialized]
    public bool isMySetPhaseInitialized = false;
    //[System.NonSerialized]
    public bool isMyMovePhase = false;
    [System.NonSerialized]
    public bool isMyMoveStart = true;
    [System.NonSerialized]
    public bool isPhaseEnd = false;
    [System.NonSerialized]
    public bool isSetRandomBlockUI = false;
    [System.NonSerialized]
    public bool isSetProgramView = false;
    [System.NonSerialized]
    public bool isFirstUI = false;

    //ゴールしたか
    [System.NonSerialized]
    public bool isPlayerGoal = false;

    [SerializeField]
    public LayerMask layHitlayer;

    [SerializeField]
    public InputNameType placingInputDownName;


    [Header("UI")]
    public ButtonBase[] selectPanelArray;
    public Image[] selsectImageArray;
    public GameObject scrorViewContent;
    public GameObject programViewImage;

    public GameObject winUI;
    public GameObject loseUI;
    public GameObject enemyFhaseUI;
    public GameObject mySetFhaseUI;
    public GameObject myMoveFhaseUI;
    public GameObject canvas;
    public Text nowPhaseText;


    [Header("Camera")]
    public GameObject camera1;
    public GameObject camera2;

    [Header("Input")]
    [System.NonSerialized]
    public bool isEnterRoom = false;
    [System.NonSerialized]
    public bool isMaster;

    [Header("Taiki")]
    [System.NonSerialized]
    public bool isMatchOk=false;
    [SerializeField]
    public GameObject taikiPanel;
    [SerializeField]
    public GameObject gamePanel;

    [Header("Tarn")]
    public TurnManger turnManger;
    //[System.NonSerialized]
    public bool isMyPhase;
    //[System.NonSerialized]
    public bool isAtackFirst;
    //[System.NonSerialized]
    public int turnNum;

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
        Break,
        RightBreak,
        LeftBreak
    }
}
