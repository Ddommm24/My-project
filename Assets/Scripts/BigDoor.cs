using UnityEngine;

public class BigDoor : MonoBehaviour
{
    private bool isOpen = false;

    public void OpenDoor()
    {
        if (isOpen) return;

        isOpen = true;

        // TEMP door open logic
        // Replace later with animation
        transform.position += Vector3.up * 5f;

        Debug.Log("BIG DOOR OPENED");
    }
}
