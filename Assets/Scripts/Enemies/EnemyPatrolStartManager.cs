using UnityEngine;
using System.Collections.Generic;

public class EnemyPatrolStartManager : MonoBehaviour, ILoopResettable
{
    public EnemyPatrol[] enemies;
    public int minWaypointSpacing = 3;

    List<int> usedIndices = new();

    void Start()
    {
        AssignRandomStartPoints();
    }

    public void AssignRandomStartPoints()
    {
        Debug.Log("AssignRandomStartPoints CALLED");

        usedIndices.Clear();

        if (enemies.Length == 0) return;

        int waypointCount = enemies[0].GetWaypointCount();
        if (waypointCount == 0) return;

        foreach (var enemy in enemies)
        {

            int index = GetSpacedRandomIndex(waypointCount);
            usedIndices.Add(index);

            Debug.Log($"{enemy.name} assigned random waypoint {index}");

            enemy.SetPatrolIndex(index);
            WarpEnemyToWaypoint(enemy, index);
            enemy.ResetState();
        }
    }

    void WarpEnemyToWaypoint(EnemyPatrol enemy, int index)
    {
        UnityEngine.AI.NavMeshAgent agent = enemy.GetComponent<UnityEngine.AI.NavMeshAgent>();
        EnemyAI ai = enemy.GetComponent<EnemyAI>();

        Transform wp = enemy.waypointParent.GetChild(index);

        if (agent != null && agent.enabled)
        {
            agent.Warp(wp.position);
        }
        else
        {
            enemy.transform.position = wp.position;
        }

        if (ai != null)
        {
            ai.SetHome(wp.position);   // optional
            ai.OverrideStartPosition(wp.position);
        }
    }


    int GetSpacedRandomIndex(int max)
    {
        const int MAX_ATTEMPTS = 50;

        for (int i = 0; i < MAX_ATTEMPTS; i++)
        {
            int candidate = Random.Range(0, max);
            bool tooClose = false;

            foreach (int used in usedIndices)
            {
                if (Mathf.Abs(candidate - used) < minWaypointSpacing)
                {
                    tooClose = true;
                    break;
                }
            }

            if (!tooClose)
                return candidate;
        }

        // Fallback if space is tight
        return Random.Range(0, max);
    }

    public void ResetState()
    {
        AssignRandomStartPoints();
    }
}
