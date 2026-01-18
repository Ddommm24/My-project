using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using System.Linq;

public class TimeLoopManager : MonoBehaviour
{
    public static TimeLoopManager Instance;
    public const float SHIFT_CHANGE_TIME = 76f;
    private float elapsedTime;

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
        elapsedTime = 0f;

        playerStartPos = player.position;
        playerStartRot = player.rotation;
    }

    void Update()
    {
        float dt = Time.deltaTime;

        timeRemaining -= dt;
        elapsedTime += dt;

        if (timeRemaining <= 0f)
        {
            ResetLoop();
        }

        if (showTime)
        {
            timeText.text = GetFormattedTime();
        }
        if (Keyboard.current != null && Keyboard.current.vKey.wasPressedThisFrame)
        {
            ResetLoop();
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
        MonoBehaviour[] allMono = FindObjectsOfType<MonoBehaviour>(true);
        ILoopResettable[] resettables = allMono.OfType<ILoopResettable>().ToArray();
        foreach (var obj in resettables)
        {
            obj.ResetState();
        }

        timeRemaining = loopDuration;
        elapsedTime = 0f;
    }

    public void ResetLoopFromDeath()
    {
        Debug.Log("PLAYER DIED — RESETTING LOOP");
        ResetLoop();
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

    public float GetElapsedTime()
    {
        return elapsedTime;
    }

}
