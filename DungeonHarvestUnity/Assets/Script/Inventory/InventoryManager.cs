using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class InventoryManager : MonoBehaviour
{
    public InventorySlot[] inventorySlots;
    public GameObject inventoryItemPrefab;
    public int maxStackSize = 20;
    public static event Action OnInventoryChanged;


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
                NotifyChange();
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
                NotifyChange();
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
            NotifyChange();
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
                NotifyChange();
                return;
            }
        }
    }

    public bool HasEnoughItem(ItemSO item, int requiredAmount)
    {
        int total = 0;

        foreach (var slot in inventorySlots)
        {
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot != null && itemInSlot.item == item)
            {
                total += itemInSlot.count;
                if (total >= requiredAmount)
                    return true;
            }
        }

        return false;
    }

    public bool HasMaterials(CraftingRecipe recipe)
    {
        foreach (var mat in recipe.Material)
        {
            if (!HasEnoughItem(mat.item, mat.amount))
                return false;
        }
        return true;
    }

    void NotifyChange()
    {
        OnInventoryChanged?.Invoke();
    }
}
