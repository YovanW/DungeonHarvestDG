using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.Tilemaps;
using Microsoft.Unity.VisualStudio.Editor;


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
        Attack,
        None
    }


}