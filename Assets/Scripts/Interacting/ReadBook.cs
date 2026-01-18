using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ReadBook : MonoBehaviour, IInteractable
{
    public static bool IsReading { get; private set; }

    public int Priority => 10;

    public GameObject bookUI;
    public Image pageImage;
    public TMP_Text captionText;
    public TMP_Text pageNumberText;

    public int codeIndex = 3; // the book gives the 4th number

    public Sprite[] pages;
    public string[] captions;

    private int page;

    void Start()
    {
        bookUI.SetActive(false);
    }

    public bool CanInteract()
    {
        return !IsReading;
    }

    public string GetPromptText()
    {
        return "Press E to Read Book";
    }

    public void Interact()
    {
        IsReading = true;
        page = 0;

        bookUI.SetActive(true);
        UpdatePage();

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void Update()
    {
        if (!IsReading) return;

        if (Input.GetKeyDown(KeyCode.RightArrow)) NextPage();
        if (Input.GetKeyDown(KeyCode.LeftArrow)) PrevPage();

        if (Input.GetKeyDown(KeyCode.Escape))
            CloseBook();
    }

    void NextPage()
    {
        if (page < pages.Length - 1)
        {
            page++;
            UpdatePage();
        }
    }

    void PrevPage()
    {
        if (page > 0)
        {
            page--;
            UpdatePage();
        }
    }

    void UpdatePage()
    {
        pageImage.sprite = pages[page];

        if (page == pages.Length - 1 && DoorCodeManager.Instance != null)
        {
            int number = DoorCodeManager.Instance.GetNumber(0);
            captionText.text =
                $"1990 - Our Holiday at Hotel {number}";
        }
        else
        {
            captionText.text = captions[page];
        }

        pageNumberText.text = $"{page + 1} / {pages.Length}";
    }


    void CloseBook()
    {
        IsReading = false;
        bookUI.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
