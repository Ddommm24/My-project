using UnityEngine;
using System.Collections.Generic;

public class EntryRouteManager : MonoBehaviour
{
    public static EntryRouteManager Instance;

    private Dictionary<string, int> routeStages = new();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    public int GetRouteStage(string routeId)
    {
        if (!routeStages.ContainsKey(routeId))
            routeStages[routeId] = 0;

        return routeStages[routeId];
    }

    public void MarkRouteUsed(string routeId)
    {
        if (!routeStages.ContainsKey(routeId))
            routeStages[routeId] = 0;

        routeStages[routeId]++;
    }

    public bool IsRouteLocked(string routeId)
    {
        return GetRouteStage(routeId) >= 2;
    }

    public void ResetAll()
    {
        routeStages.Clear();
    }
}
