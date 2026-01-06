using UnityEngine;
using System.Collections;

public class SwordAttack : MonoBehaviour
{
    public ItemInHand itemHand;
    public SwordHitbox swordHitbox;

    private bool isSwinging;
    private ItemSO currentItem;

    void Update()
    {
        currentItem = itemHand.getSelectedSO();

        if (Input.GetMouseButtonDown(0))
        {
            TryAttack();
        }
    }

    void TryAttack()
    {
        // Debug.Log("TryAttack");
        if (isSwinging) return;
        if (currentItem == null) return;
        if (currentItem.actionType != ItemSO.ActionType.Attack) return;
        StartCoroutine(SwingAnimation());
    }

    IEnumerator SwingAnimation()
    {
        if (isSwinging) yield break;
        isSwinging = true;

        if (GameAudio.Instance != null)
            GameAudio.Instance.PlaySFX(GameAudio.Instance.swing);


        Transform sword = itemHand.transform;

        Vector3 startPos = sword.localPosition;
        Quaternion startRot = sword.localRotation;

        Vector3 backPos = startPos + new Vector3(-0.06f, 0.16f, -0.10f);
        Quaternion backRot = Quaternion.Euler(startRot.eulerAngles + new Vector3(-22f, 0f, 8f));

        Vector3 hitPos = startPos + new Vector3(-0.25f, 0.177f, 0.298f);

        Vector3 localHitDir = (hitPos - startPos).normalized;
        Quaternion aimRot = Quaternion.LookRotation(localHitDir);
        Quaternion hitRot = Quaternion.Slerp(
            startRot,
            aimRot * Quaternion.Euler(90f, 0f, 0f),
            0.45f
        );

        // pull back
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * 8f;
            float ease = 1f - Mathf.Pow(1f - t, 2f);

            sword.localPosition = Vector3.Lerp(startPos, backPos, ease);
            sword.localRotation = Quaternion.Lerp(startRot, backRot, ease);
            yield return null;
        }

        // strike — ENABLE HITBOX
        swordHitbox.EnableHitbox(currentItem.miningPower);

        t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * 13f;
            float ease = t * t * t;

            sword.localPosition = Vector3.Lerp(backPos, hitPos, ease);
            sword.localRotation = Quaternion.Lerp(backRot, hitRot, ease);
            yield return null;
        }

        // VERY IMPORTANT: wait one frame so physics can detect it
        yield return new WaitForFixedUpdate();

        // keep hitbox active briefly
        yield return new WaitForSeconds(0.05f);

        // DISABLE HITBOX
        swordHitbox.DisableHitbox();

        // recover
        t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * 9f;
            float ease = 1f - Mathf.Pow(1f - t, 2f);

            sword.localPosition = Vector3.Lerp(hitPos, startPos, ease);
            sword.localRotation = Quaternion.Lerp(hitRot, startRot, ease);
            yield return null;
        }

        sword.localPosition = startPos;
        sword.localRotation = startRot;

        isSwinging = false;
    }
}
