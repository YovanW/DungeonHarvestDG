using UnityEngine;

public class soilHitBox : MonoBehaviour
{
    public InventoryManager inventoryManager = null;
    public GameObject plantArea;
    public bool isRaked = false;
    public bool readyToHarvest = false;
    public int soilQuality = 0;

    public GameObject seedPrefab;
    public GameObject[] cropGrids;


    void Start()
    {
        if (plantArea == null) { Debug.LogWarning("Plant Area is not assigned in the inspector."); }

        if (inventoryManager == null)
        {
            inventoryManager = GameObject.FindGameObjectWithTag("InventoryManager").GetComponent<InventoryManager>();
        }
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


            // // RESET SOIL AFTER HARVESTING
            // isRaked = false;
            // readyToHarvest = false;
            // soilQuality = 0;
            // seedPrefab = null;

            // // delete crops
            // foreach (GameObject grid in cropGrids)
            // {
            //     foreach (Transform child in grid.transform)
            //     {
            //         Destroy(child.gameObject);
            //     }
            // }
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
