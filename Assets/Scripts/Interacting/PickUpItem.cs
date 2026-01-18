using UnityEngine;

public class PickupItem : MonoBehaviour, IInteractable, ILoopResettable
{
    public string itemId;
    public string promptText = "Pick up item";

    [Header("Optional Route Lock")]
    public string routeId;
    public int disablePickupAtStage = 2;

    public int Priority => 10;

    private bool pickedUp;

    public bool CanInteract()
    {
        if (pickedUp)
            return false;

        if (!string.IsNullOrEmpty(routeId))
        {
            int stage = EntryRouteManager.Instance.GetRouteStage(routeId);
            if (stage >= disablePickupAtStage)
                return false;
        }

        return true;
    }

    public string GetPromptText()
    {
        if (!string.IsNullOrEmpty(routeId) &&
            EntryRouteManager.Instance.IsRouteLocked(routeId))
        {
            return "";
        }

        return promptText;
    }

    public void Interact()
    {
        if (!string.IsNullOrEmpty(routeId) &&
            EntryRouteManager.Instance.IsRouteLocked(routeId))
            return;

        PlayerInventory.Instance.AddItem(itemId);
        pickedUp = true;

        gameObject.SetActive(false);
    }

    public void ResetState()
    {
        pickedUp = false;

        gameObject.SetActive(true);   // <-- REQUIRED

        Collider col = GetComponent<Collider>();
        if (col != null)
            col.enabled = true;
    }
}
