using System;
using TMPro;
using UnityEngine;

public class ObjectDetector : MonoBehaviour
{
    public GameObject lookingAt;
    private GameObject lastLookingAt;
    public ItemInHand itemInHand;
    public InventoryMenu inventoryMenu;

    public Vector3 collision = Vector3.zero;
    public float maxInteractRange = 2.5f;

    // data untuk PlayerMining script
    public bool hitSomething;
    public RaycastHit hitInfoPublic;

    public TextMeshProUGUI intractInfo;

    void Start()
    {
        inventoryMenu = GameObject.FindGameObjectWithTag("CanvasController").GetComponent<InventoryMenu>();
    }

    void Update()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, maxInteractRange))
        {
            hitSomething = true;
            hitInfoPublic = hit;

            lookingAt = hit.transform.gameObject;
            collision = hit.point;

            if (lookingAt != lastLookingAt)
            {
                Debug.Log("Looking at: " + lookingAt.name);
                lastLookingAt = lookingAt;
            }
        }
        else
        {
            hitSomething = false;
            lookingAt = null;

            // // always update gizmo for testing
            // collision = transform.position + transform.forward * maxInteractRange;
        }

        // Hide intractInfo when inventory is open
        intractInfo.gameObject.SetActive(!inventoryMenu.isOpen);

        UpdateIntractInfo();

    }

    void UpdateIntractInfo()
    {
        // Default Empty
        intractInfo.text = "";
        if (lookingAt == null) { return; }

        // Chest
        if (lookingAt.TryGetComponent(out chestOpen chest))
        {
            intractInfo.text = "Press \"E\" to Open Chest";
            return;
        }

        // Door
        if (lookingAt.TryGetComponent(out doorMove door))
        {
            intractInfo.text = door.isDoorOpenNow()
                ? "Press \"E\" to Close"
                : "Press \"E\" to Open";
            return;
        }

        // Farming Soil 
        if (lookingAt.TryGetComponent(out soilHitBox soil))
        {

            ItemSO selected = itemInHand.getSelectedSO();

            // if (!soil.isRaked && selected != null && selected.actionType == ItemSO.ActionType.Rake)
            // {
            //     intractInfo.text = "Press Left Click to Rake Soil";
            // }

            if (soil.isRaked && soil.seedPrefab == null && selected != null && selected.type == ItemSO.ItemType.Seed)
            {
                intractInfo.text = "Press \"E\" to Plant Seed";
            }
            else if (soil.readyToHarvest)
            {
                intractInfo.text = "Press \"E\" to Harvest Crop";
            }


            return;
        }
    }


    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(collision, 0.05f);
    }

}
