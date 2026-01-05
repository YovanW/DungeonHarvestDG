using UnityEngine;


[System.Serializable]
public struct ItemAmount
{
    public ItemSO item;
    [Range(1, 20)] public int amount;
}

[CreateAssetMenu(fileName = "New Crafting Recipe", menuName = "Crafting/Recipe")]
public class CraftingRecipe : ScriptableObject
{
    public ItemAmount[] Material;
    public ItemAmount[] Result;
}
