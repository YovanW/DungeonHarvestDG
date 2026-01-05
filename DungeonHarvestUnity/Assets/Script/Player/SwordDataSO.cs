using UnityEngine;

[CreateAssetMenu(fileName = "NewSword", menuName = "Weapons/Sword")]
public class SwordData : ScriptableObject
{
    public string swordName;
    public GameObject prefab;
    public Sprite icon;
    [Header("Combat Stats")]
    public float damage = 25f;
    public float staminaCost = 20f;
    public float attackRange = 2.5f;
    public float attackCooldown = 0.6f;
}
