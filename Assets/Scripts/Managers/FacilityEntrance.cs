using UnityEngine;

public class FacilityEntrance : MonoBehaviour, ILoopResettable
{
    public string routeId = "MainEntrance";

    [Header("Stage Objects")]
    public GameObject blockade;
    public GameObject chain;

    void Start()
    {
        ApplyStage();
    }

    void ApplyStage()
    {
        int stage = EntryRouteManager.Instance.GetRouteStage(routeId);

        // Stage 1 → blockade appears
        blockade.SetActive(stage >= 1);

        // Stage 2 → chain appears
        chain.SetActive(stage >= 2);

    }

    // Called when player actually enters
    public void OnPlayerEntered()
    {
        Debug.Log("player entered");

        EntryRouteManager.Instance.MarkRouteUsed(routeId);
    }

    public void ResetState()
    {
        Debug.Log("state resetted");

        ApplyStage();
    }
}
