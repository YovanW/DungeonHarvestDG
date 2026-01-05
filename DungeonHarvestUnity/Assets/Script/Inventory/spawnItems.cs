using UnityEngine;

public class spawnItems : MonoBehaviour
{
    public InventoryManager inventoryManager;
    public ItemSO[] itemsToPickup;

    public void PickupItem(int id)
    {
        inventoryManager.AddItem(itemsToPickup[id]);
    }

}
