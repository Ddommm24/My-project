using UnityEngine;
using TMPro;

public class PlayerInteractor : MonoBehaviour
{
    public float interactDistance = 3f;
    public LayerMask interactLayer;
    public TMP_Text promptText;

    public Camera playerCamera;

    private IInteractable current;
    
    void Start()
    {
        if (playerCamera == null)
            playerCamera = Camera.main;
    }

    void Update()
    {
        CheckInteractable();

        if (current != null && Input.GetKeyDown(KeyCode.E))
        {
            current.Interact();
        }        
    }

    void CheckInteractable()
    {
        if (ReadBook.IsReading)
            return;

        current = null;
        promptText.gameObject.SetActive(false);

        if (playerCamera == null)
            return;

        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);

        //int mask = interactLayer;

        if (Physics.Raycast(ray, out RaycastHit hit, interactDistance, interactLayer, QueryTriggerInteraction.Collide))
        {

            IInteractable[] interactables = hit.collider.GetComponentsInParent<IInteractable>();

            IInteractable best = null;
            int bestPriority = int.MinValue;

            foreach (var i in interactables)
            {
                if (!i.CanInteract()) continue;

                if (i.Priority > bestPriority)
                {
                    bestPriority = i.Priority;
                    best = i;
                }
            }

            if (best != null)
            {
                current = best;
                promptText.text = best.GetPromptText();
                promptText.gameObject.SetActive(true);
            }
        }
    }
}
