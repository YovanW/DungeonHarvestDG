using UnityEngine;

public class InventoryMenu : MonoBehaviour
{
    public GameObject Inventory;
    private bool isOpen = false;
    public GameObject deleteSlot;
    public GameObject playerStats;


    void Start()
    {
        Inventory.SetActive(false);
        deleteSlot.SetActive(false);
        playerStats.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (isOpen)
            {
                isOpen = false;
                Inventory.SetActive(false);
                deleteSlot.SetActive(false);
                playerStats.SetActive(false);
                Cursor.lockState = CursorLockMode.Locked;
            }
            else
            {
                isOpen = true;
                Inventory.SetActive(true);
                deleteSlot.SetActive(true);
                playerStats.SetActive(true);
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }
    }

}
