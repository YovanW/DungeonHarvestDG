using System.Linq;
using UnityEngine;

public class PlayerMining : MonoBehaviour
{
    public InventoryManager inventory = null;
    private string[] oreType = { "Stone Ore", "Coal Ore", "Bronze Ore", "Iron Ore", "Mana Crystal", "Mythril Ore" };
    public ObjectDetector ray;  // drag reference di Inspector

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && ray.lookingAt != null)
        {
            Ore ore = ray.lookingAt.GetComponent<Ore>();
            if (ore != null && oreType.Contains(ore.oreData.oreName))
            {
                Debug.Log("Mining: " + ore.oreData.oreName);
                TryMine();
            }
        }

    }

    void TryMine()
    {
        Ore ore = ray.hitInfoPublic.transform.GetComponent<Ore>();
        if (ore == null) return;

        var miningPower = 5;

        // TODO: mining logic
        ore.Mine(miningPower);
    }


}
