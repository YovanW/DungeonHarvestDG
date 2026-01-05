using System.Collections.Generic;
using UnityEngine;

public enum ChestMode { Random, Fixed }


public class chestOpen : MonoBehaviour
{
    public ChestMode chestMode = ChestMode.Random;
    [Header("Fixed Chest Items")]
    public List<FixedChestItem> fixedItems;

    [Header("Random Chest Items")]
    public List<InventorySlot> chestInventory;
    public List<ItemSO> lootTable;
    public int maxItems = 5;
    private List<InventoryItem> currentItems;
    public bool isOpen = false;
    private ChestUIManager chestUIManager;
    private bool generatedOnce;
    private bool saveOnce;

    class savedItemSlot
    {
        public InventoryItem item;
        public int slot;

        public savedItemSlot(InventoryItem item, int slot)
        {
            this.item = item;
            this.slot = slot;
        }
    }

    private List<savedItemSlot> saveItemInSlot = new List<savedItemSlot>();

    void Awake()
    {
        chestUIManager = GameObject.FindGameObjectWithTag("InventoryManager").GetComponent<ChestUIManager>();

        if (generatedOnce) return;
        currentItems = new List<InventoryItem>();
        if (chestMode == ChestMode.Random)
        {
            GenerateChestItems();
        }
        else
        {
            GenerateFixedItems();
        }

        generatedOnce = true;
    }

    void GenerateFixedItems()
    {
        foreach (var data in fixedItems)
        {
            if (data.item == null) continue;

            InventoryItem item = new InventoryItem();
            item.item = data.item;
            item.count = Mathf.Max(1, data.count);

            currentItems.Add(item);
        }
    }


    public void printChestItems()
    {
        foreach (var item in currentItems)
        {
            Debug.Log("Item: " + item.item.itemName + ", Count: " + item.count);
        }
    }

    void GenerateChestItems()
    {
        currentItems = new List<InventoryItem>();

        for (int i = 0; i < maxItems; i++)
        {
            int randomIndex = Random.Range(0, lootTable.Count);
            ItemSO itemData = lootTable[randomIndex];

            InventoryItem item = new InventoryItem();
            item.item = itemData;

            if (itemData.stackable)
                item.count = Random.Range(1, 10);
            else
                item.count = 1;

            currentItems.Add(item);
        }
    }

    public void OpenChest()
    {
        if (isOpen) return;

        chestUIManager.ClearChestItems();

        // if first time opening, load from generated items
        if (!saveOnce)
        {
            // open chest
            this.GetComponent<chestOpenAnimation>().openState();

            for (int i = 0; i < currentItems.Count; i++)
                chestUIManager.AddChestItem(currentItems[i].item, currentItems[i].count);

            saveOnce = true;
        }
        else
        {
            LoadItemsToUI();
        }


        GameObject.Find("CanvasController").GetComponent<InventoryMenu>().isOpen = true;
        isOpen = true;
    }

    public void CloseChest()
    {
        if (!isOpen) return;

        SaveItemsFromUI();
        chestUIManager.ClearChestItems();
        isOpen = false;

    }

    public void LoadItemsToUI()
    {
        for (int i = 0; i < saveItemInSlot.Count; i++)
        {
            savedItemSlot savedSlot = saveItemInSlot[i];
            chestUIManager.AddChestItemToSlot(savedSlot.item.item, savedSlot.item.count, savedSlot.slot);
        }
    }

    public void SaveItemsFromUI()
    {
        saveItemInSlot.Clear();

        for (int i = 0; i < chestUIManager.inventorySlots.Length; i++)
        {
            InventoryItem itemInSlot = chestUIManager.inventorySlots[i].GetComponentInChildren<InventoryItem>();

            if (itemInSlot != null)
            {
                InventoryItem saved = new InventoryItem();
                saved.item = itemInSlot.item;
                saved.count = itemInSlot.count;

                saveItemInSlot.Add(new savedItemSlot(saved, i));
            }
        }
    }
}