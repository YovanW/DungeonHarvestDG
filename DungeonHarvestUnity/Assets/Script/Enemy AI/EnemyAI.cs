using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public Transform player;

    public float chaseRange = 8f;
    public float attackRange = 1.8f;
    public float attackCooldown = 2f;

    private NavMeshAgent agent;
    private Animator anim;
    private float nextAttackTime = 0f;
    private bool isAttacking = false;

    public GameObject attackHitbox;
    private bool isDead = false;


    public void EnableHitbox()
    {
        attackHitbox.SetActive(true);
    }

    public void DisableHitbox()
    {
        attackHitbox.SetActive(false);
    }

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();

        // Assign player automatically
        GameObject p = GameObject.FindWithTag("Player");
        if (p != null)
        {
            player = p.transform;
        }
        else
        {
            Debug.LogError("Player with tag 'Player' not found!");
        }
    }

    public void Die()
    {
        isDead = true;

        if (agent != null && agent.isOnNavMesh)
            agent.ResetPath();

        if (agent != null)
            agent.enabled = false;

        anim.SetBool("Walk", false);
        anim.SetBool("InRange", false);
    }


    void Update()
    {
        float dist = Vector3.Distance(transform.position, player.position);

        // Attack range
        if (dist <= attackRange)
        {
            agent.ResetPath();
            transform.LookAt(player);
            anim.SetBool("Walk", false);

            // Check if cooldown is active
            if (Time.time >= nextAttackTime && !isAttacking)
            {
                // Start attack
                anim.SetBool("InRange", true);
                isAttacking = true;
            }
            else if (isAttacking)
            {
                // Keep InRange true during attack animation
                anim.SetBool("InRange", true);
                
                // Check if attack animation is finishing (you can adjust this timing)
                AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
                if (stateInfo.IsName("Attack02") && stateInfo.normalizedTime >= 0.9f)
                {
                    // Attack animation nearly finished, start cooldown
                    nextAttackTime = Time.time + attackCooldown;
                    isAttacking = false;
                    anim.SetBool("InRange", false);
                }
            }
            else
            {
                // In cooldown - stay in idle
                anim.SetBool("InRange", false);
            }
            return;
        }
        else
        {
            // Reset attack state if player moves out of range
            isAttacking = false;
        }

        // Chase
        if (dist <= chaseRange)
        {
            anim.SetBool("Walk", true);
            anim.SetBool("InRange", false);
            agent.SetDestination(player.position);
            return;
        }

        // Idle when out of chase range
        agent.ResetPath();
        anim.SetBool("Walk", false);
        anim.SetBool("InRange", false);
    }

}