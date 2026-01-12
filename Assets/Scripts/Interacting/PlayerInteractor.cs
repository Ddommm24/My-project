using UnityEngine;
using TMPro;

public class PlayerInteractor : MonoBehaviour
{
    public float interactDistance = 3f;
    public LayerMask interactLayer;
    public TMP_Text promptText;

    private IInteractable current;

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
        if (promptText != null)
        {
            promptText.gameObject.SetActive(false);
        }
        
        current = null;

        Ray ray = new Ray(transform.position, transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, interactDistance, interactLayer))
        {
            IInteractable interactable = hit.collider.GetComponent<IInteractable>();
            if (interactable != null && promptText != null)
            {
                current = interactable;
                promptText.text = interactable.GetPromptText();
                promptText.gameObject.SetActive(true);
            }
        }
    }
}
