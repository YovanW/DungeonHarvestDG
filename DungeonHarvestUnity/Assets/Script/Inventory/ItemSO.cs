using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.Tilemaps;


[CreateAssetMenu(fileName = "Item", menuName = "Scriptable Objects/Item")]
public class ItemSO : ScriptableObject
{
    public string itemName;
    public TileBase tile;
    public Sprite icon;
    public GameObject prefab;
    public ItemType type;
    public ActionType actionType;
    public Vector2Int range = new Vector2Int(5, 4);
    public bool stackable = true;

    public enum ItemType
    {
        Tool,
        RawOre
    }
    public enum ActionType
    {
        Mine,
        Chop
    }


}