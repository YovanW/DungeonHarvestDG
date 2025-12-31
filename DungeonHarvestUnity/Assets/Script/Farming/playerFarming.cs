using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class playerFarming : MonoBehaviour
{
    public ObjectDetector ray;
    public ItemInHand itemHand;
    bool isSwinging = false;
    private float maxRakeDistance = 2.5f;


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
                    TryPlant(soil, seed);
                }
            }


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
