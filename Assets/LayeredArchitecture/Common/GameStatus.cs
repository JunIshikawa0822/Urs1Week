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

    [System.NonSerialized]
    public PlaceableObject objectToPlace;

    //Blockのパターン全てを格納するArray
    [SerializeField]
    public PlaceableObject[] objectAllPatternArray;

    //画面左端に出る選択肢4つに対して、それぞれ位置予測オブジェクトを用意するためのArray
    [System.NonSerialized]
    public PlaceableObject[] predictionObjectArray = new PlaceableObject[4];

    //画面左端に出る選択肢4つに対して、生成のために準備するArray
    [System.NonSerialized]
    public PlaceableObject[] placeToObjectArray = new PlaceableObject[4];

    //盤面に置くBlockのPrefab
    [SerializeField]
    public PlaceableObject objectToPlacePrefab;

    //位置予測オブジェクトのPrefab
    [SerializeField]
    public PlaceableObject predictionObjectPrefab;

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
