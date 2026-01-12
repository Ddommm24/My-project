using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class EnemyAttack : MonoBehaviour
{
    public float attackRange = 1.5f;
    public float attackCooldown = 1f;
    public float damage = 20f;

    private float lastAttackTime;
    private NavMeshAgent agent;
    private Transform player;
    private PlayerStats playerStats;
    private EnemyAI enemyAI;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.stoppingDistance = attackRange;
        enemyAI = GetComponent<EnemyAI>();

        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerStats = player.GetComponent<PlayerStats>();
    }

    void Update()
    {
        if (!enemyAI.IsChasing)
            return;

        if (Time.time < lastAttackTime + attackCooldown)
            return;

        float distance = Vector3.Distance(
            transform.position,
            player.position
        );


        if (!agent.pathPending && distance <= agent.stoppingDistance + attackRange)
        {
            Attack();
        }
    }


    void Attack()
    {
        Debug.Log("Enemy attacked");

        lastAttackTime = Time.time;

        playerStats.TakeDamage(damage);

        StartCoroutine(HitPause());
    }

    IEnumerator HitPause()
    {
        agent.isStopped = true;
        enemyAI.PauseChase(true);

        yield return new WaitForSeconds(1f);

        agent.isStopped = false;
        enemyAI.PauseChase(false);

        // FORCE RETURN TO CHASE
        agent.SetDestination(player.position);
    }

}
