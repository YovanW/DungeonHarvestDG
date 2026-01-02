using UnityEngine;

public class soilHitBox : MonoBehaviour
{
    public GameObject plantArea;
    public bool isRaked = false;
    public bool readyToHarvest = false;
    public int soilQuality = 0;
    public int fertilizerSlots = 2;

    public float growthTimer = 0f;
    public float growthDuration = 10f; // seconds

    // status
    public enum GrowthStatus { Start, Middle, End }
    public GrowthStatus status = GrowthStatus.Start;

    public GameObject seedPrefab;
    public GameObject[] cropGrids;


    void Start()
    {
        if (plantArea == null) { Debug.LogWarning("Plant Area is not assigned in the inspector."); }
    }

    void Update()
    {
        if (isRaked)
            plantArea.SetActive(true);         // show the plant area
        else
            plantArea.SetActive(false);        // hide the plant area

        if (isRaked && seedPrefab != null)
        {
            if (readyToHarvest) return;

            // seed growth cycle
            growthTimer += Time.deltaTime;

            // reduce growth duration by 5% based on soil quality 
            float qualityModifier = Mathf.Max(0f, 1f - soilQuality * 0.05f);

            float growthProgress = growthTimer / (growthDuration * qualityModifier) * 100f;

            // if >50% growth, change to middle crop prefab once
            if (growthProgress >= 50f && growthProgress < 100f && status == GrowthStatus.Start)
            {
                // change to middle crop prefab
                GameObject middlePrefab = seedPrefab.GetComponent<cropOffset>().cropPrefab[1];
                changePrefabStage(middlePrefab);
                status = GrowthStatus.Middle;
            }

            if (growthProgress >= 100f && status == GrowthStatus.Middle)
            {
                // change to final crop prefab
                GameObject finalPrefab = seedPrefab.GetComponent<cropOffset>().cropPrefab[2];
                changePrefabStage(finalPrefab);

                readyToHarvest = true;
                status = GrowthStatus.End;
            }
        }

    }



    public bool getStatusHarvest()
    {
        return readyToHarvest;
    }

    public bool getFertilizerSlots()
    {
        return fertilizerSlots > 0;
    }

    public void ResetSoil()
    {
        isRaked = false;
        readyToHarvest = false;
        soilQuality = 0;
        fertilizerSlots = 2;
        seedPrefab = null;
        growthTimer = 0f;
        status = GrowthStatus.Start;

        // delete crops
        foreach (GameObject grid in cropGrids)
        {
            foreach (Transform child in grid.transform)
            {
                if (child != null)
                    Destroy(child.gameObject);
            }
        }
    }

    public void changePrefabStage(GameObject newPrefab)
    {
        // change all crops in the grids to the new prefab
        for (int i = 0; i <= 2; i++)
        {
            // destroy existing crop
            foreach (Transform child in cropGrids[i].transform)
            {
                Destroy(child.gameObject);
            }

            // instantiate new crop prefab
            Vector3 offset = newPrefab.GetComponent<cropOffset>().offset;
            Vector3 randYRot = new Vector3(0, Random.Range(0f, 360f), 0);   // random Y rotation
            Instantiate(newPrefab, cropGrids[i].transform.position + offset, Quaternion.Euler(randYRot), cropGrids[i].transform);
        }
    }

    public void PlantSeed(GameObject seed)
    {
        if (isRaked)
        {
            seedPrefab = seed;
            growthTimer = 0f;
            readyToHarvest = false;
            status = GrowthStatus.Start;

            // get crop offset 
            Vector3 offset = seedPrefab.GetComponent<cropOffset>().offset;

            // instantiate the crop grid on the plant area only 0,1,2
            for (int i = 0; i <= 2; i++)
            {
                GameObject cropPrefab = seedPrefab.GetComponent<cropOffset>().cropPrefab[0];
                Vector3 randYRot = new Vector3(0, Random.Range(0f, 360f), 0);   // random Y rotation

                Instantiate(cropPrefab, cropGrids[i].transform.position + offset, Quaternion.Euler(randYRot), cropGrids[i].transform);
            }

            // // testing
            // GameObject seedStart = seedPrefab.GetComponent<cropOffset>().cropPrefab[0];
            // GameObject seedMiddle = seedPrefab.GetComponent<cropOffset>().cropPrefab[1];
            // GameObject seedEnd = seedPrefab.GetComponent<cropOffset>().cropPrefab[2];

            // Instantiate(seedStart, cropGrids[0].transform.position + seedStart.GetComponent<cropOffset>().offset, Quaternion.identity, cropGrids[0].transform);
            // Instantiate(seedMiddle, cropGrids[1].transform.position + seedMiddle.GetComponent<cropOffset>().offset, Quaternion.identity, cropGrids[1].transform);
            // Instantiate(seedEnd, cropGrids[2].transform.position + seedEnd.GetComponent<cropOffset>().offset, Quaternion.identity, cropGrids[2].transform);
        }
    }

    public void FertilizeSoil(GameObject fertilizer)
    {
        if (fertilizerSlots > 0)
        {
            Vector3 offset = fertilizer.GetComponent<fertilizerOffset>().offset;
            Vector3 scale = fertilizer.GetComponent<fertilizerOffset>().scale;

            // instantiate fertilizer prefab on crop grid 3 or 4
            int slot = 3;
            if (fertilizerSlots == 1) slot = 4;

            Vector3 randYRot = new Vector3(0, Random.Range(0f, 360f), 0);   // random Y rotation

            GameObject fertInstance = Instantiate(fertilizer, cropGrids[slot].transform.position + offset, Quaternion.identity, cropGrids[slot].transform);
            fertInstance.transform.localScale = scale;

            // apply random Y rotation to cropGrid
            cropGrids[slot].transform.rotation = Quaternion.Euler(randYRot);

            fertilizerSlots--;
        }
    }
}
