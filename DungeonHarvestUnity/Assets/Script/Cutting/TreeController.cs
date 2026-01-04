using System.Collections;
using UnityEngine;

public class TreeController : MonoBehaviour
{
    public InventoryManager inventoryManager = null;
    public GameObject treeModel;
    public GameObject choppedTreeModel;
    public ItemSO drop;
    public int dropAmount = 5;

    private float health = 100f;
    public float respawnTime = 30f;
    private float spawnTimer = 0f;

    public bool isChopped = false;

    void Start()
    {
        if (inventoryManager == null) { inventoryManager = GameObject.FindGameObjectWithTag("InventoryManager").GetComponent<InventoryManager>(); }
        if (choppedTreeModel == null) { Debug.LogWarning("choppedTreeModel reference is missing in TreeController"); }
        if (treeModel == null) { Debug.LogWarning("treeModel reference is missing in TreeController"); }
        if (drop == null) { Debug.LogWarning("drop ItemSO reference is missing in TreeController"); }

        UpdateModel();
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
                // respawn tree
                isChopped = false;
                health = 100f; // reset health

                UpdateModel();
            }
        }

    }

    IEnumerator HitAnimation()
    {
        yield return new WaitForSeconds(0f); // start delay (tunggu animasi axe hit dulu)

        // TODO: play sfx chopping

        Vector3 startPos = treeModel.transform.localPosition;

        float duration = 0.12f;
        float strength = 0.06f;
        float t = 0f;

        while (t < duration)
        {
            t += Time.deltaTime;

            float fade = 1f - (t / duration);

            Vector3 offset = new Vector3(
                Random.Range(-strength, strength) * fade, 0f,
                Random.Range(-strength, strength) * fade
);

            treeModel.transform.localPosition = startPos + offset;
            yield return null;
        }

        treeModel.transform.localPosition = startPos;;
    }


    void UpdateModel()
    {
        if (isChopped)
        {
            treeModel.SetActive(false);
            choppedTreeModel.SetActive(true);
        }
        else
        {
            treeModel.SetActive(true);
            choppedTreeModel.SetActive(false);
        }
    }

    public void Chop(int chopPower)
    {
        if (isChopped) return;

        health -= chopPower;
        StartCoroutine(HitAnimation());

        if (health <= 0)
        {
            isChopped = true;

            // Drop item
            int dropCount = Random.Range(3, dropAmount);
            for (int i = 0; i < dropCount; i++)
            {
                inventoryManager.AddItem(drop);
            }

            // Start respawn timer
            UpdateModel();
            spawnTimer = respawnTime;
        }
    }


}
