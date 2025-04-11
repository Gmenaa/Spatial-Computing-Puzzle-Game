using UnityEngine;

public class MirrorInteraction : MonoBehaviour
{
    public float rotationAngle = 22.5f;
    private bool isPlayerInRange = false;

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object entering the trigger is the player (VR Rig)
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
        }
    }

    void Update()
    {
        // If the player is in range and presses the A button on the right controller, rotate the mirror.
        if (isPlayerInRange && OVRInput.GetDown(OVRInput.Button.One))
        {
            RotateMirror();
        }
    }

    private void RotateMirror()
    {
        // Rotate the mirror by rotation angle degrees around Y axis.
        transform.Rotate(Vector3.up, rotationAngle, Space.Self);

        // TODO: Add haptic feedback
    }
}
