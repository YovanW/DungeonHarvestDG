[System.Serializable]
public class InventorySlotSave
{
    public string itemName;
    public int count;
}

[System.Serializable]
public class InventoryWrapper
{
    public InventorySlotSave[] slots;
}
