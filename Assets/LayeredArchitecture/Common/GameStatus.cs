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
    //[System.NonSerialized]
    //public PlaceableObject predictionObject;
    [SerializeField]
    public List<PlaceableObject> predictionObjectArray;

    [SerializeField]
    public PlaceableObject objectToPlacePrefab;

    [SerializeField]
    public PlaceableObject predictionObjectPrefab;

    [System.NonSerialized]
    public List<PlaceableObject> placedObjectlist = new List<PlaceableObject>();

    [SerializeField]
    public Vector3 mousePos;

    [SerializeField]
    public Vector3 selectingCellPos;

    //[SerializeField]
    //public GridLayout gridLayout;

    [Header("Input")]
    [SerializeField]
    public bool isPlacingInput = false;

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
}
