using System.Collections;
using UnityEngine;

public class PlayerCutting : MonoBehaviour
{
    private InventoryManager inventoryManager;
    public ObjectDetector ray;
    public ItemInHand itemHand;
    bool isSwinging = false;
    private float maxAxeDistance = 1.7f;

    void Start()
    {
        if (inventoryManager == null)
        {
            inventoryManager = GameObject.FindGameObjectWithTag("InventoryManager").GetComponent<InventoryManager>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        var selectedSO = itemHand.getSelectedSO();      // get the item in hand
        if (ray.lookingAt == null || Time.deltaTime == 0) return;

        // Chop tree
        if (selectedSO != null && selectedSO.actionType == ItemSO.ActionType.Chop)
        {
            TreeController tree = ray.lookingAt.GetComponent<TreeController>();
            if (tree == null) return;

            if (Input.GetKey(KeyCode.Mouse0))
            {
                if (isSwinging) return;

                if (!tree.isChopped)
                {
                    TryChop(tree);
                }
            }
        }
    }

    void TryChop(TreeController tree)
    {
        if (tree == null) return;

        float dist = Vector3.Distance(transform.position, ray.hitInfoPublic.transform.position);
        if (dist > maxAxeDistance) return;

        // Debug.Log("Chopping tree");

        StartCoroutine(SwingAnimation(ray.hitInfoPublic.transform));
        int chopPower = itemHand.getSelectedSO().miningPower;
        tree.Chop(chopPower);
    }

    IEnumerator SwingAnimation(Transform hitpos)
    {
        if (isSwinging) yield break;
        isSwinging = true;

        Transform axe = itemHand.transform;

        Vector3 startPos = axe.localPosition;
        Quaternion startRot = axe.localRotation;

        // 1) tarik ke samping dan ke atas
        Vector3 backPos = startPos + new Vector3(0.18f, 0.12f, -0.08f);
        Quaternion backRot = Quaternion.Euler(startRot.eulerAngles + new Vector3(-15f, 40f, -10f));

        // 2) posisi tebasan
        Vector3 hitPos = startPos + new Vector3(-0.22f, 0.05f, 0.25f);

        Vector3 localHitDir = (hitPos - startPos).normalized;
        Quaternion aimRot = Quaternion.LookRotation(localHitDir);
        Quaternion hitRot = Quaternion.Slerp(startRot, aimRot * Quaternion.Euler(90f, 0f, 0f), 0.5f);

        // pull back
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * 7f;
            float ease = 1f - Mathf.Pow(1f - t, 2f);

            axe.localPosition = Vector3.Lerp(startPos, backPos, ease);
            axe.localRotation = Quaternion.Lerp(startRot, backRot, ease);
            yield return null;
        }

        // swing / tebas
        t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * 14f;
            float ease = t * t * t;

            axe.localPosition = Vector3.Lerp(backPos, hitPos, ease);
            axe.localRotation = Quaternion.Lerp(backRot, hitRot, ease);
            yield return null;
        }

        // recover
        t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * 8f;
            float ease = 1f - Mathf.Pow(1f - t, 2f);

            axe.localPosition = Vector3.Lerp(hitPos, startPos, ease);
            axe.localRotation = Quaternion.Lerp(hitRot, startRot, ease);
            yield return null;
        }

        axe.localPosition = startPos;
        axe.localRotation = startRot;
        isSwinging = false;
    }
}
