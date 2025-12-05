using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventoryManager : MonoBehaviour
{
    public InventorySlot[] inventorySlots;
    public GameObject inventoryItemPrefab;
    public int maxStackSize = 64;

    public void AddItem(ItemSO item)
    {
        // Check for existing stackable item
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            InventorySlot slot = inventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();

            if (itemInSlot != null && itemInSlot.item == item && itemInSlot.count < maxStackSize && itemInSlot.item.stackable == true)
            {
                // Stackable
                itemInSlot.count++;
                itemInSlot.refreshCount();
                return;
            }
        }


        // Find any empty slot
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            InventorySlot slot = inventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();

            if (itemInSlot == null)
            {
                // Empty slot found
                SpawnNewItem(item, slot);

                print("Added item");

                return;
            }
        }

        void SpawnNewItem(ItemSO item, InventorySlot slot)
        {
            GameObject newItemGameObject = Instantiate(inventoryItemPrefab, slot.transform);
            InventoryItem inventoryItem = newItemGameObject.GetComponent<InventoryItem>();
            inventoryItem.InitialiseItem(item);
        }
    }
}
