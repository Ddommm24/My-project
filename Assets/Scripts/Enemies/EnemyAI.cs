using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour, ILoopResettable
{
    enum EnemyState { Patrol, Chase, MoveToLastSeen, Search }
    EnemyState currentState = EnemyState.Patrol;

    Vector3 startPosition;
    Quaternion startRotation;


    [Header("Speeds")]
    public float patrolSpeed = 2f;
    public float chaseSpeed = 4.5f;

    [Header("Search")]
    public float searchDuration = 5f;
    public float searchRadius = 3f;

    [Header("Visuals")]
    public Renderer eyeRenderer;
    public Color patrolColor = Color.blue;
    public Color chaseColor = Color.red;

    EnemyVision vision;
    EnemyPatrol patrol;
    NavMeshAgent agent;
    Transform player;

    Vector3 lastSeenPosition;
    float searchTimer;
    private bool isPaused;

    public bool IsChasing =>
        currentState == EnemyState.Chase ||
        currentState == EnemyState.MoveToLastSeen ||
        currentState == EnemyState.Search;

    private bool inCombat;


    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        vision = GetComponentInChildren<EnemyVision>();
        patrol = GetComponent<EnemyPatrol>();
        player = GameObject.FindGameObjectWithTag("Player").transform;

        startPosition = transform.position;
        startRotation = transform.rotation;
    }

    void Update()
    {
        if (isPaused)
        return;

        switch (currentState)
        {
            case EnemyState.Patrol:
                HandlePatrol();
                break;

            case EnemyState.Chase:
                HandleChase();
                break;

            case EnemyState.MoveToLastSeen:
                HandleMoveToLastSeen();
                break;

            case EnemyState.Search:
                HandleSearch();
                break;
        }
    }

    // ================= STATES =================

    void HandlePatrol()
    {
        SetEyeColor(patrolColor);

        if (vision.CanSeePlayer)
            EnterChase();
    }

    void HandleChase()
    {
        SetEyeColor(chaseColor);

        if (vision.CanSeePlayer)
        {
            lastSeenPosition = vision.LastSeenPosition;
            agent.SetDestination(player.position);
        }
        else
        {
            agent.SetDestination(lastSeenPosition);
            currentState = EnemyState.MoveToLastSeen;
        }
    }

    void HandleMoveToLastSeen()
    {
        if (vision.CanSeePlayer)
        {
            EnterChase();
            return;
        }

        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance + 0.2f)
        {
            EnterSearch();
        }
    }

    void HandleSearch()
    {
        if (vision.CanSeePlayer)
        {
            EnterChase();
            return;
        }

        searchTimer -= Time.deltaTime;

        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            Vector3 randomPoint = lastSeenPosition + Random.insideUnitSphere * searchRadius;
            NavMeshHit hit;

            if (NavMesh.SamplePosition(randomPoint, out hit, searchRadius, NavMesh.AllAreas))
            {
                agent.SetDestination(hit.position);
            }
        }

        if (searchTimer <= 0f)
        {
            EnterPatrol();
        }
    }

    // ================= TRANSITIONS =================

    void EnterPatrol()
    {
        if (inCombat) return;

        currentState = EnemyState.Patrol;
        agent.speed = patrolSpeed;
        patrol.ResumePatrol();
    }


    void EnterChase()
    {
        currentState = EnemyState.Chase;
        patrol.StopPatrol();
        agent.speed = chaseSpeed;
        lastSeenPosition = vision.LastSeenPosition;
        agent.SetDestination(player.position);
    }

    void EnterSearch()
    {
        currentState = EnemyState.Search;
        searchTimer = searchDuration;
        agent.speed = patrolSpeed;
    }

    void SetEyeColor(Color color)
    {
        if (eyeRenderer != null)
            eyeRenderer.material.color = color;
    }

    public void PauseChase(bool pause)
    {
        isPaused = pause;
        inCombat = pause;
    }


    public void ResetState()
    {
        // Stop everything
        StopAllCoroutines();

        // Reset NavMeshAgent safely
        agent.isStopped = true;
        agent.ResetPath();

        // Teleport enemy back
        transform.position = startPosition;
        transform.rotation = startRotation;

        // Force NavMeshAgent to re-sync
        agent.Warp(startPosition);
        agent.isStopped = false;

        // Reset AI state
        currentState = EnemyState.Patrol;
        searchTimer = 0f;
        isPaused = false;

        // Resume patrol
        patrol.ResumePatrol();

        // Reset visuals
        SetEyeColor(patrolColor);
    }


}
