using UnityEngine;
using TMPro;

public class OpenDrawer : MonoBehaviour
{
    public TMP_Text promptText;

    [Header("Movement")]
    public Vector3 openOffset = new Vector3(0, 0, 0.35f);
    public float moveSpeed = 4f;

    [Header("Interaction")]
    public float interactDistance = 3f;

    private Camera cam;
    private Vector3 closedPos;
    private Vector3 openPos;
    private bool isOpen;
    private bool isMoving;

    void Start()
    {
        cam = Camera.main;
        closedPos = transform.localPosition;
        openPos = closedPos + openOffset;

        promptText.gameObject.SetActive(false);
    }

    void Update()
    {
        if (ReadBook.IsReading)
        return;

        CheckForLook();
    }

    void CheckForLook()
    {
        Debug.DrawRay(cam.transform.position, cam.transform.forward * interactDistance, Color.red);

        if (isMoving) return;

        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactDistance))
        {
            // 🔹 FIRST: check if we're looking at a book
            var book = hit.collider.GetComponentInParent<ReadBook>();
            if (book != null)
            {
                promptText.gameObject.SetActive(false);
                return;
            }

            // 🔹 THEN: check if we're looking at THIS drawer
            if (hit.collider.GetComponentInParent<OpenDrawer>() == this)
            {
                promptText.gameObject.SetActive(true);
                promptText.text = isOpen
                    ? "Press E to close drawer"
                    : "Press E to open drawer";

                if (Input.GetKeyDown(KeyCode.E))
                {
                    ToggleDrawer();
                }

                return;
            }
        }

        promptText.gameObject.SetActive(false);
    }

    void ToggleDrawer()
    {
        isOpen = !isOpen;
        StartCoroutine(MoveDrawer());
    }

    System.Collections.IEnumerator MoveDrawer()
    {
        isMoving = true;
        Vector3 target = isOpen ? openPos : closedPos;

        while (Vector3.Distance(transform.localPosition, target) > 0.001f)
        {
            transform.localPosition = Vector3.MoveTowards(
                transform.localPosition,
                target,
                moveSpeed * Time.deltaTime
            );

            yield return null;
        }

        transform.localPosition = target;
        isMoving = false;
    }
}
