using System.Collections;
using UnityEngine;

public class PlayerConsumable : MonoBehaviour
{
    private InventoryManager inventoryManager;
    public ItemInHand itemInHand;
    public statController statController;

    bool isEating = false;
    private ItemSO eatingItem = null;
    private Coroutine eatingCoroutine;

    enum BuffType { Damage, Defense, Health }
    private Vector3 defaultLocalPos;
    private Quaternion defaultLocalRot;

    void Start()
    {
        defaultLocalPos = itemInHand.transform.localPosition;
        defaultLocalRot = itemInHand.transform.localRotation;   

        if (statController == null)
            Debug.LogWarning("statController reference is missing");

        if (itemInHand == null)
            Debug.LogWarning("itemInHand reference is missing");

        if (inventoryManager == null)
            inventoryManager = GameObject.FindGameObjectWithTag("InventoryManager")
                .GetComponent<InventoryManager>();
    }

    void Update()
    {
        if (Time.timeScale == 0) return;

        ItemSO selectedItem = itemInHand.getSelectedSO();

        // STOP dulu animasi kalau item berubah / hilang tengah makan
        if (isEating && selectedItem != eatingItem)
        {
            if (eatingCoroutine != null)
                StopCoroutine(eatingCoroutine);

            itemInHand.transform.localPosition = defaultLocalPos;
            itemInHand.transform.localRotation = defaultLocalRot;

            isEating = false;
        }

        if (selectedItem == null) return;
        if (selectedItem.type != ItemSO.ItemType.Consumable) return;

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            // Debug.Log("Click");
            eatingItem = selectedItem;

            if (selectedItem.name == "Attack Mashroom" && statController.maxDamageBuff > 0)
            {
                StartCoroutine(EatAndBuff(selectedItem, BuffType.Damage));
            }
            else if (selectedItem.name == "Def Mashroom" && statController.maxDefenseBuff > 0)
            {
                StartCoroutine(EatAndBuff(selectedItem, BuffType.Defense));
            }
            else if (selectedItem.name == "Health Mashroom" && statController.maxHealthBuff > 0)
            {
                StartCoroutine(EatAndBuff(selectedItem, BuffType.Health));
            }
        }
    }

    IEnumerator EatAndBuff(ItemSO item, BuffType type)
    {
        // ===== ANIMASI MAKAN =====
        eatingCoroutine = StartCoroutine(EatAnimation(item));
        yield return eatingCoroutine;
        eatingCoroutine = null;

        // ===== HAPUS ITEM DARI INVENTORY =====
        int slotIndex = inventoryManager.GetComponent<HotbarManager>().selectedIndex;
        inventoryManager.RemoveItem(item, slotIndex);

        // ===== APPLY BUFF =====
        if (type == BuffType.Damage)
            StartCoroutine(BuffDamage(item.extraInfo, 10f));
        else if (type == BuffType.Defense)
            StartCoroutine(BuffDefense(item.extraInfo, 10f));
        else
            StartCoroutine(BuffHealth(item.extraInfo, 10f));
    }

    // ================= BUFFS =================
    IEnumerator BuffDamage(int amount, float duration)
    {
        statController.maxDamageBuff--;
        statController.setDamage(statController.getDamage() + amount);
        yield return new WaitForSeconds(duration);
        statController.setDamage(statController.getDamage() - amount);
        statController.maxDamageBuff++;
    }

    IEnumerator BuffDefense(int amount, float duration)
    {
        statController.maxDefenseBuff--;
        statController.setDefense(statController.getDefense() + amount);
        yield return new WaitForSeconds(duration);
        statController.setDefense(statController.getDefense() - amount);
        statController.maxDefenseBuff++;
    }

    IEnumerator BuffHealth(int amount, float duration)
    {
        statController.maxHealthBuff--;
        statController.setHealth(statController.getHealth() + amount);
        yield return new WaitForSeconds(duration);
        statController.setHealth(statController.getHealth() - amount);
        statController.maxHealthBuff++;
    }

    // ================= EAT ANIMATION =================
    IEnumerator EatAnimation(ItemSO item)
    {
        if (isEating) yield break;

        isEating = true;

        // Use the current local position/rotation as start
        Vector3 startLocalPos = itemInHand.transform.localPosition;
        Quaternion startLocalRot = itemInHand.transform.localRotation;

        Vector3 eatLocalPos = startLocalPos + new Vector3(-0.32f, 0.10f, 0f);
        Quaternion eatLocalRot = startLocalRot * Quaternion.Euler(-12f, 18f, -4f);

        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime * 8f;
            itemInHand.transform.localPosition = Vector3.Lerp(startLocalPos, eatLocalPos, t);
            itemInHand.transform.localRotation = Quaternion.Slerp(startLocalRot, eatLocalRot, t);
            yield return null;
        }

        float chewTimer = 0f;
        while (chewTimer < 1f)
        {
            chewTimer += Time.deltaTime;
            float chew = Mathf.PingPong(chewTimer * 4.5f, 1f);
            itemInHand.transform.localPosition = eatLocalPos + Vector3.up * chew * 0.01f;
            yield return null;
        }

        // return to start pos
        t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * 6f;
            itemInHand.transform.localPosition = Vector3.Lerp(eatLocalPos, startLocalPos, t);
            itemInHand.transform.localRotation = Quaternion.Slerp(eatLocalRot, startLocalRot, t);
            yield return null;
        }

        itemInHand.transform.localPosition = startLocalPos;
        itemInHand.transform.localRotation = startLocalRot;

        isEating = false;
    }

}
