using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class playerFarming : MonoBehaviour
{
    public InventoryManager inventoryManager;
    public ObjectDetector ray;
    public ItemInHand itemHand;
    bool isSwinging = false;
    private float maxRakeDistance = 2.5f;

    void Start()
    {

        if (inventoryManager == null)
        {
            inventoryManager = GameObject.FindGameObjectWithTag("InventoryManager").GetComponent<InventoryManager>();
        }

    }
    void Update()
    {
        var selectedSO = itemHand.getSelectedSO();      // get the item in hand
        if (ray.lookingAt == null) return;              // get the object player is looking at

        // Rake soil
        if (selectedSO != null && selectedSO.actionType == ItemSO.ActionType.Rake)
        {
            soilHitBox soil = ray.lookingAt.GetComponent<soilHitBox>();
            if (soil == null) return;

            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                if (isSwinging) return;

                if (!soil.isRaked)
                {
                    TryRake(soil);
                }

                // buat testing
                else { soil.isRaked = false; }
            }

        }

        // Plant seed
        if (selectedSO != null && selectedSO.type == ItemSO.ItemType.Seed)
        {
            GameObject seed = selectedSO.prefab;
            soilHitBox soil = ray.lookingAt.GetComponent<soilHitBox>();
            if (soil == null || seed == null) return;

            if (Input.GetKeyDown(KeyCode.E))
            {
                if (soil.isRaked && soil.seedPrefab == null)
                {
                    Debug.Log("Trying to plant seed...");

                    // reduce seed count in inventory
                    int slotIndex = inventoryManager.GetComponent<HotbarManager>().selectedIndex;
                    inventoryManager.RemoveItem(selectedSO, slotIndex);

                    TryPlant(soil, seed);
                }
            }


        }

        // Harvest crop 
        if (ray.lookingAt != null && ray.lookingAt.GetComponent<soilHitBox>() != null)
        {
            soilHitBox soil = ray.lookingAt.GetComponent<soilHitBox>();
            if (soil == null || !soil.readyToHarvest) return;

            if (Input.GetKeyDown(KeyCode.E))
            {
                TryHarvest(soil);
            }
        }


    }

    void TryHarvest(soilHitBox soil)
    {
        float dist = Vector3.Distance(transform.position, soil.transform.position);
        if (dist > maxRakeDistance) return;

        // add harvested crops to inventory
        ItemSO cropPrefab = soil.seedPrefab.GetComponent<harvestPrefab>().harvestItem;
        ItemSO seedItem = soil.seedPrefab.GetComponent<harvestPrefab>().seedItem;
        if (cropPrefab != null && seedItem != null)
        {
            // harvested item
            int quantity = 3;
            for (int i = 0; i < quantity; i++) inventoryManager.AddItem(cropPrefab);

            // seed item (drop chance 30%)
            int seedDropChance = Random.Range(0, 100);
            if (seedDropChance < 30)
                inventoryManager.AddItem(seedItem);

            // reset soil state
            soil.ResetSoil();
        }
    }

    void TryRake(soilHitBox soil)
    {
        float dist = Vector3.Distance(transform.position, soil.transform.position);
        if (dist > maxRakeDistance) return;

        StartCoroutine(SwingAnimation(soil.transform));

        soil.isRaked = true;
        soil.soilQuality = itemHand.getSelectedSO().extraInfo;
    }

    void TryPlant(soilHitBox soil, GameObject seed)
    {
        float dist = Vector3.Distance(transform.position, soil.transform.position);
        if (dist > maxRakeDistance) return;

        // plant seed in the soil
        soil.PlantSeed(seed);
    }


    IEnumerator SwingAnimation(Transform hitpos)
    {
        // stop spam action
        if (isSwinging) yield break;
        isSwinging = true;

        // TODO: rake swing animation
        // yield return new WaitForSeconds(1f);

        isSwinging = false;
        yield return null;
    }
}
