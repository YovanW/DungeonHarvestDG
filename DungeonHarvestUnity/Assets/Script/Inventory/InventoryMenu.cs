using UnityEngine;

public class InventoryMenu : MonoBehaviour
{
    public GameObject Inventory;
    private bool isOpen = false;

    void Start()
    {
        Inventory.SetActive(false);
    }

    void Update()
    {
        if (Inventory.activeSelf == true)
        {
            // Time.timeScale = 0f;
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (isOpen)
            {
                isOpen = false;
                Inventory.SetActive(false);
                Cursor.lockState = CursorLockMode.Locked;
            }
            else
            {
                isOpen = true;
                Inventory.SetActive(true);
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }
    }

}
