using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using System.Collections;


public class EnemyHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    private float currentHealth;

    private Animator anim;
    private NavMeshAgent agent;
    private Collider[] colliders;
    public float MaxHealth => maxHealth;
    public float CurrentHealth => currentHealth;

    public Image fillImage;
    private bool isDead = false;

    void Awake()
    {
        anim = GetComponentInParent<Animator>();
        agent = GetComponentInParent<NavMeshAgent>();
        colliders = GetComponentsInChildren<Collider>();
    }


    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        if (isDead) return;

        currentHealth -= damage;

        fillImage.fillAmount =
            currentHealth / maxHealth;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;

        // Stop movement
        if (agent != null)
        {
            if (agent.isOnNavMesh)
                agent.ResetPath();

            agent.enabled = false;
        }

        // Disable all colliders
        foreach (Collider c in colliders)
        {
            c.enabled = false;
        }

        // Play death animation
        anim.SetTrigger("Die");

        // Destroy after delay
        StartCoroutine(DestroyAfterDelay());
    }


    IEnumerator DestroyAfterDelay()
    {
        yield return new WaitForSeconds(5f);
        Destroy(transform.root.gameObject);

    }
}
