using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public Transform player;
    public Transform[] waypoints;

    public float chaseRange = 8f;
    public float attackRange = 1.8f;
    public float attackCooldown = 5f;

    private NavMeshAgent agent;
    private Animator anim;
    private int currentWP = 0;
    private float nextAttackTime = 0f;

    public GameObject attackHitbox;

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

        agent.SetDestination(waypoints[currentWP].position);
    }

    void Update()
    {
        float dist = Vector3.Distance(transform.position, player.position);

        // Always update Distance parameter
        anim.SetFloat("Distance", dist);

        // Already dead
        if (anim.GetInteger("Health") <= 0)
        {
            agent.ResetPath();
            return;
        }

        // Attack
        if (dist <= attackRange)
        {
            agent.ResetPath();
            anim.SetBool("Walk", false);
            anim.SetBool("InRange", true);

            if (Time.time >= nextAttackTime)
            {
                nextAttackTime = Time.time + attackCooldown;
            }

            transform.LookAt(player);
            return;
        }

        // Chase
        if (dist <= chaseRange)
        {
            anim.SetBool("Walk", true);
            anim.SetBool("InRange", false);
            agent.SetDestination(player.position);
            return;
        }

        // Patrol
        Patrol();
    }

    void Patrol()
    {
        anim.SetBool("Walk", true);
        anim.SetBool("InRange", false);

        if (agent.remainingDistance < 0.3f)
        {
            currentWP = (currentWP + 1) % waypoints.Length;
            agent.SetDestination(waypoints[currentWP].position);
        }
    }

    public void TakeDamage(int dmg)
    {
        int h = anim.GetInteger("Health");
        anim.SetInteger("Health", h - dmg);

        if (anim.GetInteger("Health") <= 0)
        {
            agent.ResetPath();
            anim.SetBool("Walk", false);
            anim.SetBool("InRange", false);
        }
    }
}
