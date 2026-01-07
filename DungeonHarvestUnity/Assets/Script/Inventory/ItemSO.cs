using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.Tilemaps;



[CreateAssetMenu(fileName = "Item", menuName = "Scriptable Objects/Item")]
public class ItemSO : ScriptableObject
{
    public string itemName;
    public Sprite icon;
    public GameObject prefab;
    public ItemType type;
    public ActionType actionType;
    public bool stackable = true;
    public int miningPower;
    public int extraInfo;

    // item offsett in hand
    public Vector3 itemOffsetInHand;

    public enum ItemType
    {
        Material,
        Armor,
        Tool,
        RawOre,
        RefinedOre,
        Fuel,
        Seed,
        Consumable,
    }
    public enum ActionType
    {
        Mine,
        Chop,
        Rake,
        Attack,
        Ferilizer,
        None
    }


}