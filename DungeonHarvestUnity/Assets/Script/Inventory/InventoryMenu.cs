using UnityEngine;

public class InventoryMenu : MonoBehaviour
{
    public GameObject Inventory;
    public bool isOpen = false;
    public GameObject deleteSlot;
    public GameObject playerStats;

    public GameObject chestUI;



    void Start()
    {
        closeInventory();
        chestUI.SetActive(false);
    }

    void Update()
    {
        // invetory open and close
        if (Input.GetKeyDown(KeyCode.Tab))
        {

            if (isOpen)
            {
                closeInventory();
            }
            else
            {
                openInventory();
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }
    }

    public void OpenChestUI()
    {
        Inventory.SetActive(true);
        deleteSlot.SetActive(true);
        playerStats.SetActive(true);
        chestUI.SetActive(true);
        isOpen = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void closeChestUI()
    {
        Inventory.SetActive(false);
        deleteSlot.SetActive(false);
        playerStats.SetActive(false);
        chestUI.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
    }


    public void closeInventory()
    {
        isOpen = false;
        Inventory.SetActive(false);
        deleteSlot.SetActive(false);
        playerStats.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void openInventory()
    {
        isOpen = true;
        Inventory.SetActive(true);
        deleteSlot.SetActive(true);
        playerStats.SetActive(true);
    }
}
