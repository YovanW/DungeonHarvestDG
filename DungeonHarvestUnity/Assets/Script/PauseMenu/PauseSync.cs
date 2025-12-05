using UnityEngine;

public class PauseSync : MonoBehaviour
{
    private bool pauseMenu;
    private bool settingCanvas;
    private bool inventoryMenu;

    private GameObject crosshair;

    void Update()
    {
        pauseMenu = GetComponent<PauseMenu>().pauseMenu.activeSelf;
        settingCanvas = GetComponent<PauseMenu>().settingCanvas.activeSelf;
        inventoryMenu = GetComponent<InventoryMenu>().Inventory.activeSelf;

        if (pauseMenu || settingCanvas || inventoryMenu)
        {
            Time.timeScale = 0f;

            // hide crosshair
            if (crosshair == null) crosshair = GameObject.Find("Crosshair");

            if (crosshair != null)
            {
                crosshair.SetActive(false);
            }

        }
        else
        {
            Time.timeScale = 1f;

            // show crosshair
            if (crosshair != null)
            {
                crosshair.SetActive(true);
            }
        }
    }
}
