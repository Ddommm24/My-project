using UnityEngine;
using TMPro;

public class InventoryUI : MonoBehaviour
{
    public GameObject panel;
    public TMP_Text itemsText;

    bool open;

    void Start()
    {
        panel.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            Toggle();
        }
    }

    void Toggle()
    {
        open = !open;
        panel.SetActive(open);

        if (open)
            Refresh();
    }

    void Refresh()
    {
        if (PlayerInventory.Instance == null)
            return;

        itemsText.text = "";

        if (PlayerInventory.Instance.HasScrewdriver)
            itemsText.text += "- Screwdriver\n";

    if (PlayerInventory.Instance.HasAxe)
            itemsText.text += "- Axe\n";

        if (itemsText.text == "")
            itemsText.text = "Inventory Empty";
    }
}
