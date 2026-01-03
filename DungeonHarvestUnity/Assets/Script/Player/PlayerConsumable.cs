using System.Collections;
using System.Threading;
using UnityEngine;

public class PlayerConsumable : MonoBehaviour
{
    private InventoryManager inventoryManager;
    public ItemInHand itemInHand;
    public statController statController;
    bool isEating = false;
    enum BuffType { Damage, Defense, Health }



    void Start()
    {
        if (statController == null) { Debug.LogWarning("statController reference is missing in PlayerConsumable"); }
        if (itemInHand == null) { Debug.LogWarning("itemInHand reference is missing in PlayerConsumable"); }
        if (inventoryManager == null) { inventoryManager = GameObject.FindGameObjectWithTag("InventoryManager").GetComponent<InventoryManager>(); }
    }

    void Update()
    {
        if (itemInHand.getSelectedSO() == null) return;

        if (Input.GetKeyDown(KeyCode.Mouse1) && itemInHand.getSelectedSO().type == ItemSO.ItemType.Consumable && Time.timeScale != 0 && !isEating)
        {
            ItemSO item = itemInHand.getSelectedSO();

            if (item.name == "Attack Mashroom" && statController.maxDamageBuff > 0)
            {
                statController.maxDamageBuff--;
                StartCoroutine(EatAndBuff(item, BuffType.Damage));
            }
            else if (item.name == "Def Mashroom" && statController.maxDefenseBuff > 0)
            {
                statController.maxDefenseBuff--;
                StartCoroutine(EatAndBuff(item, BuffType.Defense));
            }
            else if (item.name == "Health Mashroom" && statController.maxHealthBuff > 0)
            {
                statController.maxHealthBuff--;
                StartCoroutine(EatAndBuff(item, BuffType.Health));
            }
        }
    }
    IEnumerator EatAndBuff(ItemSO item, BuffType type)
    {
        yield return StartCoroutine(EatAnimation());

        // remove item AFTER eating animation
        int slotIndex = inventoryManager.GetComponent<HotbarManager>().selectedIndex;
        inventoryManager.RemoveItem(item, slotIndex);

        // apply buff
        if (type == BuffType.Damage)
            StartCoroutine(BuffDamage(item.extraInfo, 10f));
        else if (type == BuffType.Defense)
            StartCoroutine(BuffDefense(item.extraInfo, 10f));
        else if (type == BuffType.Health)
            StartCoroutine(BuffHealth(item.extraInfo, 10f));
    }


    IEnumerator BuffDamage(int amount, float duration)
    {
        statController.setDamage(statController.getDamage() + amount);
        yield return new WaitForSeconds(duration);
        statController.setDamage(statController.getDamage() - amount);
        statController.maxDamageBuff++;
    }

    IEnumerator BuffDefense(int amount, float duration)
    {
        statController.setDefense(statController.getDefense() + amount);
        yield return new WaitForSeconds(duration);
        statController.setDefense(statController.getDefense() - amount);
        statController.maxDefenseBuff++;
    }

    IEnumerator BuffHealth(int amount, float duration)
    {
        statController.setHealth(statController.getHealth() + amount);
        yield return new WaitForSeconds(duration);
        statController.setHealth(statController.getHealth() - amount);
        statController.maxHealthBuff++;
    }

    IEnumerator EatAnimation()
    {
        if (isEating) yield break;
        isEating = true;

        Transform hand = itemInHand.transform;

        Vector3 startPos = hand.localPosition;
        Quaternion startRot = hand.localRotation;

        // posisi tangan saat makan
        Vector3 eatPos = startPos + new Vector3(-0.32f, 0.10f, 0f);
        Quaternion eatRot = startRot * Quaternion.Euler(-12f, 18f, -4f);
        float t = 0f;

        // move to mouth
        while (t < 1f)
        {
            t += Time.deltaTime * 8f;
            hand.localPosition = Vector3.Lerp(startPos, eatPos, t);
            hand.localRotation = Quaternion.Slerp(startRot, eatRot, t);
            yield return null;
        }

        // makan (chewing)
        float chewTime = 1f;
        float chewTimer = 0f;

        Vector3 chewUp = eatPos + new Vector3(0f, 0.01f, 0f);
        Quaternion chewRot = eatRot * Quaternion.Euler(4f, 0f, 0f);

        while (chewTimer < chewTime)
        {
            chewTimer += Time.deltaTime;

            float chew = Mathf.PingPong(chewTimer * 4.5f, 1f);

            hand.localPosition = Vector3.Lerp(eatPos, chewUp, chew);
            hand.localRotation = Quaternion.Slerp(eatRot, chewRot, chew);

            yield return null;
        }

        // balik ke idle
        t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * 6f;
            hand.localPosition = Vector3.Lerp(eatPos, startPos, t);
            hand.localRotation = Quaternion.Slerp(eatRot, startRot, t);
            yield return null;
        }

        hand.localPosition = startPos;
        hand.localRotation = startRot;

        isEating = false;
    }

}