using UnityEngine;

public class openCraftMenu : MonoBehaviour
{
    private GameObject inventoryManager;
    public CraftingType stationType; 

    void Start()
    {
        if (inventoryManager == null)
            inventoryManager = GameObject.FindGameObjectWithTag("InventoryManager");
    }

    public void Open()
    {
        inventoryManager.GetComponent<CraftingUIManager>().SetCraftingType(stationType);
    }
}
