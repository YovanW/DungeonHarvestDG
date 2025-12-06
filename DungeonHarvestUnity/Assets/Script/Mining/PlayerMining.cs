using System.Linq;
using UnityEngine;

public class PlayerMining : MonoBehaviour
{
    public InventoryManager inventory = null;
    private string[] oreType = { "Stone Ore", "Coal Ore", "Copper Ore", "Iron Ore", "Mana Crystal", "Mythril Ore" };
    public ObjectDetector ray;
    public ItemInHand itemHand;   // drag the ItemInHand object here in Inspector

    void Update()
    {
        var selectedSO = itemHand.getSelectedSO();

        bool canMine = selectedSO != null && selectedSO.actionType == ItemSO.ActionType.Mine;

        if (Input.GetKeyDown(KeyCode.Mouse0) && ray.lookingAt != null && canMine)
        {
            Ore ore = ray.lookingAt.GetComponent<Ore>();
            if (ore != null && oreType.Contains(ore.oreData.oreName))
            {
                // Debug.Log("Mining: " + ore.oreData.oreName);
                TryMine();
            }
        }
    }

    void TryMine()
    {
        Ore ore = ray.hitInfoPublic.transform.GetComponent<Ore>();
        if (ore == null) return;

        int miningPower = itemHand.getSelectedSO().miningPower;
        ore.Mine(miningPower);
    }
}
