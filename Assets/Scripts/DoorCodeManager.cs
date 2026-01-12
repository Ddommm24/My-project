using UnityEngine;

public class DoorCodeManager : MonoBehaviour
{
    public static DoorCodeManager Instance;

    public string doorCode; // 4-digit code, stored as string

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            GenerateCode();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void GenerateCode()
    {
        doorCode = "";

        for (int i = 0; i < 4; i++)
        {
            doorCode += Random.Range(0, 10); // 0–9
        }

        Debug.Log("Generated Door Code: " + doorCode);
    }

    public bool CheckCode(string input)
    {
        return input == doorCode;
    }
}
