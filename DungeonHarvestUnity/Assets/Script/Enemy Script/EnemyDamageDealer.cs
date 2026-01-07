using UnityEngine;

public class EnemyDamageDealer : MonoBehaviour
{
    public int damage = 10;

    private void OnTriggerEnter(Collider other)
    {
        // Debug.Log("Hitbox triggered with: " + other.name);

        if (other.CompareTag("Player"))
        {
            // Debug.Log("Hitbox confirmed PLAYER hit.");

            HealthStaminaManager playerHealth = other.GetComponentInParent<HealthStaminaManager>();

            if (playerHealth != null)
            {
                // Debug.Log("Dealing damage: " + damage);
                playerHealth.TakeDamage(damage);
            }
            else
            {
                // Debug.Log("Player has NO HealthStaminaManager found on parent.");
            }
        }
    }
}
