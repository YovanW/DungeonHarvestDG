using System.Collections;
using System.Linq;
using UnityEngine;

public class PlayerMining : MonoBehaviour
{
    public InventoryManager inventory = null;
    private string[] oreType = { "Stone Ore", "Coal Ore", "Copper Ore", "Iron Ore", "Mana Crystal", "Mythril Ore" };
    public ObjectDetector ray;
    public ItemInHand itemHand;

    bool isSwinging = false;
    private float maxMineDistance = 2f;

    void Update()
    {
        var selectedSO = itemHand.getSelectedSO();

        bool canMine = selectedSO != null && selectedSO.actionType == ItemSO.ActionType.Mine;

        if (Input.GetKey(KeyCode.Mouse0) && ray.lookingAt != null && canMine)
        {
            Ore ore = ray.lookingAt.GetComponent<Ore>();
            if (ore != null && oreType.Contains(ore.oreData.oreName))
            {
                // Debug.Log("Mining: " + ore.oreData.oreName);

                if (!isSwinging)
                {
                    TryMine();
                }

            }
        }
    }

    void TryMine()
    {
        Ore ore = ray.hitInfoPublic.transform.GetComponent<Ore>();
        if (ore == null) return;

        float dist = Vector3.Distance(transform.position, ray.hitInfoPublic.transform.position);
        // Debug.Log(dist);
        // Debug.Log(maxMineDistance);

        if (dist > maxMineDistance) return;

        StartCoroutine(SwingAnimation(ray.hitInfoPublic.transform));
        int miningPower = itemHand.getSelectedSO().miningPower;
        ore.Mine(miningPower);
    }

    IEnumerator SwingAnimation(Transform hitpos)
    {
        if (isSwinging) yield break;
        isSwinging = true;

        Transform pickaxe = itemHand.transform;

        Vector3 startPos = pickaxe.localPosition;
        Quaternion startRot = pickaxe.localRotation;

        // 1) pull back and up
        Vector3 backPos = startPos + new Vector3(-0.06f, 0.16f, -0.10f);
        Quaternion backRot = Quaternion.Euler(startRot.eulerAngles + new Vector3(-22f, 0f, 8f));

        // 2) swing down to hit
        Vector3 hitPos = startPos + new Vector3(-0.25f, 0.177f, 0.298f);

        Vector3 localHitDir = (hitPos - startPos).normalized;
        Quaternion aimRot = Quaternion.LookRotation(localHitDir);
        Quaternion hitRot = Quaternion.Slerp(startRot, aimRot * Quaternion.Euler(90f, 0f, 0f), 0.45f);

        // pull back
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * 8f;
            float ease = 1f - Mathf.Pow(1f - t, 2f);

            pickaxe.localPosition = Vector3.Lerp(startPos, backPos, ease);
            pickaxe.localRotation = Quaternion.Lerp(startRot, backRot, ease);
            yield return null;
        }

        // strike
        t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * 13f;
            float ease = t * t * t;

            pickaxe.localPosition = Vector3.Lerp(backPos, hitPos, ease);
            pickaxe.localRotation = Quaternion.Lerp(backRot, hitRot, ease);
            yield return null;
        }

        // recover
        t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * 9f;
            float ease = 1f - Mathf.Pow(1f - t, 2f);

            pickaxe.localPosition = Vector3.Lerp(hitPos, startPos, ease);
            pickaxe.localRotation = Quaternion.Lerp(hitRot, startRot, ease);
            yield return null;
        }

        pickaxe.localPosition = startPos;
        pickaxe.localRotation = startRot;
        isSwinging = false;
    }

}
