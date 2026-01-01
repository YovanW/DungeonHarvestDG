using UnityEngine;

public class soilHitBox : MonoBehaviour
{
    public GameObject plantArea;
    public bool isRaked = false;
    public bool readyToHarvest = false;
    public int soilQuality = 0;

    public float growthTimer = 0f;
    public float growthDuration = 10f; // seconds
    public string status = "start"; // start, middle, end

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
            // TODO: seed growth cycle
            growthTimer += Time.deltaTime;

            // reduce growth duration by 10% based on soil quality 
            float qualityModifier = 1f - (soilQuality * 0.1f);

            float growthProgress = growthTimer / (growthDuration * qualityModifier) * 100f;

            // if >50% growth, change to middle crop prefab once
            if (growthProgress >= 50f && growthProgress < 100f && status == "start")
            {
                // change to middle crop prefab
                GameObject middlePrefab = seedPrefab.GetComponent<cropOffset>().cropPrefab[1];
                changePrefabStage(middlePrefab);
                status = "middle";
            }

            if (growthTimer >= growthDuration && status == "middle")
            {
                // change to final crop prefab
                GameObject finalPrefab = seedPrefab.GetComponent<cropOffset>().cropPrefab[2];
                changePrefabStage(finalPrefab);

                readyToHarvest = true;
                growthTimer = 0f; // reset timer for next growth cycle
                status = "end";
            }
        }

    }

    public void ResetSoil()
    {
        isRaked = false;
        readyToHarvest = false;
        soilQuality = 0;
        seedPrefab = null;
        growthTimer = 0f;
        status = "start";

        // delete crops
        foreach (GameObject grid in cropGrids)
        {
            foreach (Transform child in grid.transform)
            {
                Destroy(child.gameObject);
            }
        }
    }

    public void changePrefabStage(GameObject newPrefab)
    {
        // change all crops in the grids to the new prefab
        foreach (GameObject grid in cropGrids)
        {
            // destroy existing crop
            foreach (Transform child in grid.transform)
            {
                Destroy(child.gameObject);
            }

            // instantiate new crop prefab
            Vector3 offset = newPrefab.GetComponent<cropOffset>().offset;
            Vector3 randYRot = new Vector3(0, Random.Range(0f, 360f), 0);   // random Y rotation
            Instantiate(newPrefab, grid.transform.position + offset, Quaternion.Euler(randYRot), grid.transform);
        }
    }

    public void PlantSeed(GameObject seed)
    {
        if (isRaked)
        {
            seedPrefab = seed;
            // get crop offset 
            Vector3 offset = seedPrefab.GetComponent<cropOffset>().offset;

            // instantiate the crop grid on the plant area
            foreach (GameObject grid in cropGrids)
            {
                Vector3 randYRot = new Vector3(0, Random.Range(0f, 360f), 0);   // random Y rotation

                // instantiate the crop on the grid + offset + random rotate as child of the grid
                Instantiate(seedPrefab, grid.transform.position + offset, Quaternion.Euler(randYRot), grid.transform);
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
}
