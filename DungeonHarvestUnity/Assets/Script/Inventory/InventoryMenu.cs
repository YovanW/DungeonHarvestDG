using UnityEngine;

public class InventoryMenu : MonoBehaviour
{
    public GameObject Inventory;
    private bool isOpen = false;
    public GameObject deleteSlot;
    public GameObject playerStats;

    public GameObject chestUI;



    void Start()
    {
        closeAll();
    }

    void Update()
    {
        // invetory open and close
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (isOpen)
            {
                closeAll();
            }
            else
            {
                isOpen = true;
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


    public void closeAll()
    {
        isOpen = false;
        Inventory.SetActive(false);
        deleteSlot.SetActive(false);
        playerStats.SetActive(false);
        chestUI.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void openInventory()
    {
        Inventory.SetActive(true);
        deleteSlot.SetActive(true);
        playerStats.SetActive(true);
    }
}
