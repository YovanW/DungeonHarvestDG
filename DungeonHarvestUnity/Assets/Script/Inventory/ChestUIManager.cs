using UnityEngine;
using System.Collections.Generic;

public class ChestUIManager : MonoBehaviour
{
    public InventorySlot[] inventorySlots;
    public GameObject inventoryItemPrefab;


    public void AddChestItem(ItemSO item, int count = 1)
    {
        List<InventorySlot> emptySlots = new List<InventorySlot>();

        for (int i = 0; i < inventorySlots.Length; i++)
        {
            InventorySlot slot = inventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();

            if (itemInSlot == null)
                emptySlots.Add(slot);
        }

        if (emptySlots.Count == 0) return;

        int randomIndex = Random.Range(0, emptySlots.Count);
        InventorySlot randomSlot = emptySlots[randomIndex];

        SpawnNewItemChest(item, count, randomSlot);
    }


    public void AddChestItemToSlot(ItemSO item, int count, int slotIndex)
    {
        InventorySlot targetSlot = inventorySlots[slotIndex];
        SpawnNewItemChest(item, count, targetSlot);
    }


    void SpawnNewItemChest(ItemSO itemData, int count, InventorySlot slot)
    {
        GameObject newItemGameObject = Instantiate(inventoryItemPrefab, slot.transform);
        InventoryItem inventoryItem = newItemGameObject.GetComponent<InventoryItem>();
        inventoryItem.InitialiseItem(itemData);
        inventoryItem.count = count;
        inventoryItem.refreshCount();
    }


    public void ClearChestItems()
    {
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            InventoryItem existing = inventorySlots[i].GetComponentInChildren<InventoryItem>();
            if (existing != null)
                Destroy(existing.gameObject);
        }
    }
}
