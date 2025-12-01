using System.Linq;
using UnityEngine;

public class PlayerMining : MonoBehaviour
{
    public InventoryManager inventory = null;
    private string[] oreType = { "Stone Ore", "Coal Ore", "Bronze Ore", "Iron Ore", "Mana Crystal", "Mythril Ore" };
    public ObjectDetector ray;  // drag reference di Inspector

    void Update()
    {
        // TODO: add pickaxe is selected in toolbar checker
        if (Input.GetKeyDown(KeyCode.Mouse0) && ray.lookingAt != null && oreType.Contains(ray.lookingAt.name)) // click kiri
        {
            Debug.Log("Mining : " + ray.lookingAt.name);
            TryMine();
        }
    }

    void TryMine()
    {
        Ore ore = ray.hitInfoPublic.transform.GetComponent<Ore>();
        if (ore == null) return;

        var miningPower = 10;

        // // TODO: mining logic
        ore.Mine(miningPower);
    }


}
