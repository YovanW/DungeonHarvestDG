using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class InventoryManager : MonoBehaviour
{
    public ItemSO[] allItems;

    public InventorySlot[] inventorySlots;
    public GameObject inventoryItemPrefab;
    public int maxStackSize = 20;
    public static event Action OnInventoryChanged;

    void OnEnable()
    {
        OnInventoryChanged += SaveInventory;
    }

    void OnDisable()
    {
        OnInventoryChanged -= SaveInventory;
    }

    void Awake()
    {
        // LoadInventory();
    }

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

    public void SaveInventory()
    {
        InventorySlotSave[] slots = new InventorySlotSave[inventorySlots.Length];

        for (int i = 0; i < inventorySlots.Length; i++)
        {
            InventoryItem item = inventorySlots[i].GetComponentInChildren<InventoryItem>();

            if (item != null)
            {
                slots[i] = new InventorySlotSave
                {
                    itemName = item.item.itemName,
                    count = item.count
                };
            }
        }

        string json = JsonUtility.ToJson(
            new InventoryWrapper { slots = slots }
        );

        PlayerPrefs.SetString("InventoryData", json);
        PlayerPrefs.SetInt("SavedGame", 1);
        PlayerPrefs.Save();
    }

    public void LoadInventory()
    {
        foreach (var slot in inventorySlots)
        {
            InventoryItem oldItem = slot.GetComponentInChildren<InventoryItem>();
            if (oldItem != null)
                Destroy(oldItem.gameObject);
        }

        if (!PlayerPrefs.HasKey("InventoryData"))
            return;


        if (!PlayerPrefs.HasKey("InventoryData"))
            return;

        string json = PlayerPrefs.GetString("InventoryData");
        InventoryWrapper wrapper = JsonUtility.FromJson<InventoryWrapper>(json);

        for (int i = 0; i < wrapper.slots.Length; i++)
        {
            var data = wrapper.slots[i];
            if (data == null) continue;

            ItemSO item = System.Array.Find(allItems, x => x.itemName == data.itemName);

            if (item == null) continue;

            InventorySlot slot = inventorySlots[i];
            GameObject go = Instantiate(inventoryItemPrefab, slot.transform);
            InventoryItem invItem = go.GetComponent<InventoryItem>();
            invItem.InitialiseItem(item);
            invItem.count = data.count;
            invItem.refreshCount();
        }
    }


}
