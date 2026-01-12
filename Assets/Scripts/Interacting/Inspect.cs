using UnityEngine; 
using TMPro;

public class Inspect : MonoBehaviour 
{ 
    public TMP_Text inspectText;
    public TMP_Text descriptionText;
    private bool inspecting = false;
    [TextArea(5, 10)]
    public string description = "Default";

    [Header("Interaction")] 
    public float interactDistance = 3f; 

    private Camera cam; 
    
    void Start() 
    { 
        cam = Camera.main; 
        inspectText.gameObject.SetActive(false);
        descriptionText.gameObject.SetActive(false);
    } 
    
    void Update() 
    { 
        CheckForLook();

        if (inspecting && Input.GetKeyDown(KeyCode.Escape))
        {
            ExitInspect();
        }
    }
    
    void CheckForLook() 
    { 
        if (inspecting) return;

        Ray ray = new Ray(cam.transform.position, cam.transform.forward); 
        RaycastHit hit; 
        
        if (Physics.Raycast(ray, out hit, interactDistance))
        { 
            if (hit.collider.GetComponentInParent<Inspect>() == this)
            { 
                inspectText.gameObject.SetActive(true);
                inspectText.text = "Press E to Inspect"; 
                
                if (Input.GetKeyDown(KeyCode.E)) 
                { 
                    InspectItem(); 
                } 

                return; 
            } 
        } 
        
        inspectText.gameObject.SetActive(false); 
    } 
    
    void InspectItem() 
    { 
        inspecting = true; 
        inspectText.gameObject.SetActive(false);
        descriptionText.gameObject.SetActive(true);
        descriptionText.text = description;
    } 

    void ExitInspect()
    {
        inspecting = false;
        inspectText.gameObject.SetActive(false);
        descriptionText.gameObject.SetActive(false);
    }

}
