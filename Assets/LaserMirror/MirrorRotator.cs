using UnityEngine;

public class MirrorRotator : MonoBehaviour
{
    // The angle (in degrees) by which the mirror rotates per button press.
    public float rotationAngle = 22.5f;

    // Optional: Specify which button you want to trigger the rotation.
    // Here we're using OVRInput.Button.One (typically the "A" button on the right controller)
    public OVRInput.Button rotationButton = OVRInput.Button.One;

    void Update()
    {
        // Check if the specified button is pressed down (fires only once per press).
        if (OVRInput.GetDown(rotationButton))
        {
            RotateMirror();
        }
    }

    // Rotates the mirror by the specified angle about the Y axis.
    public void RotateMirror()
    {
        // Rotate around the Y axis. You can change the axis by modifying the vector if needed.
        transform.Rotate(Vector3.up, rotationAngle, Space.Self);
        
        // Optionally, you can provide feedback here (e.g. haptics or a sound effect) to signal that the rotation occurred.
    }
}
