[System.Serializable]
public class ChestItemSave
{
    public string itemName;
    public int count;
    public int slotIndex; 
}


[System.Serializable]
public class ChestSave
{
    public string chestID;
    public ChestItemSave[] items;
}

[System.Serializable]
public class AllChestSave
{
    public ChestSave[] allChests;
}
