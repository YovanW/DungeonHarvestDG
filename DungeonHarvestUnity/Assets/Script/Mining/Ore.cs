using System.Collections;
using UnityEngine;

public class Ore : MonoBehaviour
{
    public oreData oreData;
    public InventoryManager inventoryManager = null;
    public ItemSO[] itemsToPickup;

    private float spawnTimer = 0f;
    private int health;

    void Start()
    {
        health = oreData.hardness * 3;

        if (inventoryManager == null)
        {
            Debug.LogWarning("InventoryManager not assigned in Ore script.");
        }
    }

    void Update()
    {
        // Spawn timer logic
        if (spawnTimer > 0f)
        {
            // Debug.Log("Spawn timer: " + spawnTimer);
            spawnTimer -= Time.deltaTime;

            if (spawnTimer <= 0f)
            {
                // respawn the ore
                ShowOre();
                health = oreData.hardness * 3; // reset health
            }
        }
    }

    public void Mine(int miningPower)
    {
        Debug.Log("Power : " + miningPower);


        // cek apakah pickaxe cukup kuat
        if (miningPower < oreData.hardness)
        {
            Debug.Log("Ore hardnes: " + oreData.hardness + " is too high for pickaxe power: " + miningPower);
        }
        else
        {
            health -= miningPower;

            // play hit animation
            StartCoroutine(HitAnimation());

            // TODO: play sfx mining

        }

        // Ore mined logic
        if (health <= 0)
        {
            if (inventoryManager != null)
                SpawnDrops();

            // hide + start respawn timer
            HideOre();
        }
    }

    IEnumerator HitAnimation()
    {
        // TODO: add animation

        yield return null;
    }


    void HideOre()
    {
        spawnTimer = oreData.respawnTime;

        foreach (Collider col in GetComponentsInChildren<Collider>())
            col.enabled = false;

        foreach (Renderer rend in GetComponentsInChildren<Renderer>())
            rend.enabled = false;
    }

    void ShowOre()
    {
        foreach (Collider col in GetComponentsInChildren<Collider>())
            col.enabled = true;

        foreach (Renderer rend in GetComponentsInChildren<Renderer>())
            rend.enabled = true;
    }

    void SpawnDrops()
    {
        int dropCount = Random.Range(1, oreData.dropAmount + 1);

        for (int i = 0; i < dropCount; i++)
        {
            // TODO: spawn mining ore drop
            int itemId = Random.Range(0, itemsToPickup.Length);
            inventoryManager.AddItem(itemsToPickup[itemId]);
        }
    }
}
