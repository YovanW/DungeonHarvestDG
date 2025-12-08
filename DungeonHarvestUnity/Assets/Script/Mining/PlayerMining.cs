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
                    StartCoroutine(SwingAnimation());
                }

            }
        }

        // hold left click
        
    }

    void TryMine()
    {
        Ore ore = ray.hitInfoPublic.transform.GetComponent<Ore>();
        if (ore == null) return;

        int miningPower = itemHand.getSelectedSO().miningPower;
        ore.Mine(miningPower);
    }

    IEnumerator SwingAnimation()
    {
        if (isSwinging) yield break;
        isSwinging = true;

        Transform pickaxe = itemHand.transform;

        Vector3 startPos = pickaxe.localPosition;
        Quaternion startRot = pickaxe.localRotation;

        // 1) pull back and up
        Vector3 backPos = startPos + new Vector3(0f, 0.18f, -0.12f);
        Quaternion backRot = Quaternion.Euler(startRot.eulerAngles + new Vector3(-20f, 0f, 10f));

        // 2) swing down to hit
        Vector3 hitPos = startPos + new Vector3(0f, -0.32f, 1f);
        Quaternion hitRot = Quaternion.Euler(startRot.eulerAngles + new Vector3(55f, 0f, -22f));

        float t;

        // back + up
        t = 0;
        while (t < 1f)
        {
            t += Time.deltaTime * 8f;
            float ease = 1 - Mathf.Pow(1 - t, 2);   // easing out
            pickaxe.localPosition = Vector3.Lerp(startPos, backPos, ease);
            pickaxe.localRotation = Quaternion.Lerp(startRot, backRot, ease);
            yield return null;
        }

        // swing down
        t = 0;
        while (t < 1f)
        {
            t += Time.deltaTime * 13f;
            float ease = Mathf.Pow(t, 3);   // accelerating into hit
            pickaxe.localPosition = Vector3.Lerp(backPos, hitPos, ease);
            pickaxe.localRotation = Quaternion.Lerp(backRot, hitRot, ease);
            yield return null;
        }

        // return to starting pose
        t = 0;
        while (t < 1f)
        {
            t += Time.deltaTime * 9f;
            float ease = 1 - Mathf.Pow(1 - t, 2);
            pickaxe.localPosition = Vector3.Lerp(hitPos, startPos, ease);
            pickaxe.localRotation = Quaternion.Lerp(hitRot, startRot, ease);
            yield return null;
        }

        pickaxe.localPosition = startPos;
        pickaxe.localRotation = startRot;
        isSwinging = false;
    }

}
