using UnityEngine;

public class BreakableBlockade : MonoBehaviour, IInteractable, ILoopResettable
{
    public string requiredItemId = "Axe";
    public string routeId;

    public int Priority => 10;

    bool broken;

    public bool CanInteract()
    {
        if (broken) return false;

        return PlayerInventory.Instance.HasAxe;
    }

    public string GetPromptText()
    {
        return "Press E to Break Blockade";
    }

    public void Interact()
    {
        broken = true;
        gameObject.SetActive(false);

        EntryRouteManager.Instance.MarkRouteUsed(routeId);
    }

    public void ResetState()
    {
        broken = false;
        gameObject.SetActive(true);
    }
}
