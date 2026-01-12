using UnityEngine;
using UnityEngine.AI;

public class EnemyPatrol : MonoBehaviour, ILoopResettable
{
    public Transform waypointParent;
    public float patrolSpeed = 2f;
    public float waitTimeAtPoint = 1f;

    public int GetCurrentWaypointIndex()
    {
        return currentIndex;
    }

    [Range(0, 55)]
    public int startingWaypointIndex = 0;


    private NavMeshAgent agent;
    private Transform[] patrolPoints;
    private int currentIndex;
    private float waitTimer;
    private bool isPatrolling = true;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = patrolSpeed;

        patrolPoints = new Transform[waypointParent.childCount];
        for (int i = 0; i < waypointParent.childCount; i++)
        {
            patrolPoints[i] = waypointParent.GetChild(i);
        }

        currentIndex = startingWaypointIndex % patrolPoints.Length;
        GoToNextPoint();
    }

    void Update()
    {
        if (!isPatrolling) return;
        if (agent.pathPending) return;

        if (!agent.pathPending && agent.remainingDistance < 0.3f)
        {
            waitTimer += Time.deltaTime;

            if (waitTimer >= waitTimeAtPoint)
            {
                GoToNextPoint();
                waitTimer = 0f;
            }
        }
    }

    void GoToNextPoint()
    {
        if (patrolPoints.Length == 0) return;

        agent.speed = patrolSpeed;
        agent.SetDestination(patrolPoints[currentIndex].position);
        currentIndex = (currentIndex + 1) % patrolPoints.Length;
    }

    // 🔹 CALLED BY EnemyAI
    public void StopPatrol()
    {
        isPatrolling = false;
    }

    // 🔹 CALLED BY EnemyAI
    public void ResumePatrol()
    {
        if (isPatrolling) return;

        isPatrolling = true;
        GoToNextPoint();
    }

    public void ResetState()
    {
        isPatrolling = true;
        waitTimer = 0f;
        currentIndex = startingWaypointIndex;

        if (agent != null && agent.enabled)
        {
            agent.ResetPath();
            agent.SetDestination(patrolPoints[currentIndex].position);
        }
    }

}
