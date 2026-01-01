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

    public void RemoveItem(ItemSO item, int slotIndex = -1)
    {
        if (slotIndex != -1)
        {
            // Remove item from specific slot
            InventorySlot slot = inventorySlots[slotIndex];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();

            if (itemInSlot != null && itemInSlot.item == item)
            {
                if (itemInSlot.count > 1)
                {
                    // Reduce count
                    itemInSlot.count--;
                    itemInSlot.refreshCount();
                }
                else
                {
                    // Remove item
                    Destroy(itemInSlot.gameObject);
                }
            }
            return;
        }

        // Find the item in inventory (no specific slot)
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            InventorySlot slot = inventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();

            if (itemInSlot != null && itemInSlot.item == item)
            {
                if (itemInSlot.count > 1)
                {
                    // Reduce count
                    itemInSlot.count--;
                    itemInSlot.refreshCount();
                }
                else
                {
                    // Remove item
                    Destroy(itemInSlot.gameObject);
                }
                return;
            }
        }
    }
}
