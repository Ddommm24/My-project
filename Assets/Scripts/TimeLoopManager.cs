using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using System.Linq;

public class TimeLoopManager : MonoBehaviour
{
    public static TimeLoopManager Instance;

    [Header("Loop Settings")]
    public float loopDuration = 300f; // 5 minutes
    private float timeRemaining;

    [Header("Player Reset")]
    public Transform player;
    private Vector3 playerStartPos;
    private Quaternion playerStartRot;

    [Header("UI")]
    public TMP_Text timeText;
    private bool showTime;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        timeRemaining = loopDuration;

        // Save initial player state
        playerStartPos = player.position;
        playerStartRot = player.rotation;
    }

    void Update()
    {
        timeRemaining -= Time.deltaTime;

        if (timeRemaining <= 0f)
        {
            ResetLoop();
        }

        if (showTime)
        {
            timeText.text = GetFormattedTime();
        }
    }

    public void ShowTime(bool show)
    {
        showTime = show;

        if (timeText == null)
        {
            Debug.LogError("TimeText not assigned!");
            return;
        }

        timeText.gameObject.SetActive(show);

        if (show)
            timeText.text = GetFormattedTime();
    }


    public void ResetLoop()
    {
        Debug.Log("TIME LOOP RESET");

        timeRemaining = loopDuration;

        // Reset player
        CharacterController cc = player.GetComponent<CharacterController>();
        if (cc) cc.enabled = false;

        player.position = playerStartPos;
        player.rotation = playerStartRot;

        if (cc) cc.enabled = true;

        // Notify everything else
        MonoBehaviour[] allMono = FindObjectsOfType<MonoBehaviour>();
        ILoopResettable[] resettables = allMono.OfType<ILoopResettable>().ToArray();
        foreach (var obj in resettables)
        {
            obj.ResetState();
        }
    }

    public float GetTimeRemaining()
    {
        return timeRemaining;
    }

    public string GetFormattedTime()
    {
        int minutes = Mathf.FloorToInt(timeRemaining / 60);
        int seconds = Mathf.FloorToInt(timeRemaining % 60);
        return $"{minutes:00}:{seconds:00}";
    }


}
