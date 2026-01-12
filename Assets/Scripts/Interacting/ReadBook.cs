using UnityEngine;
using TMPro;
using UnityEngine.UI;

[System.Serializable]
public class BookPage
{
    public Sprite image;

    [TextArea(3, 6)]
    public string caption;
}

public class ReadBook : MonoBehaviour
{
    public static bool IsReading { get; private set; }

    [Header("UI")]
    public TMP_Text promptText;
    public GameObject bookUI;
    public Image pageImage;
    public TMP_Text captionText;
    public TMP_Text pageIndicator;

    [Header("Pages")]
    public BookPage[] pages;

    [Header("Interaction")]
    public float interactDistance = 3f;

    private Camera cam;
    private int currentPage;

    void Start()
    {
        cam = Camera.main;
        promptText.gameObject.SetActive(false);
        bookUI.SetActive(false);
    }

    void Update()
    {
        if (!IsReading)
            CheckForLook();
        else
            HandleReadingInput();
    }

    void CheckForLook()
    {
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactDistance))
        {
            if (hit.collider.GetComponentInParent<ReadBook>() == this)
            {
                promptText.gameObject.SetActive(true);
                promptText.text = "Press E to read book";

                if (Input.GetKeyDown(KeyCode.E))
                    StartReading();

                return;
            }
        }

        promptText.gameObject.SetActive(false);
    }

    void StartReading()
    {
        IsReading = true;
        currentPage = 0;

        promptText.gameObject.SetActive(false);
        bookUI.SetActive(true);
        UpdatePage();

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void HandleReadingInput()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
            NextPage();

        if (Input.GetKeyDown(KeyCode.LeftArrow))
            PreviousPage();

        if (Input.GetKeyDown(KeyCode.Escape))
            ExitReading();
    }

    void NextPage()
    {
        if (currentPage < pages.Length - 1)
        {
            currentPage++;
            UpdatePage();
        }
    }

    void PreviousPage()
    {
        if (currentPage > 0)
        {
            currentPage--;
            UpdatePage();
        }
    }

    void UpdatePage()
    {
        pageImage.sprite = pages[currentPage].image;
        captionText.text = pages[currentPage].caption;

        if (pageIndicator != null)
            pageIndicator.text = $"Page {currentPage + 1} / {pages.Length}";
    }

    void ExitReading()
    {
        IsReading = false;
        bookUI.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
