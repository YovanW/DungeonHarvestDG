using System;
using System.Collections.Generic;
using UnityEngine;

public enum ChestMode { Random, Fixed }

public class chestOpen : MonoBehaviour
{
    public ChestMode chestMode = ChestMode.Random;

    [Header("Fixed Chest Items")]
    public List<FixedChestItem> fixedItems;

    [Header("Random Chest Items")]
    public List<ItemSO> lootTable;
    public int maxItems = 5;

    private List<InventoryItem> generatedItems = new();
    private List<SavedItemSlot> savedSlots = new();

    private ChestUIManager chestUIManager;
    public bool isOpen;


    class SavedItemSlot
    {
        public InventoryItem item;
        public int slot;

        public SavedItemSlot(InventoryItem item, int slot)
        {
            this.item = item;
            this.slot = slot;
        }
    }


    void Start()
    {
        chestUIManager = GameObject.FindGameObjectWithTag("InventoryManager")
            .GetComponent<ChestUIManager>();

        if (HasSavedChest())
        {
            GetComponent<chestOpenAnimation>().openState();
            return;
        }


        // generate items once
        if (chestMode == ChestMode.Random)
            GenerateRandomItems();
        else
            GenerateFixedItems();
    }


    void GenerateRandomItems()
    {
        generatedItems.Clear();

        for (int i = 0; i < maxItems; i++)
        {
            ItemSO itemSO = lootTable[UnityEngine.Random.Range(0, lootTable.Count)];

            generatedItems.Add(new InventoryItem
            {
                item = itemSO,
                count = itemSO.stackable ? UnityEngine.Random.Range(1, 10) : 1
            });
        }
    }

    void GenerateFixedItems()
    {
        generatedItems.Clear();

        foreach (var data in fixedItems)
        {
            if (data.item == null) continue;

            generatedItems.Add(new InventoryItem
            {
                item = data.item,
                count = Mathf.Max(1, data.count)
            });
        }
    }


    public void OpenChest()
    {
        if (isOpen) return;

        chestUIManager.ClearChestItems();
        LoadSavedSlots();

        // First time open (no save yet)
        if (savedSlots.Count == 0)
        {
            GetComponent<chestOpenAnimation>().openState();

            foreach (var item in generatedItems)
                chestUIManager.AddChestItem(item.item, item.count);
        }
        else
        {
            LoadItemsToUI();
        }

        GameObject.Find("CanvasController")
            .GetComponent<InventoryMenu>().isOpen = true;

        isOpen = true;
    }

    public void CloseChest()
    {
        if (!isOpen) return;

        SaveItemsFromUI();
        SaveChestToFile();

        chestUIManager.ClearChestItems();
        isOpen = false;
    }


    void LoadItemsToUI()
    {
        foreach (var slot in savedSlots)
        {
            chestUIManager.AddChestItemToSlot(
                slot.item.item,
                slot.item.count,
                slot.slot
            );
        }
    }

    void SaveItemsFromUI()
    {
        savedSlots.Clear();

        for (int i = 0; i < chestUIManager.inventorySlots.Length; i++)
        {
            InventoryItem item = chestUIManager.inventorySlots[i].GetComponentInChildren<InventoryItem>();

            if (item == null) continue;

            savedSlots.Add(new SavedItemSlot(new InventoryItem { item = item.item, count = item.count }, i));
        }
    }


    void SaveChestToFile()
    {
        AllChestSave allData = LoadAllChests();

        ChestSave chestSave = new ChestSave
        {
            chestID = gameObject.name,
            items = new ChestItemSave[savedSlots.Count]
        };

        for (int i = 0; i < savedSlots.Count; i++)
        {
            chestSave.items[i] = new ChestItemSave
            {
                itemName = savedSlots[i].item.item.itemName,
                count = savedSlots[i].item.count,
                slotIndex = savedSlots[i].slot
            };
        }

        int index = Array.FindIndex(allData.allChests, c => c.chestID == chestSave.chestID);

        if (index >= 0)
            allData.allChests[index] = chestSave;
        else
        {
            var list = new List<ChestSave>(allData.allChests);
            list.Add(chestSave);
            allData.allChests = list.ToArray();
        }

        PlayerPrefs.SetString("allChestSaved", JsonUtility.ToJson(allData, true));
        PlayerPrefs.Save();
    }

    void LoadSavedSlots()
    {
        savedSlots.Clear();

        AllChestSave allData = LoadAllChests();
        ChestSave chestSave = Array.Find(allData.allChests, c => c.chestID == gameObject.name);

        if (chestSave == null) return;

        InventoryManager inv = GameObject.FindGameObjectWithTag("InventoryManager").GetComponent<InventoryManager>();

        foreach (var data in chestSave.items)
        {
            ItemSO itemSO = Array.Find(inv.allItems, x => x.itemName == data.itemName);

            if (itemSO == null) continue;

            savedSlots.Add(new SavedItemSlot(new InventoryItem { item = itemSO, count = data.count }, data.slotIndex));
        }
    }

    bool HasSavedChest()
    {
        AllChestSave allData = LoadAllChests();
        return Array.Exists(allData.allChests, c => c.chestID == gameObject.name);
    }

    AllChestSave LoadAllChests()
    {
        if (!PlayerPrefs.HasKey("allChestSaved"))
            return new AllChestSave { allChests = new ChestSave[0] };

        return JsonUtility.FromJson<AllChestSave>(
            PlayerPrefs.GetString("allChestSaved")
        );
    }

}
