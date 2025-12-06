using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    public ObjectDetector ray;


    void Start()
    {

    }

    void Update()
    {
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

                Debug.Log(chest.isOpen);

                if (!chest.isOpen)
                {
                    GameObject inventoryMenu = GameObject.Find("CanvasController");
                    inventoryMenu.GetComponent<InventoryMenu>().OpenChestUI();

                    chest.OpenChest();
                }
                else
                {
                    chest.CloseChest();
                }
            }
        }

        // fail-safe checker for Tab close all inventory
        if (Input.GetKeyDown(KeyCode.Tab) && ray.lookingAt != null)
        {
            chestOpen chest = ray.lookingAt.GetComponent<chestOpen>();
            if (chest == null) return;

            if (chest.isOpen)
            {
                chest.CloseChest();
            }
        }
    }
}
