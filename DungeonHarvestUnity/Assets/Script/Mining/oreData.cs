using UnityEngine;

[CreateAssetMenu(fileName = "New Ore Data", menuName = "Mining/Ore Data")]
public class oreData : ScriptableObject
{
    public string oreName;
    public int hardness;
    public int dropAmount;
    public GameObject dropPrefab;
    public int respawnTime;
}
