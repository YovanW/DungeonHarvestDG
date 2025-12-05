using UnityEngine;

public class InventoryMenu : MonoBehaviour
{
    public GameObject InventorySlot;
    private bool isOpen = false;

    void Start()
    {
        InventorySlot.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (isOpen)
            {
                isOpen = false;
                InventorySlot.SetActive(false);
                Cursor.lockState = CursorLockMode.Locked;
                Time.timeScale = 1f;
            }
            else
            {
                isOpen = true;
                InventorySlot.SetActive(true);
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                Time.timeScale = 0f;
            }
        }
    }

}
