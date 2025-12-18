using UnityEngine;

public class DoorButton : Interactable, ILoopResettable
{
    public DoorResettable door;

    private bool pressed;
    private Vector3 startLocalPos;

    void Start()
    {
        startLocalPos = transform.localPosition;
    }

    public override void Interact()
    {
        if (pressed) return;

        pressed = true;
        door.OpenDoor();

        // visual press feedback
        transform.localPosition += Vector3.down * 0.05f;
    }

    public void ResetState()
    {
        pressed = false;
        transform.localPosition = startLocalPos;
    }
}
