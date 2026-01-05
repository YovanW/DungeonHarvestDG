using UnityEngine;

public class SwordHitbox : MonoBehaviour
{
    public float damage;
    private bool canDamage;
    private Collider hitbox;

    void Awake()
    {
        hitbox = GetComponent<Collider>();
        hitbox.enabled = false; // disabled by default
    }

    public void EnableHitbox(float dmg)
    {
        damage = dmg;
        canDamage = true;
        hitbox.enabled = true;
        
    }

    public void DisableHitbox()
    {
        canDamage = false;
        hitbox.enabled = false;
    }
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("here");
        if (!canDamage) return;
        EnemyHealth enemy = other.GetComponent<EnemyHealth>();
        Debug.Log(
            $"HIT TRIGGERED BY: {other.gameObject.name} | " +
            $"Tag: {other.gameObject.tag} | " +
            $"Layer: {LayerMask.LayerToName(other.gameObject.layer)} | " +
            $"Has EnemyHealth: {other.GetComponentInParent<EnemyHealth>() != null}"
        );

        if (enemy != null)
        {
            Debug.Log("enemy is hit");
            enemy.TakeDamage(damage);
            canDamage = false; // one hit per swing
        }
    }
}
