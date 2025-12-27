using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    public ObjectDetector ray;


    void Start()
    {

    }

    void Update()
    {
        InventoryMenu inventoryMenu = GameObject.Find("CanvasController").GetComponent<InventoryMenu>();

        if (Input.GetKeyDown(KeyCode.E) && ray.lookingAt != null)
        {
            // Door Interaction
            if (ray.lookingAt.GetComponent<doorMove>())
            {
                doorMove door = ray.lookingAt.GetComponent<doorMove>();
                if (door != null)
                {
                    door.ToggleDoor();
                }
            }

            // Chest Interaction
            if (ray.lookingAt.GetComponent<chestOpen>())
            {
                chestOpen chest = ray.lookingAt.GetComponent<chestOpen>();
                if (chest == null) return;


                if (!chest.isOpen && !inventoryMenu.Inventory.activeSelf)
                {
                    inventoryMenu.OpenChestUI();

                    chest.OpenChest();
                }
                else if (chest.isOpen && inventoryMenu.chestUI.activeSelf)
                {
                    chest.CloseChest();
                    inventoryMenu.closeChestUI();
                    GameObject.Find("CanvasController").GetComponent<InventoryMenu>().isOpen = false;
                }
            }
        }

        // fail-safe checker for Tab close all inventory
        if (Input.GetKeyDown(KeyCode.Tab) && ray.lookingAt != null)
        {
            chestOpen chest = ray.lookingAt.GetComponent<chestOpen>();
            if (chest == null) return;

            if (chest.isOpen && inventoryMenu.chestUI.activeSelf)
            {
                chest.CloseChest();
                chest.isOpen = false;
                inventoryMenu.closeChestUI();
            }
        }
    }
}
