using UnityEngine;

public class HotbarManager : MonoBehaviour
{
    public InventorySlot[] inventorySlots;
    public GameObject selectedImage;
    public int selectedIndex = 0;

    void Start()
    {
        // default select first slot
        SelectSlot(0);
    }

    void Update()
    {
        if (Time.timeScale == 0f) return;
        
        ShortcutSelectSlot();

        // scroll wheel to change selected slot
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll < 0f)
        {
            SelectSlot((selectedIndex + 1) % inventorySlots.Length);
        }
        else if (scroll > 0f)
        {
            SelectSlot((selectedIndex - 1 + inventorySlots.Length) % inventorySlots.Length);
        }

    }

    public ItemSO getSelectedItem()
    {
        InventorySlot slot = inventorySlots[selectedIndex];
        InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();

        if (itemInSlot != null)
        {
            return itemInSlot.item;
        }

        // Debug.Log("No item in selected slot");
        return null;
    }

    public void SelectSlot(int slotIndex)
    {
        if (slotIndex == selectedIndex) return;

        selectedIndex = slotIndex;

        Transform slot = inventorySlots[slotIndex].transform;

        // move under the same slot
        selectedImage.transform.SetParent(slot);
        selectedImage.transform.localPosition = Vector3.zero;

        // find the item image and place the selector directly underneath it in the hierarchy
        InventoryItem item = slot.GetComponentInChildren<InventoryItem>();
        if (item != null)
        {
            int itemIndex = item.transform.GetSiblingIndex();
            selectedImage.transform.SetSiblingIndex(itemIndex);
        }
        else
        {
            // no item in slot: put selector above background
            selectedImage.transform.SetAsFirstSibling();
        }
    }

    public void ShortcutSelectSlot()
    {
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                SelectSlot(i);
            }
        }
    }

}
