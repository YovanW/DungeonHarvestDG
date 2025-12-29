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
        if (inventoryManager == null)
        {
            inventoryManager = GameObject.FindGameObjectWithTag("InventoryManager").GetComponent<InventoryManager>();
        }

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

            // Start coroutine that handles hit animation and ore destruction
            StartCoroutine(HitAndCheckOre());

        }
    }

    IEnumerator HitAndCheckOre()
    {
        yield return HitAnimation(); // tunggu animasi selesai

        if (health <= 0)
        {
            if (inventoryManager != null)
                SpawnDrops();

            HideOre();
        }
    }


    IEnumerator HitAnimation()
    {
        yield return new WaitForSeconds(0.14f); // start delay (tunggu animasi pickaxe hit dulu)

        // TODO: play sfx mining

        Vector3 startPos = transform.localPosition;

        float duration = 0.12f;
        float strength = 0.06f;
        float t = 0f;

        while (t < duration)
        {
            t += Time.deltaTime;

            float fade = 1f - (t / duration);

            Vector3 offset = new Vector3(
                Random.Range(-strength, strength) * fade,
                0f,
                Random.Range(-strength, strength) * fade
            );

            transform.localPosition = startPos + offset;
            yield return null;
        }

        transform.localPosition = startPos;
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
            // spawn mining ore drop
            int itemId = Random.Range(0, itemsToPickup.Length);
            inventoryManager.AddItem(itemsToPickup[itemId]);
        }
    }
}
