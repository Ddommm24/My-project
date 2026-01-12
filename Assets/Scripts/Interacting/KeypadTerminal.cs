using UnityEngine; 
using TMPro;

public class KeypadTerminal : MonoBehaviour 
{ 
    public BigDoor door; 
    public TMP_Text terminalText; 

    [Header("Interaction")] 
    public float interactDistance = 3f; 

    private Camera cam; 
    private bool enteringCode; 
    private string currentInput = ""; 
    
    void Start() 
    { 
        cam = Camera.main; 
        terminalText.gameObject.SetActive(false);
    } 
    
    void Update() 
    { 
        if (!enteringCode) 
        { 
            CheckForLook();
        } 
        else 
        { 
            HandleCodeInput(); 
        } 
    }
    
    void CheckForLook() 
    { 
        Ray ray = new Ray(cam.transform.position, cam.transform.forward); 
        RaycastHit hit; 
        
        if (Physics.Raycast(ray, out hit, interactDistance))
        { 
            if (hit.collider.gameObject == gameObject) 
            { 
                terminalText.gameObject.SetActive(true);
                terminalText.text = "Press E to use terminal"; 
                
                if (Input.GetKeyDown(KeyCode.E)) 
                { 
                    EnterTerminal(); 
                } 

                return; 
            } 
        } 
        
        terminalText.gameObject.SetActive(false); 
    } 
    
    void EnterTerminal() 
    { 
        enteringCode = true; 
        currentInput = ""; 
        terminalText.text = "ENTER CODE:"; 
    } 
    
    void HandleCodeInput() 
    { 
        for (int i = 0; i <= 9; i++) 
        { 
            if (Input.GetKeyDown(i.ToString())) 
            { 
                if (currentInput.Length < 4) 
                { 
                    currentInput += i; 
                    terminalText.text = currentInput; 
                } 
            } 
        } 
        
        if (Input.GetKeyDown(KeyCode.Backspace) && currentInput.Length > 0) 
        {   
            currentInput = currentInput.Substring(0, currentInput.Length - 1);
            terminalText.text = currentInput;
        } 

        if (Input.GetKeyDown(KeyCode.Return)) 
        { 
            SubmitCode(); 
        } 

        if (Input.GetKeyDown(KeyCode.Escape)) 
        { 
            ExitTerminal(); 
        } 
    } 
    
    void SubmitCode() 
    { 
        enteringCode = false; 
        if (DoorCodeManager.Instance.CheckCode(currentInput)) 
        { 
            terminalText.text = "ACCESS GRANTED"; 
            door.OpenDoor(); 
        } 
        else 
        { 
            terminalText.text = "ACCESS DENIED"; 
            Invoke(nameof(ExitTerminal), 1.5f);
        } 
    } 
         
    void ExitTerminal() 
    { 
        enteringCode = false; 
        terminalText.gameObject.SetActive(false); 
    } 
}
