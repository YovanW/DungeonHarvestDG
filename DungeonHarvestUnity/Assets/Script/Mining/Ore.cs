using UnityEngine;

public class Ore : MonoBehaviour
{
    public oreData oreData;
    public InventoryManager inventoryManager = null;
    public ItemSO[] itemsToPickup;

    private int health;

    void Start()
    {
        health = oreData.hardness * 3;

        if (inventoryManager == null)
        {
            Debug.LogWarning("InventoryManager not assigned in Ore script.");
        }
    }

    public void Mine(int power)
    {
        Debug.Log("Power : " + power);


        // cek apakah pickaxe sesuai
        if (power < oreData.hardness)
        {
            Debug.Log("Your pickaxe is too weak");
        }
        else
        {
            health -= power;
        }

        // TODO: play sfx mining


        if (health <= 0)
        {
            if (inventoryManager != null)
                SpawnDrops();
            Destroy(gameObject);
        }
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
