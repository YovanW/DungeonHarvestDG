using UnityEngine;

public class PauseSync : MonoBehaviour
{
    private bool pauseMenu;
    private bool settingCanvas;
    private bool inventoryMenu;


    void Update()
    {
        pauseMenu = GetComponent<PauseMenu>().pauseMenu.activeSelf;
        settingCanvas = GetComponent<PauseMenu>().settingCanvas.activeSelf;
        inventoryMenu = GetComponent<InventoryMenu>().Inventory.activeSelf;

        if (pauseMenu || settingCanvas || inventoryMenu)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
        }
    }
}
