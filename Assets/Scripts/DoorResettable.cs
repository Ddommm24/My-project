using UnityEngine;

public class DoorResettable : MonoBehaviour, ILoopResettable
{
    public Transform door;
    public Vector3 openOffset = new Vector3(0, 3f, 0);
    public float openSpeed = 3f;

    private Vector3 closedPos;
    private Vector3 openPos;
    private bool isOpen;

    void Start()
    {
        closedPos = door.position;
        openPos = closedPos + openOffset;
    }

    void Update()
    {
        Vector3 target = isOpen ? openPos : closedPos;
        door.position = Vector3.MoveTowards(
            door.position,
            target,
            openSpeed * Time.deltaTime
        );
    }

    public void OpenDoor()
    {
        isOpen = true;
    }

    public void ResetState()
    {
        isOpen = false;
        door.position = closedPos;
    }
}
