using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class playerFarming : MonoBehaviour
{
    private InventoryManager inventoryManager;
    public ObjectDetector ray;
    public ItemInHand itemHand;
    bool isSwinging = false;
    private float maxRakeDistance = 3f;

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
        if (ray.lookingAt == null || Time.deltaTime == 0) return;

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
                // else { soil.isRaked = false; }
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
                    if (soil.getStatusHarvest()) return;

                    Debug.Log("Trying to plant seed...");

                    // reduce seed count in inventory
                    int slotIndex = inventoryManager.GetComponent<HotbarManager>().selectedIndex;
                    inventoryManager.RemoveItem(selectedSO, slotIndex);

                    TryPlant(soil, seed);
                }
            }
        }

        // Fertilize soil
        if (selectedSO != null && selectedSO.actionType == ItemSO.ActionType.Ferilizer)
        {
            selectedSO = itemHand.getSelectedSO();          // get the item in hand
            soilHitBox soil = ray.lookingAt.GetComponent<soilHitBox>();
            if (soil == null || selectedSO == null) return;

            if (Input.GetKeyDown(KeyCode.E))
            {
                if (soil.isRaked)
                {
                    // Debug.Log("Soil fertilizer slots: " + soil.getFertilizerSlots());
                    if (soil.getStatusHarvest() || soil.getFertilizerSlots() <= 0) return;

                    Debug.Log("Trying to fertilize soil...");

                    // reduce fertilizer count in inventory
                    int slotIndex = inventoryManager.GetComponent<HotbarManager>().selectedIndex;
                    inventoryManager.RemoveItem(selectedSO, slotIndex);

                    TryFertilize(soil, selectedSO);
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
        // sfx
        if (GameAudio.Instance != null)
            GameAudio.Instance.PlaySFX(GameAudio.Instance.harvest);

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

    void TryFertilize(soilHitBox soil, ItemSO fertilizer)
    {
        if (soil.getFertilizerSlots() <= 0) return;

        // fertilize the soil
        soil.soilQuality += fertilizer.extraInfo;
        soil.FertilizeSoil(fertilizer.prefab);
    }

    IEnumerator SwingAnimation(Transform hitpos)
    {
        if (isSwinging) yield break;
        isSwinging = true;

        // sfx
        if (GameAudio.Instance != null)
            GameAudio.Instance.PlaySFX(GameAudio.Instance.rake);

        Transform rake = itemHand.transform;

        Vector3 startPos = rake.localPosition;
        Quaternion startRot = rake.localRotation;

        Quaternion soilRot = Quaternion.Euler(65f, 0f, 12f);
        Vector3 prepPos = startPos + new Vector3(0f, 0.78f, 0.05f);

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * 6f;
            rake.localRotation = Quaternion.Slerp(startRot, soilRot, t);
            rake.localPosition = Vector3.Lerp(startPos, prepPos, t);

            yield return null;
        }

        // pull dirt toward feet 
        Vector3 pullToFeetPos = startPos + new Vector3(0f, -0.08f, -0.35f);
        Quaternion scrapeRot = soilRot * Quaternion.Euler(0f, 0f, 8f);

        t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * 8f;
            rake.localPosition = Vector3.Lerp(prepPos, pullToFeetPos, t);
            rake.localRotation = Quaternion.Slerp(soilRot, scrapeRot, t);
            yield return null;
        }

        // lift rake off ground
        Vector3 liftPos = startPos + new Vector3(0f, 0.12f, -0.1f);

        t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * 6f;
            rake.localPosition = Vector3.Lerp(pullToFeetPos, liftPos, t);
            rake.localRotation = Quaternion.Slerp(scrapeRot, startRot, t);
            yield return null;
        }

        rake.localPosition = startPos;
        rake.localRotation = startRot;

        isSwinging = false;
    }

}
